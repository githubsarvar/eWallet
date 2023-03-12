namespace eWallet.DTOs;

public class WalletRechargeRequest
{
    public string WalletNo { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set;}    
}
