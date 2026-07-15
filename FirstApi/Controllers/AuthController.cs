using FirstApi.Dto;
using FirstApi.GenericResponse;
using FirstApi.IServices;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FirstApi.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    [ApiController]
    public class AuthController(IAuthService _Authservice) : ControllerBase
    {
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]UserDto dto)
        {
            try
            {
                var result = await _Authservice.UserLogin(dto);
                if (result.Item1 == 0)
                {
                    return NotFound(ResponseResult<TokenDto>.Failure(result.Item2, result.Item2.Message));
                }
                else if (result.Item1 == 1)
                {
                    return BadRequest(ResponseResult<TokenDto>.Failure(result.Item2, result.Item2.Message));
                }

                return Ok(ResponseResult<TokenDto>.Success(result.Item2, result.Item2.Message));
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserDto dto)
        {
            try
            {
                var result = await _Authservice.UserRegister(dto);
                if (result.Item1 == 0)
                {
                    return BadRequest(new { status = false, message = result.Item2 });
                }
                return Ok(new { status = true, message = result.Item2 });
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
