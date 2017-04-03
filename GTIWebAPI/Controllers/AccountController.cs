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
using GTIWebAPI.Novell;
using GTIWebAPI.Models.Repository.Identity;
using GTIWebAPI.Exceptions;
using System.IO;

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

        private INovellManager novell { get; set; }

        private IAccountRepository repo { get; set; }

        public AccountController()
        {
            novell = new NovellManager();
            repo = new AccountRepository();
        }

        public AccountController(IAccountRepository repo, INovellManager novell, ApplicationUserManager userManager)
        {
            UserManager = userManager;
            this.novell = novell;
            this.repo = repo;
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

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
            ApplicationUser user = await UserManager.FindByIdAsync(UserId);
            UserInfoViewModel model = new UserInfoViewModel();
            if (user != null)
            {
                model.Email = user.Email;
                model.UserName = user.UserName;
                model.TableId = user.TableId;
                model.TableName = user.TableName;
                model.UserRights = user.GetUserRightsDTO();
                model.ProfilePicturePath = repo.GetProfilePicturePathByUserId(UserId);
                if (user.TableName == "Employee")
                {
                    model.EmployeeInformation = repo.IsEmployeeInformationFilled(user.TableId);
                }
                if (user.TableName == "OrganizationContactPerson")
                {
                    model.OrganizationId = repo.GetOrganizationIdOfPerson(user.TableId);
                }
            }
            return await Task<UserInfoViewModel>.Factory.StartNew(() => model);
        }


        [GTIFilter]
        [HttpPost]
        [Route("UploadProfilePicture")]
        public IHttpActionResult UploadNewProfilePicture()
        {
            UserImage image = new UserImage();
            var httpRequest = HttpContext.Current.Request;
            try
            {
                if (httpRequest.Files.Count > 0)
                {
                    ApplicationDbContext db = new ApplicationDbContext();
                    string userId = User.Identity.GetUserId();
                    foreach (string file in httpRequest.Files)
                    {
                        string filePath = repo.SaveFile(httpRequest.Files[file]);
                        image = repo.AddNewProfilePicture(
                            new UserImage
                            {
                                ImageName = filePath,
                                IsProfilePicture = true,
                                UploadDate = DateTime.Now,
                                UserId = User.Identity.GetUserId()
                            });
                    }
                }
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(image);
        }


        [GTIFilter]
        [HttpPost]
        [Route("SetAProfilePicture")]
        public IHttpActionResult SetAsProfilePicture(int pictureId)
        {
            UserImage image = repo.SetAsProfilePicture(pictureId);
            return Ok(image);
        }


        [Route("UserRights")]
        public IEnumerable<UserRightDTO> GetUserRights()
        {
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            return user.GetUserRightsDTO();
        }


        //[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        //[Route("UserInfoExternalLogin")]
        //public UserInfoViewModel GetUserExternalLoginInfo()
        //{
        //    ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);
        //    return new UserInfoViewModel
        //    {
        //        Email = User.Identity.GetUserName(),
        //        HasRegistered = externalLogin == null,
        //        LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
        //    };
        //}

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


                person = repo.FindPerson(organizationContactPersonId);

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


                INovellOrganizationContactPerson novellPerson;
                try
                {
                    novellPerson = new NovellOrganizationContactPerson(person);
                    novellPerson.Login = novell.GenerateLogin(novellPerson.Login);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }


                bool novellResult = novell.CreateOrganization(novellPerson);
                if (novellResult)
                {
                        bool dbResult = repo.CreateOrganization(novellPerson.Login, novellPerson.Password);
                        if (dbResult)
                        {
                        ApplicationUser user = new ApplicationUser()
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
                                        await SendEmailWithCredentials(novellPerson, user.Id);
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
                else
                {
                    return BadRequest("eDirectory login has not been created");
                }

            }


            return BadRequest("Wrong organization contact person");
        }

        public async Task SendEmailWithCredentials(INovellOrganizationContactPerson novellPerson, string userId)
        {
            string letterText = File.ReadAllText(HttpContext.Current.Server.MapPath("~/HtmlMailFile/letter.html"));


            await UserManager.SendEmailAsync(userId, "Регистрация в кабинете клиента GTI", letterText);

           // await UserManager.SendEmailAsync(userId,
                                           // "Регистрация",
                                           // "<div style=\"background: #fcfcfc; color: #4d4d4d\"><h2 style = \"margin-bottom: 10px;\"> Вы были успешно зарегестрированны в <a href = \"https://wwww.gtiweb.formag-group.com\" style = \"color: #61bc30 \"> клиентской версии GTI </a></h2><h4 style = \"margin-top: 5px;\"> Для доступа к кабинету клиента можно использовать следующие данные для входа на сайт: </h4><p style = \"padding-left: 30px\"> login: <strong>  " + novellPerson.Login + " </strong><br/> password: <strong> " + novellPerson.Password + " </strong></p></div>");
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
