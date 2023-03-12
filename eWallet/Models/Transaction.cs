using eWallet.Infrastructure;

namespace eWallet.Models;

public class Transaction:AggregateRoot
{
    public int Id { get; set; }
    public string WalletNo { get; set; }
    public TransactionType TransactionType { get; set; }
    public Money Amount { get; set; }        
    public DateTime TransactionDate { get; set; }                
    public string Status { get; set; }                
}


public enum TransactionType
{
    Debit,
    Recharg
}


public static class TransactionStatus
{
    public const string New = "NEW";
    public const string Completed = "COMPLETED";
    public const string Rejected = "REJECTED";
}