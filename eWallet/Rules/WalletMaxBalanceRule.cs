using eWallet.Infrastructure;
using eWallet.Models;

namespace eWallet.Rules
{
    public class WalletMaxBalanceRule : IWalletChargeRule
    {
        ApplicationUserStatus _status;
        Money _maxBanalce;

        public WalletMaxBalanceRule(ApplicationUserStatus status, Money maxBanalce) 
        {
            this._status = status;
            this._maxBanalce = maxBanalce;
        }

        public bool IsChargeable(ApplicationUser user, Money walletBalance, Money rechargeMoney)
        {

            if (user.Status != _status ) return true;
            return _maxBanalce.GreaterThanOrEqual(walletBalance.Add(rechargeMoney));
            
        }
    }
}
