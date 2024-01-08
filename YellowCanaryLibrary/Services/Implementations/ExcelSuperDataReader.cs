using ExcelDataReader;
using System.Data;
using System.Globalization;
using System.Text;
using YellowCanaryLibrary.Extensions;
using YellowCanaryLibrary.Services.Interfaces;

namespace YellowCanaryLibrary.Services.Implementations
{
    public class ExcelSuperDataReader : ISuperDataReader
    {
        private const string PayCodesTableName = "PayCodes";
        private const string PayslipsTableName = "Payslips";
        private const string DisbursementsTableName = "Disbursements";

        private const string PayCodeColumn = "pay_code";
        private const string OteTreatmentColumn = "ote_treament";
        private const string PayslipIdColumn = "payslip_id";
        private const string EndColumn = "end";
        private const string PayCodeIdColumn = "code";
        private const string EmployeeCodeColumn = "employee_code";
        private const string AmountColumn = "amount";
        private const string SgcAmountColumn = "sgc_amount";
        private const string PaymentMadeColumn = "payment_made";
        private const string PayPeriodFromColumn = "pay_period_from";
        private const string PayPeriodToColumn = "pay_period_to";

        public SuperData GetSuperData(string filePath)
        {
            
            
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The specified file does not exist.");
            }

            var superData = new SuperData();

            // this line is needed to make the excel reader work
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = true
                        }
                    });

                    superData.PayCodeData = HandlePayCodesTableData(result.Tables[PayCodesTableName]);
                    superData.PayslipData = HandlePayslipsTableData(result.Tables[PayslipsTableName]);
                    superData.DisbursementData = HandleDisbursementsTableData(result.Tables[DisbursementsTableName]);
                }
            }
            return superData;
        }

        private List<PayCodeData> HandlePayCodesTableData(DataTable dataTable)
        {
            List<PayCodeData> payCodes = dataTable.AsEnumerable().Select(row =>
                new PayCodeData
                {
                    Id = row.Field<string>(PayCodeColumn),
                    Treatment = row.Field<string>(OteTreatmentColumn).ToOteTreatment()
                }
            ).ToList();

            return payCodes;
        }

        private List<PayslipData> HandlePayslipsTableData(DataTable dataTable)
        {
            List<PayslipData> payslips = dataTable.AsEnumerable().Select(row =>
                new PayslipData
                {
                    Id = row.Field<string>(PayslipIdColumn),
                    End = row.Field<DateTime>(EndColumn),
                    PayCodeId = row.Field<string>(PayCodeIdColumn),
                    EmployeeCode = Convert.ToInt32(row.Field<double>(EmployeeCodeColumn)),
                    Amount = row.Field<double>(AmountColumn),
                }
            ).ToList();

            return payslips;
        }

        private List<DisbursementData> HandleDisbursementsTableData(DataTable dataTable)
        {
            List<DisbursementData> disbursements = dataTable.AsEnumerable().Select(row =>
                new DisbursementData
                {
                    Amount = row.Field<double>(SgcAmountColumn),
                    PaymentMade = ParseDateTime(row, PaymentMadeColumn),
                    PayPeriodFrom = ParseDateTime(row, PayPeriodFromColumn),
                    PayPeriodTo = ParseDateTime(row, PayPeriodToColumn),
                    EmployeeCode = Convert.ToInt32(row.Field<double>(EmployeeCodeColumn)),
                }
            ).ToList();

            return disbursements;
        }
        
        private DateTime ParseDateTime(DataRow row, string columnName)
        {
            string dateString = row[columnName].ToString();
            DateTime resultDateTime;
            if (DateTime.TryParseExact(dateString, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out resultDateTime))
            {
                return resultDateTime;
            }
            else
            {
                throw new FormatException($"Error parsing the date in column '{columnName}'");
            }
        }

    }
}
