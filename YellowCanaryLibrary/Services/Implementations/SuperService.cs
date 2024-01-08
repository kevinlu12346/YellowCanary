using System.Data;
using YellowCanaryLibrary.Models;
using YellowCanaryLibrary.Services.Interfaces;

namespace YellowCanaryLibrary.Services.Implementations
{
    public class SuperService : ISuperService
    {
        public const double SuperPercentage = 0.095;

        public List<Employee> ProcessSuperData(SuperData superData)
        {
            var employees = new List<Employee>();

            var employeesDictionary = new Dictionary<double, Employee>();

            var payCodeData = superData.PayCodeData;
            var payslipData = superData.PayslipData;
            var disbursementData = superData.DisbursementData;

            var payslips = payslipData
            .Join(payCodeData, payslip => payslip.PayCodeId, payCode => payCode.Id,
                (payslip, payCode) => new Payslip
                {
                    Id = payslip.Id,
                    End = payslip.End,
                    PayCodeId = payslip.PayCodeId,
                    EmployeeCode = payslip.EmployeeCode,
                    Amount = payslip.Amount,
                    IsOTE = payCode.Treatment == OteTreatment.Ote
                });

            var disbursements = disbursementData
                .Select(d => new Disbursement {
                    Amount = d.Amount,
                    PaymentMade = d.PaymentMade,
                    PayPeriodFrom = d.PayPeriodFrom,
                    PayPeriodTo = d.PayPeriodTo,
                    EmployeeCode = d.EmployeeCode
                });

            var payslipsGroupedByEmployee = payslips.GroupBy(p => p.EmployeeCode);
            foreach (var employeePayslipGroup in payslipsGroupedByEmployee)
            {
                var employeeCode = employeePayslipGroup.Key;
                if (!employeesDictionary.ContainsKey(employeeCode))
                {
                    var employee = new Employee();
                    employee.EmployeeCode = employeeCode;
                    employeesDictionary.Add(employeeCode, employee);
                }

                employeesDictionary[employeeCode].Payslips = employeePayslipGroup.ToList();
            }

            var disbursementsGroupedByEmployee = disbursements.GroupBy(p => p.EmployeeCode);
            foreach (var employeeDisbursementGroup in disbursementsGroupedByEmployee)
            {
                var employeeCode = employeeDisbursementGroup.Key;
                if (!employeesDictionary.ContainsKey(employeeCode))
                {
                    var employee = new Employee();
                    employee.EmployeeCode = employeeCode;
                    employeesDictionary.Add(employeeCode, employee);
                }

                employeesDictionary[employeeCode].Disbursements = employeeDisbursementGroup.ToList();
            }

            employees = employeesDictionary.Values.ToList();
            return employees;
        }

        public void CalculateEmployeesSuperSummary(List<Employee> employees)
        {
            var employeesSuperSummary = new List<EmployeeSuperSummary>();

            foreach (var employee in employees)
            {
                var employeesSuperSummaryDictionary = new Dictionary<(int, Quarter), EmployeeSuperSummary>();

                var payslipsGroupedByQuarterAndYear = employee.Payslips.GroupBy(p => new
                {
                    Quarter = ConvertDateTimeToQuarter(p.End),
                    Year = p.End.Year
                });

                var disbursementsGroupedByQuarterAndYear = employee.Disbursements.GroupBy(d => new
                {
                    Quarter = ConvertDisbursementPaymentDateToQuarter(d.PaymentMade),
                    Year = d.PaymentMade.Year
                });

                foreach (var QuarterAndYearPayslips in payslipsGroupedByQuarterAndYear)
                {
                    var year = QuarterAndYearPayslips.Key.Year;
                    var quarter = QuarterAndYearPayslips.Key.Quarter;

                    var employeeSuperSummary = new EmployeeSuperSummary();
                    employeeSuperSummary.Year = year;
                    employeeSuperSummary.Quarter = quarter;
                    employeeSuperSummary.OteTotal = QuarterAndYearPayslips.Where(p => p.IsOTE).Sum(p => p.Amount);
                    employeeSuperSummary.SuperPayableTotal = employeeSuperSummary.OteTotal * SuperPercentage;
                    employeesSuperSummaryDictionary.Add((year, quarter), employeeSuperSummary);
                }

                foreach (var QuarterAndYearDisbursements in disbursementsGroupedByQuarterAndYear)
                {
                    var year = QuarterAndYearDisbursements.Key.Year;
                    var quarter = QuarterAndYearDisbursements.Key.Quarter;

                    var employeeSuperSummaryKey = (year, quarter);
                    if (!employeesSuperSummaryDictionary.ContainsKey(employeeSuperSummaryKey))
                    {
                        var employeeSuperSummary = new EmployeeSuperSummary();
                        employeeSuperSummary.Year = year;
                        employeeSuperSummary.Quarter = quarter;

                        employeesSuperSummaryDictionary.Add((year, quarter), employeeSuperSummary);
                    }

                    employeesSuperSummaryDictionary[employeeSuperSummaryKey].DisbursementsTotal = QuarterAndYearDisbursements.Sum(d => d.Amount);
                }

                employee.SuperSummaries.AddRange(employeesSuperSummaryDictionary.Values.ToList());
            }
        }

        private Quarter ConvertDisbursementPaymentDateToQuarter(DateTime paymentMade)
        {
            int month = paymentMade.Month;
            int day = paymentMade.Day;

            if ((month == 1 && day >= 29) || month == 2 || month == 3 || (month == 4 && day <= 28))
            {
                return Quarter.first;
            }
            else if ((month == 4 && day >= 29) || month == 5 || month == 6 || (month == 7 && day <= 28))
            {
                return Quarter.second;
            }
            else if ((month == 7 && day >= 29) || month == 8 || month == 9 || (month == 10 && day <= 28))
            {
                return Quarter.third;
            }
            else // October 29th - December 28th and also includes December 29th
            {
                return Quarter.fourth;
            }
        }

        private Quarter ConvertDateTimeToQuarter(DateTime date)
        {
            var month = date.Month;
            switch (month) 
            {
                case var m when m >= 1 && m <= 3:
                    return Quarter.first;
                case var m when m >= 4 && m <= 6:
                    return Quarter.second;
                case var m when m >= 7 && m <= 9:
                    return Quarter.third;
                case var m when m >= 10 && m <= 12:
                    return Quarter.fourth;
                default:
                    throw new ArgumentOutOfRangeException(nameof(month), "Invalid month value");
            }
        }

        public void SortEmployeesSuperSummary(List<Employee> employees)
        {
            employees.ForEach(employee =>
            {
                employee.SuperSummaries = employee.SuperSummaries
                    .OrderBy(s => s.Year)
                    .ThenBy(s => s.Quarter)
                    .ToList();
            });
        }
    }
}
