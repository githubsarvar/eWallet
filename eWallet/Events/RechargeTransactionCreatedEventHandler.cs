using eWallet.Models;
using MediatR;

namespace eWallet.Events
{
    public class RechargeTransactionCreatedEventHandler : INotificationHandler<RechargeTransactionCreatedEvent>
    {

        private readonly WalletDbContext _dbContext;
        public RechargeTransactionCreatedEventHandler(WalletDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(RechargeTransactionCreatedEvent notification, CancellationToken cancellationToken)
        {
            
            var wallet = _dbContext.Wallets.Where(x=>x.WalletNo == notification._transaction.WalletNo).FirstOrDefault();
            wallet.Balance = wallet.Balance.Add(notification._transaction.Amount);
            wallet.LastTransactionDate = notification._transaction.TransactionDate;
            notification._transaction.Status = TransactionStatus.Completed;

            await _dbContext.SaveChangesAsync();            

        }
    }
}
