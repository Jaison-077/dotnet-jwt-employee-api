using FirstApi.Data;
using FirstApi.Dto;
using FirstApi.Entities;
using FirstApi.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FirstApi.Services
{
    public class AuthService(ApplicationDbContext _context) : IAuthService
    {
        public async Task<Tuple<int, TokenDto>> UserLogin(UserDto dto)
        {
            TokenDto TokenDto = new TokenDto();
            try
            {
                if (dto == null)
                {
                    TokenDto.Token = string.Empty;
                    TokenDto.Message = "Please fill all the details";
                    return Tuple.Create(1, TokenDto);
                }

                var existingUser = await _context.AccountUsers.FirstOrDefaultAsync(x => x.Email == dto.Email);
                if (existingUser == null)
                {
                    TokenDto.Token = string.Empty;
                    TokenDto.Message = "This User Not Exist, Please Login";
                    return new Tuple<int, TokenDto>(0, TokenDto);
                }

                var passwordHasher = new PasswordHasher<string>();
                var verifyPassword = passwordHasher.VerifyHashedPassword(dto.Email, existingUser.Password, dto.Password);

                if (verifyPassword == PasswordVerificationResult.Success)
                {
                    UserDto user = new();
                    user.Name = existingUser.Name;
                    user.Id = existingUser.Id;
                    user.Email = existingUser.Email;

                    TokenDto.Token = getJwtToken(user);
                    TokenDto.Message = $"Welcome {existingUser.Name}";
                    return new Tuple<int, TokenDto>(2, TokenDto);
                }
                else if (verifyPassword == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    UserDto user = new();
                    user.Name = existingUser.Name;
                    user.Id = existingUser.Id;
                    user.Email = existingUser.Email;

                    var token = getJwtToken(user);
                    existingUser.Password = PasswordHashing(dto);
                    _context.AccountUsers.Update(existingUser);
                    await _context.SaveChangesAsync();

                    TokenDto.Token = token;
                    TokenDto.Message = $"Welcome {existingUser.Name}, Login successfull, New Hash Generated";
                    return new Tuple<int, TokenDto>(2, TokenDto);
                }

                TokenDto.Token = string.Empty;
                TokenDto.Message = "Invalid password credentials provided.";
                return new Tuple<int, TokenDto>(1, TokenDto);
            }
            catch (Exception)
            {
                TokenDto.Token = string.Empty;
                TokenDto.Message = "Something Went Wrong";
                return new Tuple<int, TokenDto>(3, TokenDto);
            }
        }

        public async Task<Tuple<int, string>> UserRegister(UserDto dto)
        {
            try
            {
                var existingUser = await _context.AccountUsers.AnyAsync(x => x.Email == dto.Email);
                if (existingUser)
                {
                    return new Tuple<int, string>(0, "This User Already Exist, Please Login");
                }

                var newUser = new User
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name,
                    Email = dto.Email,
                    Password = PasswordHashing(dto)
                };
                await _context.AccountUsers.AddAsync(newUser);
                await _context.SaveChangesAsync();

                return new Tuple<int, string>(1, $"{newUser.Name} added successfully");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string PasswordHashing(UserDto dto)
        {
            var passwordHasher = new PasswordHasher<string>();
            return passwordHasher.HashPassword(dto.Email, dto.Password);
        }

        private string getJwtToken(UserDto dto)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, dto.Name ?? string.Empty),
                new Claim(ClaimTypes.Email, dto.Email ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, dto.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("671bd5f1f7483f74b439838783b3f62333e5f7d6aeb3196b23f3ea737a98ce3a"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "FirstApi",
                audience: "FirstApiClient",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
