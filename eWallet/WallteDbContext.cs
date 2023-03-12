using eWallet.Configurations;
using eWallet.Infrastructure;
using eWallet.Models;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eWallet;

public class WalletDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{

    private readonly IMediator _mediator;

    public WalletDbContext() : base()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        Database.Migrate();
    }
    public WalletDbContext(DbContextOptions<WalletDbContext> options, IMediator mediator) : base(options)
    {
        this._mediator = mediator;
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        Database.Migrate();
    }



    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Wallet> Wallets { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new WalletConfiguration());
        modelBuilder.ApplyConfiguration(new TransactionConfiguration());            
    }

    public async Task DispatchDomainEventsAsync()
    {
        var domainEventEntities = ChangeTracker.Entries<AggregateRoot>()
            .Select(po => po.Entity)
            .Where(po => po.DomainEvents.Any())
            .ToArray();

        foreach (var entity in domainEventEntities)
        {
            var events = entity.DomainEvents.ToArray();
            entity.DomainEvents.Clear();
            foreach (var entityDomainEvent in events)
                await _mediator.Publish(entityDomainEvent);
        }
    }
}
