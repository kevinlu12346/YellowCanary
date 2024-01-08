namespace YellowCanaryLibrary
{
    public class SuperData
    {
        public List<PayCodeData> PayCodeData { get; set; } = new List<PayCodeData>();
        public List<DisbursementData> DisbursementData { get; set; } = new List<DisbursementData>();
        public List<PayslipData> PayslipData { get; set; } = new List<PayslipData>();
    }
}