using eWallet.Infrastructure;

namespace eWallet.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string WalletNo { get; set; }
        public Money Amount { get; set; }        
        public DateTime Date { get; set; }                
    }
}
