using eWallet.Models;

namespace eWallet.Infrastructure;
public interface IUserProvider
{
    ApplicationUser CurrentUser { get; }
}


