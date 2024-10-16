using KPImanDental.Dto;
using KPImanDental.Dto.LookupDto;
using KPImanDental.Dto.UserDto;
using KPImanDental.Model;

namespace KPImanDental.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDtoExt>> GetAllUsersAsync();
        Task<IEnumerable<KpImanUser>> GetAllKPImanUsersAsync();
        Task<KpImanUser> GetUserByIdAsync(long id);
        Task<UserDto> GetUserDtoByIdAsync(long id);
        Task<StaffLookupDto> GetUserLookupDtoByIdAsync(long id);
        Task<Department> GetDepartmentByIdAsync(long id);
        Task<IEnumerable<DepartmentDto>> GetAllDepartmentAsync();

        Task<Department> GetDepartmentByCodeAsync(string code);

        Task<Position> GetPositionByCodeAsync(string code);
    }
}
