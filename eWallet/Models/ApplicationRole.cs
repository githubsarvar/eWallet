using Microsoft.AspNetCore.Identity;
using NullGuard;

namespace eWallet.Models
{
    public class ApplicationRole:IdentityRole
    {
        public ApplicationRole() { }
        public ApplicationRole(string roleName, string roleTitle)
        {
            this.Name = roleName;
            this.Title = roleTitle;
        }
        [AllowNull]
        public string Title { get; set; }
    }
}
