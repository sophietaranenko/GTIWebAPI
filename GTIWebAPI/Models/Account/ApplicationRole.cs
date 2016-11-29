using Microsoft.AspNet.Identity.EntityFramework;

namespace GTIWebAPI.Models.Account
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() { }
        public string Description { get; set; }
    }
}
