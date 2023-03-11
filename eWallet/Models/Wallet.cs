using eWallet.Infrastructure;
using NullGuard;

namespace eWallet.Models;

public class Wallet:BaseEntity
{
    public int Id { get; set; }
    public string WalletNo { get; set; }
    public string OwnerId { get; set; }
    [AllowNull]
    public virtual ApplicationUser? Owner { get; set; }
    public DateTime LastTransactionDate { get; set; }
    public Money Balance { get; set; }
}
