using Microsoft.Extensions.Logging;
using YellowCanaryLibrary.Services.Interfaces;

public class SuperApplication
{
    private readonly ISuperDataReader _excelSuperDataReaderService;
    private readonly ISuperService _superService;
    private readonly IReportGenerator _consoleReportGeneratorService;
    private readonly ILogger<SuperApplication> _logger;


    public SuperApplication(
        ISuperDataReader excelSuperDataReaderService,
        ISuperService superService,
        IReportGenerator consoleReportGeneratorService,
        ILogger<SuperApplication> logger)
    {
        _excelSuperDataReaderService = excelSuperDataReaderService;
        _superService = superService;
        _consoleReportGeneratorService = consoleReportGeneratorService;
        _logger = logger;
    }

    public void Run()
    {
        try
        {
            var dataFilePath = GetDataFilePath();
            var superData = _excelSuperDataReaderService.GetSuperData(dataFilePath);
            var employees = _superService.ProcessSuperData(superData);
            _superService.CalculateEmployeesSuperSummary(employees);
            _superService.SortEmployeesSuperSummary(employees);
            _consoleReportGeneratorService.GenerateReport(employees);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while running the SuperApplication: {ErrorMessage}", ex.Message);
        }
    }

    private string GetDataFilePath()
    {
        var projectDirectory = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.Parent?.FullName;
        return $"{projectDirectory ?? "./"}/Sample Super Data.xlsx";
    }
}