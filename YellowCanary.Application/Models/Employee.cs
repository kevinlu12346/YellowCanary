using YellowCanaryLibrary.Models;

namespace YellowCanaryLibrary
{
    public class Employee
    {
        public double EmployeeCode { get; set; }
        public List<Payslip>? Payslips { get; set;}
        public List<Disbursement>? Disbursements { get; set; }

        public List<EmployeeSuperSummary>? SuperSummaries { get; set; } = new List<EmployeeSuperSummary>();
    }
}
