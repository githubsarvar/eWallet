using eWallet.Infrastructure;
using eWallet.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace eWallet.Services
{
    public class UserProvider : IUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        private ApplicationUser _currentUser;
        public UserProvider(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._userManager = userManager;
        }


        public ApplicationUser CurrentUser => _currentUser = _currentUser ?? GetCurrentUser();     

        private ApplicationUser GetCurrentUser()
        {
            var claimsIdentity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
            Claim maybeClaim = claimsIdentity.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            return _userManager.FindByIdAsync(maybeClaim.Value).GetAwaiter().GetResult();

        }        
    }
}
