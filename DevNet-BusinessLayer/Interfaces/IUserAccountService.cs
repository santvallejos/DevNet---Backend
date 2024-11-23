using DevNet_WebAPI.Infrastructure.DTO;

namespace DevNet_BusinessLayer.Interfaces
{
    public interface IUserAccountService
    {
        Task<bool> RegisterUserAsync(RegisterUserDto registerDto);
    }
}
