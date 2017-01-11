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
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Clients;
using GTIWebAPI.Filters;
using System.Net;
using GTIWebAPI.Models;
using System.Linq;

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
        }

        /// <summary>
        /// property UserManager
        /// </summary>
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

        /// <summary>
        /// AccessTokenFormat
        /// </summary>
        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            string UserId = User.Identity.GetUserId();
            ApplicationUser user = UserManager.FindById(UserId);
            UserInfoViewModel model = new UserInfoViewModel();
            if (user != null)
            {
                //GTIUser gtiUser = GetGTIUser(UserId);
                string profilePicturePath = null;
                UserImage im = user.Image;

                if (im != null)
                {
                    if (im.ImageName != null && im.ImageName != "")
                    {
                        profilePicturePath = im.ImageName;
                    }
                }
                model.UserName = user.UserName;
                model.TableId = user.TableId;
                model.TableName = user.TableName;
                model.ProfilePicturePath = profilePicturePath;
                model.UserRights = user.UserRightsDto;
            }
            return model;
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

                    UserImage image = db.UserImage.Where(i => i.UserId == userId).FirstOrDefault();
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

                UserImage newImage = db.UserImage.Where(i => i.UserId == userId).FirstOrDefault();
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
            return user.UserRightsDto;
        }


        /// <summary>
        /// Get information about user
        /// </summary>
        /// <returns>UserIfoViewModel</returns>
        // GET api/Account/UserInfo
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
        // POST api/Account/Logout
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
        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
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
        // POST api/Account/SetPassword
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


        //[AllowAnonymous]
        //[HttpPost]
        //[Route("ForgotPassword")]
        //public async Task<IHttpActionResult> ClientPassword(string userName)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await UserManager.FindByNameAsync(userName);
        //        if (user == null)
        //        {
        //            return Ok();
        //        }
        //        string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

        //        string url = Url.Link(
        //            "DefaultApi",
        //            new
        //            {
        //                controller = "Account/ResetPassword/",
        //                UserId = user.Id,
        //                PasswordResetToken = code
        //            }
        //        );
        //        await UserManager.
        //            SendEmailAsync(user.Id, "Set Password", "Please set your password by clicking here: <a href=\"" + url + "\">link</a>");
        //        return Ok();
        //    }
        //    return BadRequest();
        //}



        //[AllowAnonymous]
        //[HttpGet]
        //[Route("ResetPassword", Name = "ResetPassword")]
        //public IHttpActionResult ResetPassword(string UserId, string PasswordResetToken)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    ChangePasswordBindingModel model = new ChangePasswordBindingModel
        //    {
        //        ConfirmPassword = null,
        //        OldPassword = PasswordResetToken
        //    };
        //    return Ok(model);
        //}


        //[AllowAnonymous]
        //[Route("PostResetPassword", Name = "PostResetPassword")]
        //public async Task<IHttpActionResult> PostResetPassword(ChangePasswordBindingModel model)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
        //        model.NewPassword);

        //    if (!result.Succeeded)
        //    {
        //        return GetErrorResult(result);
        //    }

        //    return Ok();
        //}


        //private string GenerateClientUsername()
        //{
        //    string name = "";
        //    using (DbClient db = new DbClient())
        //    {
        //        name = db.ClientUserNameGenerator();
        //    }
        //    return name;
        //}
        private string GenerateClientPassword()
        {
            return System.Web.Security.Membership.GeneratePassword(8, 0);
        }

        private bool CheckLoginExists(string login)
        {
            bool result = true;
            if (login != null && login != "")
            {
                ApplicationUser user = UserManager.FindByName(login);
                if (user == null)
                {
                    result = false;
                }
            }
            return result;
        }

        private bool CheckEmailExists(string email)
        {
            bool result = true;
            if (email != null && email != "")
            {
                ApplicationUser user = UserManager.FindByEmail(email);
                if (user == null)
                {
                    result = false;
                }
            }
            return result;
        }



        [AllowAnonymous]
        [HttpGet]
        [Route("SimpleRegisterClientContact")]
        public async Task<IHttpActionResult> SimpleRegisterClientContact(int clientContactId, string email)
        {
            if (clientContactId != 0)
            {
                //РАЗКОММЕНТИРОВАТЬ ОБЯЗАТЕЛЬНО
                //if (CheckEmailExists(email))
                //{
                //    return BadRequest("Email already exists");
                //}

                string username = email;
                string password = GenerateClientPassword();

                ApplicationUser user = new ApplicationUser()
                {
                    UserName = username,
                    Email = email,
                    TableName = "ClientContact",
                    TableId = clientContactId
                };

                IdentityResult result = await UserManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                if (user == null)
                {
                    return BadRequest();
                }

                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                   // GenerateEmailConfirmationTokenAsync(user.Id);
                var callbackUrl = Url.Link("DefaultApi", new
                {
                    Controller = "Account/SimplePasswordReset/",
                    PasswordResetToken = code,
                    UserId = user.Id
                });
                //ADD
                //отсылка sms с кодом
                //ADD
                await UserManager.
                    SendEmailAsync(user.Id, "Register user", "Please register your login and set new password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
                return Ok();

            }
            return BadRequest();
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("SimplePasswordReset")]
        public IHttpActionResult SimplePasswordReset(string PasswordResetToken, string UserId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ChangePasswordBindingModel model = new ChangePasswordBindingModel
            {
                ConfirmPassword = null,
                OldPassword = PasswordResetToken
            };
            return Ok(model);
        }

        [AllowAnonymous]
        [Route("PostSimplePasswordReset", Name = "PostSimplePasswordReset")]
        public async Task<IHttpActionResult> PostSimplePasswordReset(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            return Ok();
        }




        //[AllowAnonymous]
        //[HttpPost]
        //[Route("RegisterClient")]
        //public async Task<IHttpActionResult> RegisterClient(ClientRegisterBindingModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (CheckEmailExists(model.Email))
        //        {
        //            return BadRequest("Email already exists");
        //        }
        //        string username = GenerateClientUsername();
        //        string password = GenerateClientPassword();
        //        ApplicationUser user = new ApplicationUser()
        //        {
        //            UserName = username,
        //            Email = model.Email,
        //            PhoneNumber = model.PhoneNumber
        //        };
        //        IdentityResult result = await UserManager.CreateAsync(user, password);
        //        if (!result.Succeeded)
        //        {
        //            return GetErrorResult(result);
        //        }
        //        if (user == null)
        //        {
        //            return BadRequest();
        //        }
        //        string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
        //        var callbackUrl = Url.Link("DefaultApi", new
        //        {
        //            Controller = "Account/ConfirmEmail/",
        //            ConfirmEmailToken = code,
        //            UserId = user.Id
        //        });
        //        //ADD
        //        //отсылка sms с кодом
        //        //ADD
        //        await UserManager.
        //            SendEmailAsync(user.Id, "Confirm e-mail", "Please confirm your e-mail by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
        //        return Ok();
        //    }
        //    return BadRequest();
        //}


        //[AllowAnonymous]
        //[HttpGet]
        //[Route("GetClientRegister", Name = "GetClientRegister")]
        //public async Task<IHttpActionResult> GetClientRegister(string ConfirmEmailToken, string UserId)
        //{
        //    if (UserId != null && UserId != "" && ConfirmEmailToken != null && ConfirmEmailToken != "")
        //    {

        //        string code = await UserManager.GeneratePasswordResetTokenAsync(UserId);
        //        ClientRegisterModel model = new ClientRegisterModel()
        //        {
        //            ConfirmEmailToken = ConfirmEmailToken,
        //            ResetPasswordToken = code,
        //            UserId = UserId
        //        };
        //        return Ok(model);
        //    }
        //    return BadRequest();
        //}

        //[AllowAnonymous]
        //[HttpPost]
        //[Route("PostClientRegister", Name = "PostClientRegister")]
        //public async Task<IHttpActionResult> PostClientRegister(ClientRegisterModel model)
        //{
        //    //тут проверка кода из смс
        //    //если все хорошо, отправляем на смену пароля
        //    if (model.UserId == null || model.ConfirmEmailToken == null || model.ResetPasswordToken == null)
        //    {
        //        return BadRequest("Empty parameters");
        //    }
        //    if (model.NewPassword == null)
        //    {
        //        return BadRequest("Empty password");
        //    }
        //    var result = await UserManager.ConfirmEmailAsync(model.UserId, model.ConfirmEmailToken);
        //    if (result.Succeeded)
        //    {
        //        result = await UserManager.ResetPasswordAsync(model.UserId, model.ResetPasswordToken, model.NewPassword);
        //        if (result.Succeeded)
        //        {
        //            return Ok();
        //        }
        //    }
        //    return BadRequest();
        //}



        //[AllowAnonymous]
        //[HttpGet]
        //[Route("ConfirmEmail", Name = "ConfirmEmail")]
        //public IHttpActionResult ConfirmEmail(string ConfirmEmailToken, string UserId)
        //{
        //    //окошко для ввода кода из смс
        //    if (UserId != null && UserId != "" && ConfirmEmailToken != null && ConfirmEmailToken != "")
        //    {
        //        return Ok();
        //    }
        //    return BadRequest();
        //}

        //[AllowAnonymous]
        //[HttpPost]
        //[Route("PostConfirmEmail", Name = "PostConfirmEmail")]
        //public async Task<IHttpActionResult> PostConfirmEmail(ConfirmEmailModel model)
        //{
        //    //тут проверка кода из смс
        //    //если все хорошо, отправляем на смену пароля
        //    if (model.UserId == null || model.ConfirmEmailToken == null)
        //    {
        //        return BadRequest();
        //    }
        //    var result = await UserManager.ConfirmEmailAsync(model.UserId, model.ConfirmEmailToken);
        //    if (result.Succeeded)
        //    {
        //        return Ok();
        //    }
        //    return BadRequest();
        //}


        /// <summary>
        /// GetExtenal login (not useful for GTI)
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="error"></param>
        /// <returns></returns>
            // GET api/Account/ExternalLogin
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

                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName); //, image.ImageData);
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
        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
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
        // POST api/Account/Register
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
        // POST api/Account/RegisterExternal
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
