using KPImanDental.Dto;
using KPImanDental.Dto.UserDto;
using KPImanDental.Model;
using KPImanDental.Model.Validator;
using Microsoft.AspNetCore.Mvc;

namespace KPImanDental.Interfaces.Services
{
    public interface IUserService
    {
        #region User Interface
        Task<long> CreateOrUpdateUser(UserCreateDto UserCreateDtoInput);

        Task<UserCreateDto> GetUserForEdit(long Id);
        Task<IEnumerable<UserDtoExt>> GetUsers();
        Task<UserDtoExt> GetUserFromId(long Id);
        #endregion

        #region Department Interface
        Task<long> CreateOrUpdateDepartment(DepartmentDto departmentDto);
        Task<IEnumerable<DepartmentDto>> GetDepartments();
        Task<DepartmentDto> GetDepartmentById(long Id);
        Task<string> DeleteDepartment(long Id);
        Task<List<ActionValidatorsOutput>> GetDepartmentActionValidator(ActionValidatorsInput<DepartmentDto> request);
        Task<List<Validators>> GetDepartmentValidator(DataValidators<DepartmentDto> request);
        #endregion
    }
}
