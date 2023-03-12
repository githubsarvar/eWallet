using eWallet.Infrastructure;
using eWallet.Models;

namespace eWallet.Events
{
    public class RechargeTransactionCreatedEvent : IDomainEvent
    {
        public Transaction _transaction  { get; }

        public RechargeTransactionCreatedEvent(Transaction transaction)
        {
            _transaction = transaction;
        }
    }
}
