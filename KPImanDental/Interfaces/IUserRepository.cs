using KPImanDental.Dto;
using KPImanDental.Model;

namespace KPImanDental.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserListDto>> GetAllUsersAsync();
        Task<KpImanUser> GetUserByIdAsync(long id);
        Task<UserDto> GetUserDtoByIdAsync(long id);
    }
}
