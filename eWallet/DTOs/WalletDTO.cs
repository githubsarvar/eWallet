
namespace eWallet.DTOs;

public class WalletDTO
{        
    public string WalletNo { get; set; }        
    public DateTime LastTransactionDate { get; set; }
    public decimal Balance { get; set; }
    public string Currency { get; set; }

}
