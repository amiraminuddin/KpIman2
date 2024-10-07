using KPImanDental.Dto;
using KPImanDental.Dto.LookupDto;
using KPImanDental.Model;

namespace KPImanDental.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserListDto>> GetAllUsersAsync();
        Task<KpImanUser> GetUserByIdAsync(long id);
        Task<UserDto> GetUserDtoByIdAsync(long id);
        Task<StaffLookupDto> GetUserLookupDtoByIdAsync(long id);
        Task<Department> GetDepartmentByIdAsync(long id);
        Task<IEnumerable<DepartmentDto>> GetAllDepartmentAsync();

        Task<Department> GetDepartmentByCodeAsync(string code);

        Task<Position> GetPositionByCodeAsync(string code);
    }
}
