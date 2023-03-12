namespace eWallet.DTOs
{
    public class RechargeStatisticsDTO
    {
        public string WalletNo { get; set; }
        public int TrasactionQty { get; set; }
        public string Currency { get; set;}
        public decimal TotatAmount { get; set;}
    }
}
