using FirstApi.Data;
using FirstApi.Dto;
using FirstApi.Entities;
using FirstApi.IServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstApi.Services
{
    public class EmployeeService(ApplicationDbContext _context) : IEmployeeService
    {
        public async Task<Tuple<int, List<EmployeeDto>>> GetAllEmployees()
        {
            return new Tuple<int, List<EmployeeDto>>(1, await _context.Employees.AsNoTracking().Select(e => new EmployeeDto
            {
                Id = e.Id,
                Name = e.Name,
                CreatedDateTime = e.CreatedDateTime,
                LastModifiedDate = e.LastModifiedDate,
                DOB = e.DOB,
                Position = e.Position,
                Department = e.Department,
                EmailAddress = e.EmailAddress
            }).ToListAsync());
        }
        public async Task<Tuple<int, EmployeeDto>> GetAllEmployeeById(Guid id)
        {
            var employee = await _context.Employees.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (employee == null)
            {
                return new Tuple<int, EmployeeDto>(0, null);
            }
            var employeeDto = new EmployeeDto
            {
                Id = employee.Id,
                Name = employee.Name,
                CreatedDateTime = employee.CreatedDateTime,
                LastModifiedDate = employee.LastModifiedDate,
                DOB = employee.DOB,
                Position = employee.Position,
                Department = employee.Department,
                EmailAddress = employee.EmailAddress
            };
            return new Tuple<int, EmployeeDto>(1, employeeDto);
        }

        public async Task<Tuple<int, string>> CreateEmployee(EmployeeDto dto)
        {
            var existingEmployee = await _context.Employees.AsNoTracking().AnyAsync(e => e.EmailAddress == dto.EmailAddress);

            if (existingEmployee)
            {
                return new Tuple<int, string>(0, "Employee with this email Id is already registered, Please register with a different Email Id");
            }

            var Employee = new Employee
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                CreatedDateTime = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow,
                DOB = dto.DOB,
                Position = dto.Position,
                Department = dto.Department,
                EmailAddress = dto.EmailAddress
            };
            await _context.Employees.AddAsync(Employee);
            await _context.SaveChangesAsync();
            return new Tuple<int, string>(1, $"Employee {dto.Name} Registered Successfully.");
        }

        public async Task<Tuple<int, string>> UpdateEmployee(EmployeeDto dto)
        {
            var existingEmployee = await _context.Employees.FirstOrDefaultAsync(e => e.EmailAddress == dto.EmailAddress);

            if (existingEmployee == null)
            {
                return new Tuple<int, string>(0, "Employee with this email Id does not exist, Please update a valid user");
            }
            existingEmployee.Name = string.IsNullOrWhiteSpace(dto.Name) ? existingEmployee.Name : dto.Name;
            existingEmployee.LastModifiedDate = DateTime.UtcNow;
            existingEmployee.DOB = dto.DOB ?? existingEmployee.DOB;
            existingEmployee.Position = string.IsNullOrWhiteSpace(dto.Position) ? existingEmployee.Position : dto.Position;
            existingEmployee.Department = string.IsNullOrWhiteSpace(dto.Department) ? existingEmployee.Department : dto.Department;

            await _context.SaveChangesAsync();
            return new Tuple<int, string>(1, $"Employee {existingEmployee.Name} Updated Successfully.");
        }


        public async Task<Tuple<int, string>> DeleteEmployee(Guid id)
        {
            var existingEmployee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (existingEmployee == null)
            {
                return new Tuple<int, string>(0, "Employee Id does not exist.");
            }
            _context.Employees.Remove(existingEmployee);
            await _context.SaveChangesAsync();

            return new Tuple<int, string>(1, "Employee deleted Successfully!");
        }
    }
}
