using eWallet.Infrastructure;
using eWallet.Models;
using System.IO;

namespace eWallet.Rules;

public interface IWalletCharge
{
    bool IsChargeable(ApplicationUser user, Money walletBalance, Money rechargeMoney);
}

public class WalletCharge : IWalletCharge
{
    List<IWalletChargeRule> _rules = new List<IWalletChargeRule>();  

    public WalletCharge()
    {
        this._rules.Add(new WalletMaxBalanceRule(ApplicationUserStatus.IDENTIFIED, new Money(100000.00M, "TJS")));
        this._rules.Add(new WalletMaxBalanceRule(ApplicationUserStatus.UNIDENTIFIED, new Money(10000.00M, "TJS")));
    }

    public bool IsChargeable(ApplicationUser user, Money walletBalance, Money rechargeMoney)
    {
        bool isChargeable = _rules.All(rule => rule.IsChargeable(user, walletBalance, rechargeMoney));
        return isChargeable;
    }
}
