namespace YellowCanaryLibrary.Services.Interfaces
{
    public interface ISuperService
    {
        public void CalculateEmployeesSuperSummary(List<Employee> employees);
        public List<Employee> ProcessSuperData(SuperData superData);
        public void SortEmployeesSuperSummary(List<Employee> employees);
    }
}
