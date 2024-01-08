namespace YellowCanaryLibrary
{
    public class Disbursement
    {
        public double Amount { get; set; }
        public DateTime PaymentMade { get; set; }
        public DateTime PayPeriodFrom { get; set; }
        public DateTime PayPeriodTo { get; set; }
        public double EmployeeCode { get; set; }
    }
}