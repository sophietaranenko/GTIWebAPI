using System;
using System.Collections.Generic;

using System.Net.Http;

using System.Security.Claims;
using System.Security.Cryptography;

using System.Threading.Tasks;

using System.Web;
using System.Web.Http;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;

using GTIWebAPI.Providers;
using GTIWebAPI.Results;

using GTIWebAPI.Models.Account;
using GTIWebAPI.Models.Security;
using GTIWebAPI.Models.Organizations;
using GTIWebAPI.Filters;
using System.Net;
using System.Linq;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Novell;

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Controller for manipulating with user account
    /// </summary>
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {

        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;
        private int SMSCode { get; set; }

        /// <summary>
        /// ctor empty
        /// </summary>
        public AccountController()
        {
            factory = new DbContextFactory();
        }

        public AccountController(ApplicationUserManager userManager)
        {
            factory = new DbContextFactory();
            UserManager = userManager;
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="accessTokenFormat"></param>
        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
            factory = new DbContextFactory();
        }

        private IDbContextFactory factory { get; set; }

        protected ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }



        [Authorize]
        [Route("UserInfo")]
        public async Task<UserInfoViewModel> GetUserInfo()
        {
            string UserId = User.Identity.GetUserId();
            //var result = UserManager.FindByIdAsync(;
            ApplicationUser user = await UserManager.FindByIdAsync(UserId);

          //  ApplicationUser user = UserManager.FindById(UserId);

            UserInfoViewModel model = new UserInfoViewModel();
            if (user != null)
            {
                string profilePicturePath = null;
                bool employeeInformation = false;
                UserImage im = user.Image;

                if (im != null)
                {
                    if (im.ImageName != null && im.ImageName != "")
                    {
                        profilePicturePath = im.ImageName;
                    }
                }

                if (user.TableName == "Employee")
                {
                    using (IAppDbContext db = factory.CreateDbContext())
                    {
                        employeeInformation = db.IsEmployeeInformationFilled(user.TableId);
                    }
                }
                if (user.TableName == "OrganizationContactPerson")
                {
                    using (IAppDbContext db = factory.CreateDbContext())
                    {
                        model.OrganizationId = 
                            db.OrganizationContactPersons
                            .Where(d => d.Id == user.TableId)
                            .Select(d => d.OrganizationId)
                            .FirstOrDefault()
                            .GetValueOrDefault();
                    }
                }

                model.UserName = user.UserName;
                model.TableId = user.TableId;
                model.TableName = user.TableName;
                model.ProfilePicturePath = profilePicturePath;
                model.UserRights = user.GetUserRightsDTO();
                model.EmployeeInformation = employeeInformation;
            }
            return await Task<UserInfoViewModel>.Factory.StartNew(() => model);
            //return model;
        }

        /// <summary>
        /// Method for profile picture upload
        /// </summary>
        /// <param name="tableName">Image of which document we're uploading</param>
        /// <param name="tableId">Id of document which image we're uploading</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("UploadProfilePicture")]
        public HttpResponseMessage UploadProfilePicture(string tableName, int tableId)
        {

            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                ApplicationDbContext db = new ApplicationDbContext();
                string userId = User.Identity.GetUserId();
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filePath = HttpContext.Current.Server.MapPath(
                        "~/PostedFiles/" + db.FileNameUnique().ToString().Trim() + "_" + postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    UserImage image = 
                        db.UserImage
                        .Where(i => i.UserId == userId)
                        .FirstOrDefault();
                    if (image == null)
                    {
                        image = new UserImage();
                        image.UserId = User.Identity.GetUserId();
                        image.ImageName = filePath;
                        db.UserImage.Add(image);
                    }
                    else
                    {
                        image.ImageName = filePath;
                        db.Entry(image).State = System.Data.Entity.EntityState.Modified;
                    }

                    db.SaveChanges();
                }

                UserImage newImage = 
                    db.UserImage
                    .Where(i => i.UserId == userId)
                    .FirstOrDefault();

                if (newImage != null)
                {
                    result = Request.CreateResponse(HttpStatusCode.Created, newImage);
                }
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return result;
        }

        /// <summary>
        /// Get User rights 
        /// </summary>
        /// <returns></returns>
        [Route("UserRights")]
        public IEnumerable<UserRightDTO> GetUserRights()
        {
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            return user.GetUserRightsDTO();
        }


        /// <summary>
        /// Get information about user
        /// </summary>
        /// <returns>UserIfoViewModel</returns>
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfoExternalLogin")]
        public UserInfoViewModel GetUserExternalLoginInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);
            return new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns>200</returns>
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);

            var authentication = HttpContext.Current.GetOwinContext().Authentication;
            authentication.SignOut(DefaultAuthenticationTypes.ExternalBearer);

            return Ok();
        }

        /// <summary>
        /// Get Manage Info 
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <param name="generateState"></param>
        /// <returns></returns>
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }



        /// <summary>
        /// Set passport
        /// </summary>
        /// <param name="model"></param>
        /// <returns>200</returns>
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }





        /// <summary>
        /// Simple register of organization contact person 
        /// </summary>
        /// <param name="organizationContactPersonId"></param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("SimpleRegisterOrganizationContactPerson")]
        public async Task<IHttpActionResult> SimpleRegisterOrganizationContactPerson(int organizationContactPersonId)
        {
            if (organizationContactPersonId != 0)
            {
                OrganizationContactPersonView person = null;
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    person = db.OrganizationContactPersonViews.Find(organizationContactPersonId);
                }

                if (person == null || person.Deleted == true)
                {
                    return BadRequest("Organization contact person doesn't exist");
                }
                if (person.IsRegistered == true)
                {
                    return BadRequest("Organization contact person is already registered as an API  user");
                }
                if (person.Email == null || person.Email == "")
                {
                    return BadRequest("Organization contact person email is empty");
                }
                NovellOrganizationContactPerson novellPerson;
                try
                {
                    novellPerson = new NovellOrganizationContactPerson(person);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }

                ApplicationUser user = null;

                bool novellResult = NovellManager.CreateOrganization(novellPerson);

                if (novellResult)
                {
                    using (ApplicationDbContext iDb = new ApplicationDbContext())
                    {
                        bool dbResult = iDb.CreateOrganization(novellPerson.Login, novellPerson.Password);
                    

                        if (dbResult)
                        {
                            user = new ApplicationUser()
                            {
                                UserName = novellPerson.Login,
                                Email = novellPerson.Email,
                                TableName = "OrganizationContactPerson",
                                TableId = organizationContactPersonId
                            };
                            try
                            {
                                IdentityResult result = await UserManager.CreateAsync(user, novellPerson.Password);
                                if (!result.Succeeded)
                                {
                                    return GetErrorResult(result);
                                }
                                else
                                {
                                    IdentityResult roleResult = UserManager.AddToRole(user.Id, "Organization");

                                    if (!roleResult.Succeeded)
                                    {
                                        return GetErrorResult(roleResult);
                                    }

                                    bool rightsResult = UserRightsManager.GrantOrganizationRights(user.Id);
                                    if (rightsResult)
                                    {
                                        await UserManager.
                                            SendEmailAsync(user.Id,
                                            "Register user",
                                            @"You have successfully registered for WEBSITE_URL. <br>
                                            Thank you for your interest and we hope you will find useful information! <br> 
                                            Your credentials in WEBSITE_URL: <br> 
                                            login: " + novellPerson.Login + "<br>" +
                                            "password: " + novellPerson.Password + "<br>" +
                                            @"информация, которую мы знаем о контакнтом лице организации
                                            тут реклама, пара картинок");
                                        return Ok();
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                string m = e.Message;
                                return BadRequest("AspNetUser has not been created");
                            }
                        }
                        else
                        {
                            return BadRequest("Database login has not been created");
                        }
                    }

                }
                else
                {
                    return BadRequest("eDirectory login has not been created");
                }

            }


            return BadRequest("Wrong organization contact person");
        }





        /// <summary>
        /// GetExtenal login (not useful for GTI)
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                   OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                Authentication.SignIn(identity);
            }

            return Ok();
        }

        /// <summary>
        /// Get external logins (not useful for GTI)
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <param name="generateState"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }

        /// <summary>
        /// Register (not useful fro GTI)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        /// <summary>
        /// RegisterExternal (not useful for GTI)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return InternalServerError();
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            result = await UserManager.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            return Ok();
        }


        /// <summary>
        /// Dispose AccountConrtoller (to destroy connections)
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
