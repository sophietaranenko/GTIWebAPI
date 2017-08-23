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

using GTIWebAPI.Models.Account;
using GTIWebAPI.Models.Security;
using GTIWebAPI.Models.Organizations;
using GTIWebAPI.Filters;
using GTIWebAPI.NovelleDirectory;
using GTIWebAPI.Exceptions;
using System.IO;
using System.Linq;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Repository;
using System.Data.SqlClient;
using System.Data;
using GTIWebAPI.Models.Service;
using GTIWebAPI.Models.Dictionary;

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

        private IApplicationDbContext context;

        private INovelleDirectory novell;  

        private IDbContextFactory factory;

        private IIdentityHelper identityHelper;

        private IRequest request; 

        public AccountController()
        {
            novell = new NovelleDirectory.NovelleDirectory();
            factory = new DbContextFactory();
            context = new ApplicationDbContext();
            identityHelper = new IdentityHelper();
            request = new Request();
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            this.UserManager = userManager;
            this.AccessTokenFormat = accessTokenFormat;

            novell = new NovelleDirectory.NovelleDirectory();
            factory = new DbContextFactory();
            context = new ApplicationDbContext();
            identityHelper = new IdentityHelper();
            request = new Request();
        }


        public AccountController(IDbContextFactory factory, IApplicationDbContext context, IRequest request, IIdentityHelper identityHelper, INovelleDirectory novell, ApplicationUserManager userManager)
        {
            this.novell = novell;
            this.factory = factory;
            this.UserManager = userManager;
            this.context = context;
            this.identityHelper = identityHelper;
            this.request = request;
        }

        public AccountController(IDbContextFactory factory, IApplicationDbContext context, IIdentityHelper identityHelper)
        {
            this.factory = factory;
            this.identityHelper = identityHelper;
            this.context = context;

            request = new Request();
            novell = new NovelleDirectory.NovelleDirectory();
            identityHelper = new IdentityHelper();
        }

        public AccountController(IDbContextFactory factory, IIdentityHelper identityHelper, IRequest request)
        {
            this.factory = factory;
            this.identityHelper = identityHelper;
            this.request = request;

            context = new ApplicationDbContext();
            novell = new NovelleDirectory.NovelleDirectory();
            identityHelper = new IdentityHelper();
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
        public UserInfoViewModel GetUserInfo()
        {
            string UserId = identityHelper.GetUserId(User);

             UnitOfWork unitOfWork = new UnitOfWork(factory);

            SqlParameter parUser = new SqlParameter
            {
                ParameterName = "@UserId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.String,
                Value = UserId
            };


            UserInfoViewModel userInfoViewModel = unitOfWork.SQLQuery<UserInfoViewModel>("exec UserInfo @UserId", parUser).FirstOrDefault();

            //Task<List<UserRightDTO>> tRights = GetUserRights(UserId);
            //Task<UserInfoViewModel> tModel = GetUserInfoViewModel(UserId);


            //await Task.WhenAll(tModel, tRights);

            //UserInfoViewModel userInfoViewModel = tModel.Result;
            //userInfoViewModel.UserRights = tRights.Result;
            //return userInfoViewModel;


            //i don't know why but they need another parameter 

            SqlParameter parUser1 = new SqlParameter
            {
                ParameterName = "@UserId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.String,
                Value = UserId
            };

            IEnumerable<UserRightOfficeDTO> rightsString = unitOfWork.SQLQuery<UserRightOfficeDTO>("exec UserRights @UserId", parUser1);
            List<UserRightDTO> dtos = new List<UserRightDTO>();
            try
            {
                if (rightsString != null)
                {
                    if (rightsString.Count() != 0)
                    {
                        var result = rightsString.Select(r => new OfficeSecurity { Id = r.OfficeId.GetValueOrDefault(), ShortName = r.OfficeShortName }).Distinct().ToList();
                        if (result != null)
                        {
                            foreach (var item in result)
                            {
                                UserRightDTO dto = new UserRightDTO();
                                dto.OfficeId = item.Id;
                                dto.OfficeName = item.ShortName;

                                List<ControllerBoxDTO> boxesList = new List<ControllerBoxDTO>();
                                var bList = rightsString
                                    .Where(d => d.OfficeId == item.Id)
                                    .Select(d => new ControllerBoxDTO { Id = d.BoxId.GetValueOrDefault(), Name = d.BoxName })
                                    .Distinct()
                                    .ToList();

                                if (bList != null)
                                {
                                    foreach (var box in bList)
                                    {

                                        ControllerBoxDTO boxDTO = new ControllerBoxDTO();
                                        boxDTO.Name = box.Name;
                                        boxDTO.Id = box.Id;

                                        List<ControllerDTO> controllerList = new List<ControllerDTO>();
                                        var cList = rightsString.Where(r => r.OfficeId == item.Id && r.BoxId == box.Id)
                                            .Select(r => new ControllerDTO { Id = r.ControllerId.GetValueOrDefault(), ControllerName = r.ControllerName })
                                            .Distinct().ToList();

                                        if (cList != null)
                                        {
                                            foreach (var c in cList)
                                            {
                                                ControllerDTO cDto = new ControllerDTO();
                                                cDto.ControllerName = c.ControllerName;
                                                cDto.Id = c.Id;

                                                List<ActionDTO> actionList = new List<ActionDTO>();
                                                var aList = rightsString.Where(r => r.OfficeId == item.Id && r.ControllerId == c.Id)
                                                    .Select(r => new ActionDTO { Id = r.ActionId.GetValueOrDefault(), ActionName = r.ActionName, ActionLongName = r.ActionLongName })
                                                    .Distinct().ToList();
                                                if (aList != null)
                                                {
                                                    foreach (var a in aList)
                                                    {
                                                        ActionDTO aDto = new ActionDTO();
                                                        aDto.Id = a.Id;
                                                        aDto.ActionLongName = a.ActionLongName == null ? "" : a.ActionLongName;
                                                        aDto.ActionName = a.ActionName == null ? "" : a.ActionName;
                                                        actionList.Add(aDto);
                                                    }
                                                }
                                                cDto.Actions = actionList;
                                                controllerList.Add(cDto);
                                            }
                                        }

                                        boxDTO.Controllers = controllerList;
                                        boxesList.Add(boxDTO);
                                    }
                                }
                                dto.Boxes = boxesList;
                                dtos.Add(dto);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string m = e.Message;
            }

            userInfoViewModel.UserRights = dtos.OrderBy(d => d.OfficeName);

            return userInfoViewModel;



        }

        private async Task<UserInfoViewModel> GetUserInfoViewModel(string userId)
        {
            UnitOfWork unitOfWork = new UnitOfWork(factory);
            SqlParameter parUser = new SqlParameter
            {
                ParameterName = "@UserId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.String,
                Value = userId
            };

            return unitOfWork.SQLQuery<UserInfoViewModel>("exec UserInfo @UserId", parUser).FirstOrDefault();
        }

        private async Task<List<UserRightDTO>> GetUserRights(string userId)
        {
            UnitOfWork unitOfWork = new UnitOfWork(factory);
            SqlParameter parUser1 = new SqlParameter
            {
                ParameterName = "@UserId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.String,
                Value = userId
            };
            IEnumerable<UserRightOfficeDTO> rightsString = unitOfWork.SQLQuery<UserRightOfficeDTO>("exec UserRights @UserId", parUser1);
            List<UserRightDTO> dtos = new List<UserRightDTO>();
            try
            {
                if (rightsString != null)
                {
                    if (rightsString.Count() != 0)
                    {
                        var result = rightsString.Select(r => new OfficeSecurity { Id = r.OfficeId.GetValueOrDefault(), ShortName = r.OfficeShortName }).Distinct().ToList();
                        if (result != null)
                        {
                            foreach (var item in result)
                            {
                                UserRightDTO dto = new UserRightDTO();
                                dto.OfficeId = item.Id;
                                dto.OfficeName = item.ShortName;

                                List<ControllerBoxDTO> boxesList = new List<ControllerBoxDTO>();
                                var bList = rightsString
                                    .Where(d => d.OfficeId == item.Id)
                                    .Select(d => new ControllerBoxDTO { Id = d.BoxId.GetValueOrDefault(), Name = d.BoxName })
                                    .Distinct()
                                    .ToList();

                                if (bList != null)
                                {
                                    foreach (var box in bList)
                                    {

                                        ControllerBoxDTO boxDTO = new ControllerBoxDTO();
                                        boxDTO.Name = box.Name;
                                        boxDTO.Id = box.Id;

                                        List<ControllerDTO> controllerList = new List<ControllerDTO>();
                                        var cList = rightsString.Where(r => r.OfficeId == item.Id && r.BoxId == box.Id)
                                            .Select(r => new ControllerDTO { Id = r.ControllerId.GetValueOrDefault(), ControllerName = r.ControllerName })
                                            .Distinct().ToList();

                                        if (cList != null)
                                        {
                                            foreach (var c in cList)
                                            {
                                                ControllerDTO cDto = new ControllerDTO();
                                                cDto.ControllerName = c.ControllerName;
                                                cDto.Id = c.Id;

                                                List<ActionDTO> actionList = new List<ActionDTO>();
                                                var aList = rightsString.Where(r => r.OfficeId == item.Id && r.ControllerId == c.Id)
                                                    .Select(r => new ActionDTO { Id = r.ActionId.GetValueOrDefault(), ActionName = r.ActionName, ActionLongName = r.ActionLongName })
                                                    .Distinct().ToList();
                                                if (aList != null)
                                                {
                                                    foreach (var a in aList)
                                                    {
                                                        ActionDTO aDto = new ActionDTO();
                                                        aDto.Id = a.Id;
                                                        aDto.ActionLongName = a.ActionLongName == null ? "" : a.ActionLongName;
                                                        aDto.ActionName = a.ActionName == null ? "" : a.ActionName;
                                                        actionList.Add(aDto);
                                                    }
                                                }
                                                cDto.Actions = actionList;
                                                controllerList.Add(cDto);
                                            }
                                        }

                                        boxDTO.Controllers = controllerList;
                                        boxesList.Add(boxDTO);
                                    }
                                }
                                dto.Boxes = boxesList;
                                dtos.Add(dto);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string m = e.Message;
            }
            return dtos;
        }




        [GTIFilter]
        [HttpPost]
        [Route("UploadProfilePicture")]
        public IHttpActionResult UploadNewProfilePicture()
        {
            try
            {
                UserImage image = new UserImage();
                if (request.FileCount() > 0)
                {
                    string userId = identityHelper.GetUserId(User);

                    UnitOfWork unitOfWork = new UnitOfWork(factory);
                    foreach (string file in request.Collection())
                    {
                        string filePath = request.SaveFile(file);
                        image.ImageName = filePath;
                        image.IsProfilePicture = true;
                        image.UploadDate = DateTime.Now;
                        image.UserId = User.Identity.GetUserId();

                        IEnumerable<UserImage> images = unitOfWork.UserImagesRepository.Get(d => d.UserId == image.UserId);
                        foreach (var item in images)
                        {
                            item.IsProfilePicture = false;
                            unitOfWork.UserImagesRepository.Update(item);
                        }
                        image.Id = Guid.NewGuid();
                        unitOfWork.UserImagesRepository.Insert(image);
                        unitOfWork.Save();
                    }
                    return Ok(image);
                }
                else
                {
                    return NotFound();
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
        }


        [GTIFilter]
        [HttpPost]
        [Route("SetAProfilePicture")]
        public IHttpActionResult SetAsProfilePicture(Guid pictureId)
        {
            UnitOfWork unitOfWork = new UnitOfWork(factory);
            UserImage image = unitOfWork.UserImagesRepository
                .Get(d => d.Id == pictureId).FirstOrDefault();
            IEnumerable<UserImage> images = unitOfWork.UserImagesRepository
            .Get(d => d.UserId == image.UserId);
            foreach (var item in images)
            {
                item.IsProfilePicture = false;
                unitOfWork.UserImagesRepository.Update(item);
            }
            image.IsProfilePicture = true;
            unitOfWork.UserImagesRepository.Update(image);
            unitOfWork.Save();
            return Ok(image);
        }

        [Route("UserRights")]
        public IEnumerable<UserRightDTO> GetUserRights()
        {
            ApplicationUser user = identityHelper.FindUserById(identityHelper.GetUserId(User));
            IEnumerable<UserRightDTO> rights = user.GetUserRightsDTO();
            return rights;
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
                // ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
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

                UnitOfWork unitOfWork = new UnitOfWork(factory);
                person = unitOfWork.OrganizationContactPersonsViewRepository.Get(d => d.Id == organizationContactPersonId)
                    .FirstOrDefault();

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
                   // novellPerson.Login = novell.GenerateLogin(novellPerson.Login);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }

                INovellOrganizationContactPerson personCreated = novell.CreateOrganization(novellPerson);
                if (personCreated != null)
                {
                    bool dbResult = context.CreateOrganization(novellPerson.Login, novellPerson.Password);
                    if (dbResult)
                    {
                        ApplicationUser user = new ApplicationUser()
                        {
                            UserName = novellPerson.Login,
                            Email = novellPerson.Email,
                            //TableName = "OrganizationContactPerson",
                            //TableId = organizationContactPersonId
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
                                IdentityResult roleResult = await UserManager.AddToRoleAsync(user.Id, "Organization");
                                if (!roleResult.Succeeded)
                                {
                                    return GetErrorResult(roleResult);
                                }

                                bool rightsResult = context.GrantRightsToOrganization(user.Id);
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
            string letterText = File.ReadAllText(request.AppPath() + "/HtmlMailFile/letter.html");
            letterText = letterText.Replace("NEW_USER_LOGIN", novellPerson.Login);
            letterText = letterText.Replace("NEW_USER_PASSWORD", novellPerson.Password);
            await UserManager.SendEmailAsync(userId, "Регистрация в кабинете клиента GTI", letterText);
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
            context.Dispose();
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
