using eWallet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eWallet.Configurations;

public class WalletConfiguration : BaseEntityConfiguration<Wallet>
{
    public override void Configure(EntityTypeBuilder<Wallet> builder)
    {
        base.Configure(builder);
        builder.HasOne(p => p.Owner).WithMany(t => t.Wallets).HasForeignKey(p => p.OwnerId);
        builder.Property(p => p.WalletNo).IsRequired().HasMaxLength(11);
        builder.Property(p => p.LastTransactionDate);
        builder.OwnsOne(p => p.Balance);
        builder.OwnsOne(p => p.Balance, navigationBuilder =>
        {
            navigationBuilder.Property(x => x.Amount).HasColumnName("Amount"); 
            navigationBuilder.Property(x => x.Currency).HasColumnName("Currency"); 
        });
    }
}
