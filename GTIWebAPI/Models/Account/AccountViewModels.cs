using GTIWebAPI.Models.Security;
using System.Collections.Generic;

namespace GTIWebAPI.Models.Account
{
    // Models returned by AccountController actions.

    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }
    }

    public class ManageInfoViewModel
    {
        public string LocalLoginProvider { get; set; }

        public string Email { get; set; }

        public IEnumerable<UserLoginInfoViewModel> Logins { get; set; }

        public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; }
    }

    public class UserInfoViewModel
    {
        public int TableId { get; set; }
        public string TableName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public bool HasRegistered { get; set; }

        public string LoginProvider { get; set; }

        public IEnumerable<UserRightDTO> UserRights { get; set; }

        public string ProfilePicturePath { get; set; }
    }

    public class UserLoginInfoViewModel
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        public string Email { get; set; }
    }
}
