using System.ComponentModel.DataAnnotations.Schema;

namespace eWallet.Infrastructure;

public class AggregateRoot : BaseEntity
{    

    [NotMapped]
    public List<IDomainEvent> DomainEvents { get; } = new List<IDomainEvent>();
    public void QueueDomainEvent(IDomainEvent @event) => DomainEvents.Add(@event);
}
