using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using GTIWebAPI.Models.Account;
using GTIWebAPI.NovelleDirectory;
using GTIWebAPI.Models.Context;
using Microsoft.Owin;
using System.Net;
using GTIWebAPI.Models.Security;
using System.Web.Http.Cors;
using NLog;
using GTIWebAPI.NovellGroupWiseSOAP;
using GTIWebAPI.Exceptions;

namespace GTIWebAPI.Providers
{

    public class Constants
    {
        public const string OwinChallengeFlag = "X-Challenge";
    }
    public class AuthenticationMiddleware : OwinMiddleware
    {
        public AuthenticationMiddleware(OwinMiddleware next) : base(next) { }

        public override async Task Invoke(IOwinContext context)
        {
            await Next.Invoke(context);
            if (context.Response.StatusCode == 400 && context.Response.Headers.ContainsKey(Constants.OwinChallengeFlag))
            {
                var headerValues = context.Response.Headers.GetValues(Constants.OwinChallengeFlag);
                context.Response.StatusCode = Convert.ToInt16(headerValues.FirstOrDefault());
                context.Response.Headers.Remove(Constants.OwinChallengeFlag);
            }

        }
    }

    // [EnableCors(origins: "*", headers: "*", methods: "GET, POST, PUT, DELETE, OPTIONS", SupportsCredentials = true)]
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;
        private INovelleDirectory novell;
        private INovellGroupWise novellGroupWise;

        //если вернемся к теме логгирования 
        //инициализация 
        //private static Logger logger = LogManager.GetCurrentClassLogger();
        //лог 
        //logger.Log(LogLevel.Info, "Credentials correct in Novell", context.UserName, context.Password);



        /// <summary>
        /// Ctor of oAuth provider
        /// </summary>
        /// <param name="publicClientId"></param>
        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }
            _publicClientId = publicClientId;
            novell = new NovelleDirectory.NovelleDirectory();
            novellGroupWise = new NovellGroupWise();
        }

        public ApplicationOAuthProvider(INovelleDirectory novell, INovellGroupWise novellGroupWise)
        {
            this.novell = novell;
            this.novellGroupWise = novellGroupWise;
        }

        /// <summary>
        /// First - NovellProvider, then OAuth Provider
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
            try
            {
                NovellUser novelleDirectoryUser = novell.Connect(context.UserName.Trim(), context.Password.Trim());
                ApplicationUser applicationUser = userManager.Find(context.UserName.Trim(), context.Password.Trim());
                //Особенности нашего обращения с Novell, ничего не попишешь 
                if (applicationUser == null)
                {
                    //на случай, если пароль поменяли 
                    applicationUser = await ChangePassword(context.UserName.Trim(), context.Password.Trim(), userManager);
                }
                if (applicationUser == null)
                {
                    //если человек есть в Novell eDirectory, но нет в AspNetUSers 
                    applicationUser = CreateUser(context.UserName.Trim(), context.Password.Trim(), novelleDirectoryUser, userManager);
                }

                if (!novelleDirectoryUser.IsAlien)
                {
                    NovellGroupWisePostOfficeConnection postOfficeConnection = novellGroupWise.Connect(context.UserName.Trim(), context.Password.Trim());
                    applicationUser.PostOfficeAddress = postOfficeConnection.PostOffice;
                    applicationUser.GroupWiseSessionId = postOfficeConnection.SessionId;
                }

                ClaimsIdentity oAuthIdentity = await applicationUser.GenerateUserIdentityAsync(userManager,
               OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookiesIdentity = await applicationUser.GenerateUserIdentityAsync(userManager,
                    CookieAuthenticationDefaults.AuthenticationType);


                AuthenticationProperties properties = CreateProperties(applicationUser.UserName);
                AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
                context.Validated(ticket);
                context.Request.Context.Authentication.SignIn(cookiesIdentity);
            }
            catch (NovelleDirectoryException nede)
            {
                //если человека нет в Novell - это и только это показатель того, что его никуда не надо пускать 
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                context.Response.Headers.Add(Constants.OwinChallengeFlag, new[] { ((int)HttpStatusCode.Unauthorized).ToString() });
                return;
            }
            catch (NovellGroupWiseException ngwe)
            {
                context.SetError("invalid_grant", ngwe.Message);
                context.Response.Headers.Add(Constants.OwinChallengeFlag, new[] { ((int)HttpStatusCode.Unauthorized).ToString() });
            }
            catch (FailedDatabaseConnectionException fdce)
            {
                context.SetError("invalid_grant", fdce.Message);
                context.Response.Headers.Add(Constants.OwinChallengeFlag, new[] { ((int)HttpStatusCode.Unauthorized).ToString() });
            }
            catch (Exception e)
            {
                context.SetError("invalid_grant", e.Message);
                context.Response.Headers.Add(Constants.OwinChallengeFlag, new[] { ((int)HttpStatusCode.Unauthorized).ToString() });
            }

        }

        private ApplicationUser CreateEmployeeApplicationUser(ApplicationUserManager userManager, string username, string password, string email)
        {
            int employeeId = CreateEmployee();
            ApplicationUser newUser = new ApplicationUser
            {
                UserName = username,
                Email = email,
                TableName = "Employee",
                TableId = employeeId
            };
            try
            { 
                userManager.Create(newUser, password);
            }
            catch (Exception e)
            {
                throw new FailedDatabaseConnectionException("Cannot create application user");
            }
            return newUser;
        }

        private async Task<ApplicationUser> ChangePassword(string username, string password, ApplicationUserManager userManager)
        {
            ApplicationUser user = userManager.FindByName(username);
            if (user != null)
            {
                String userId = user.Id;
                String hashedNewPassword = userManager.PasswordHasher.HashPassword(password);
                UserStore<ApplicationUser> store = new UserStore<ApplicationUser>();
                await store.SetPasswordHashAsync(user, hashedNewPassword);
            }
            return user;
        }

        private ApplicationUser CreateUser(string username, string password, NovellUser novellUser, ApplicationUserManager userManager)
        {

            bool dbResult = false;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                dbResult = db.CreateHoldingUser(username, password);
                if (!dbResult)
                { 
                throw new FailedDatabaseConnectionException("Cannot create database login");
                }
            }

            ApplicationUser user = CreateEmployeeApplicationUser(userManager, username, password, novellUser.Attributes["mail"][0]);
            userManager.AddToRole(user.Id, "Personnel");
            bool rightsResult = false;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                rightsResult = db.GrantStandardRightsToPersonnel(user.Id);
            }
            if (!rightsResult)
            {
                throw new FailedDatabaseConnectionException("Cannot create user rights");
            }
            return user;
        }

        private int CreateEmployee()
        {
            int employeeId = 0;
            try
            {
                using (SecureEmployeeCreatorDbContext db = new SecureEmployeeCreatorDbContext())
                {
                    employeeId = db.CreateEmployee();
                }
            }
            catch (Exception e)
            {
                throw new FailedDatabaseConnectionException("Cannot create employee");
            }
            return employeeId;
        }

        /// <summary>
        /// Where dp we get the token 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// If everything is OK, we mark context as validated 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (context.ClientId == null)
            {
                context.Validated();
            }
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Where do we need to return 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Creating application properties (adding username to context)
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }
}