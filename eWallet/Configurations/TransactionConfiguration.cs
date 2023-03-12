using eWallet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eWallet.Configurations;

public class TransactionConfiguration : BaseEntityConfiguration<Transaction>
{
    public override void Configure(EntityTypeBuilder<Transaction> builder)
    {
        base.Configure(builder);            
        builder.Property(p => p.WalletNo).HasMaxLength(11).IsRequired();
        builder.OwnsOne(p => p.Amount, navigationBuilder =>
        {
            navigationBuilder.Property(x => x.Amount).HasColumnName("Amount"); 
            navigationBuilder.Property(x => x.Currency).HasColumnName("Currency");
        });        
        builder.Property(p => p.TransactionDate).HasDefaultValueSql("CURRENT_TIMESTAMP").IsRequired();
        builder.Property(p => p.TransactionType).IsRequired();
        builder.Property(p => p.Status).HasDefaultValue(TransactionStatus.New).IsRequired();
        
    }
}
