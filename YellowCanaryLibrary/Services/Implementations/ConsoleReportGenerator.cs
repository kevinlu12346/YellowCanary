using System.Data;
using YellowCanaryLibrary.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text;

namespace YellowCanaryLibrary.Services.Implementations
{
    public class ConsoleReportGenerator : IReportGenerator
    {

        private readonly ILogger<ConsoleReportGenerator> _logger;

        public ConsoleReportGenerator(ILogger<ConsoleReportGenerator> logger)
        {
            _logger = logger;
        }

        public void GenerateReport(List<Employee> employees)
        {
            if (employees == null)
            {
                _logger.LogError("Employee list is null. Unable to generate the report.");
                return;
            }

            _logger.LogInformation("Generating report...\n");

            foreach (var employee in employees)
            {
                string report = GenerateEmployeeReport(employee);
                _logger.LogInformation(report);
            }
            _logger.LogInformation("Generating report finished...\n");
        }

        private string GenerateEmployeeReport(Employee employee)
        {
            StringBuilder reportBuilder = new StringBuilder();

            reportBuilder.AppendLine($"Employee Code: {employee.EmployeeCode}\n");

            var superSummaries = employee.SuperSummaries;

            foreach (var superSummary in superSummaries)
            {
                reportBuilder.AppendLine($"\t{superSummary.Year} -- {superSummary.Quarter} quarter");
                reportBuilder.AppendLine($"\t\tOTE total: {superSummary.OteTotal}");
                reportBuilder.AppendLine($"\t\tSuper payable: {superSummary.SuperPayableTotal}");
                reportBuilder.AppendLine($"\t\tDisbursement total: {superSummary.DisbursementsTotal}\n");
            }

            return reportBuilder.ToString();
        }
    }
}
