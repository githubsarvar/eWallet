using eWallet.Models;

namespace eWallet.Infrastructure;

public class Money : ValueObject<Money>
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new ArgumentException("Cannot add money with different currencies");
        
        return new Money(Amount + other.Amount, Currency);
    }
    
    public bool GreaterThanOrEqual(Money other)
    {
        if (this.Currency != other.Currency)        
            throw new ArgumentException("Cannot compare Money objects with different currencies");
        

        return  this.Amount >= other.Amount;
    }

}
