using eWallet.Infrastructure;
using eWallet.Models;

namespace eWallet.Rules
{
    public interface IWalletChargeRule
    {
        bool IsChargeable(ApplicationUser user, Money walletBalance, Money rechargeMoney);
    }
}
