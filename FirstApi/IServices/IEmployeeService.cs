using FirstApi.Dto;

namespace FirstApi.IServices
{
    public interface IEmployeeService
    {
        public Task<Tuple<int, List<EmployeeDto>>> GetAllEmployees();
        public Task<Tuple<int, EmployeeDto>> GetAllEmployeeById(Guid id);
        public Task<Tuple<int, string>> CreateEmployee(EmployeeDto dto);
        public Task<Tuple<int, string>> UpdateEmployee(EmployeeDto dto);
        public Task<Tuple<int, string>> DeleteEmployee(Guid id);
    }
}
