using FirstApi.Dto;
using FirstApi.GenericResponse;
using FirstApi.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController(IEmployeeService _employeeService) : ControllerBase
    {
        [HttpGet("GetAllEmployee")]
        public async Task<IActionResult> GetEmployee()
        {
            try
            {
                var emplist = await _employeeService.GetAllEmployees();
                if (!emplist.Item2.Any())
                {
                    return Ok(ResponseResult<IEnumerable<EmployeeDto>>.Failure(emplist.Item2, "No Employee Found"));
                }
                return Ok(ResponseResult<IEnumerable<EmployeeDto>>.Success(emplist.Item2, $"Total Employee found {emplist.Item2.Count}"));
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet("GetAllEmployee/{id:guid}")]
        public async Task<IActionResult> GetEmployeeById(Guid id)
        {
            try
            {
                var emplist = await _employeeService.GetAllEmployeeById(id);
                if (emplist.Item2==null)
                {
                    return Ok(ResponseResult<IEnumerable<EmployeeDto>>.Failure(null, "No Employee Found"));
                }
                return Ok(ResponseResult<EmployeeDto>.Success(emplist.Item2, $""));
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("CreateEmployee")]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeDto dto)
        {
            try
            {
                var result = await _employeeService.CreateEmployee(dto);
                if (result.Item1 == 0)
                {
                    return BadRequest(ResponseResult<string>.Failure(null, result.Item2));
                }
                return Ok(ResponseResult<string>.Success(null, result.Item2));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("updateEmployee")]
        public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeDto dto)
        {
            try
            {
                var result = await _employeeService.UpdateEmployee(dto);
                if (result.Item1 == 0)
                {
                    return BadRequest(ResponseResult<string>.Failure(null, result.Item2));
                }
                return Ok(ResponseResult<string>.Success(null, result.Item2));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete("DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            try
            {
                var result = await _employeeService.DeleteEmployee(id);
                if (result.Item1 == 0)
                {
                    return BadRequest(ResponseResult<string>.Failure(null, result.Item2));
                }
                return Ok(ResponseResult<string>.Success(null, result.Item2));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
