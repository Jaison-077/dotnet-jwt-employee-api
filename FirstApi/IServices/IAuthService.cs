using FirstApi.Dto;

namespace FirstApi.IServices
{
    public interface IAuthService
    {
        public Task<Tuple<int, TokenDto>> UserLogin(UserDto dto);
        public Task<Tuple<int, string>> UserRegister(UserDto dto);
    }
}
