using System;
using System.Collections.Generic;
using Xunit;
using YellowCanaryLibrary;
using YellowCanaryLibrary.Services.Implementations;

namespace YellowCanary.Tests
{
    public class SuperServiceTests
    {
        [Fact]
        public void CalculateEmployeesSuperSummary_Should_Calculate_Summary_For_Each_Employee()
        {
            // Arrange
            var superService = new SuperService();
            var employees = new List<Employee>
            {
                new Employee
                {
                    EmployeeCode = 1234,
                    Payslips = new List<Payslip>
                    {
                        new Payslip { Id = "1", End = new DateTime(2023, 1, 15), PayCodeId = "1", EmployeeCode = 1234, Amount = 100, IsOTE = true },
                        new Payslip { Id = "2", End = new DateTime(2023, 3, 30), PayCodeId = "2", EmployeeCode = 1234, Amount = 150, IsOTE = false },
                    },
                    Disbursements = new List<Disbursement>
                    {
                        new Disbursement { Amount = 50, PaymentMade = new DateTime(2023, 2, 10), PayPeriodFrom = new DateTime(2023, 1, 1), PayPeriodTo = new DateTime(2023, 1, 31), EmployeeCode = 1234 },
                    }
                },
            };

            // Act
            superService.CalculateEmployeesSuperSummary(employees);

            // Assert
            Assert.NotNull(employees); 
            Assert.Equal(1, employees?.Count);
            Assert.Equal(1, employees?[0].SuperSummaries?.Count);
            Assert.Equal(100, employees?[0].SuperSummaries?[0].OteTotal);
            Assert.Equal(9.5, employees?[0].SuperSummaries?[0].SuperPayableTotal);
            Assert.Equal(50, employees?[0].SuperSummaries?[0].DisbursementsTotal);

        }
    }
}
