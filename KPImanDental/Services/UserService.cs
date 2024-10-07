using AutoMapper;
using KPImanDental.Data;
using KPImanDental.Dto;
using KPImanDental.Interfaces.Repositories;
using KPImanDental.Interfaces.Services;
using KPImanDental.Model;
using KPImanDental.Model.Validator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KPImanDental.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepo;
        private readonly ILookupRepository _lookupRepo;

        public UserService(DataContext context, IMapper mapper, IUserRepository userRepo, ILookupRepository lookupRepo)
        {
            _context = context;
            _mapper = mapper;
            _userRepo = userRepo;
            _lookupRepo = lookupRepo;
        }

        #region User
        public async Task<IEnumerable<UserListDto>> GetUsers()
        {
            var usersListDto = await _userRepo.GetAllUsersAsync();
            return usersListDto;
        }

        public async Task<UserDtoExt> GetUserFromId(long Id)
        {
            var user = await _userRepo.GetUserByIdAsync(Id);
            var userDto =  _mapper.Map<UserDtoExt>(user);
            userDto.DepartmentL = await _lookupRepo.GetDepartmentLookup(userDto.Department);
            userDto.PositionL = await _lookupRepo.GetPositionLookup(userDto.Position);
            userDto.SupervisorNameL = await _lookupRepo.GetKPImanUserLookup(userDto.SupervisorId);

            return userDto;
        }

        public async Task<long> CreateOrUpdateUser(UserDto userDto)
        {
            if (userDto.Id.HasValue) 
            {
                var updatedUser = await UpdateUser(userDto);
                return updatedUser;
            }
            else
            {
                var createdUser = await CreateUser(userDto);
                return createdUser;
            }
        }
        #endregion

        #region Department
        public async Task<long> CreateOrUpdateDepartment(DepartmentDto departmentDto)
        {
            if (departmentDto.Id.HasValue)
            {
                var updateDepartment = await UpdateDepartment(departmentDto);
                return updateDepartment;
            }
            else
            {
                var createDepartment = await CreateDepartment(departmentDto);
                return createDepartment;
            }
        }

        public async Task<IEnumerable<DepartmentDto>> GetDepartments()
        {
            var Departments = await _userRepo.GetAllDepartmentAsync();
            return Departments;
        }

        public async Task<DepartmentDto> GetDepartmentById(long Id)
        {
            var department = await _userRepo.GetDepartmentByIdAsync(Id);
            if (department == null) { return null; }

            var departmentDto = _mapper.Map<DepartmentDto>(department);

            return departmentDto;
        }

        public async Task<string> DeleteDepartment(long Id)
        {
            var department = await _userRepo.GetDepartmentByIdAsync(Id);
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return "Data Deleted";
        }

        public async Task<List<ActionValidatorsOutput>> GetDepartmentActionValidator(ActionValidatorsInput<DepartmentDto> request)
        {
            var actionValidatorsOutput = new List<ActionValidatorsOutput>();
            var properties = typeof(DepartmentDto).GetProperties();
            var departmentDto = request.Data;

            var department = await _userRepo.GetDepartmentByIdAsync((long)departmentDto.Id);
            var position = _context.Posititon.Where(x => x.DepartmentId == departmentDto.Id);
            var postionCount = await position.CountAsync();
            var userCount = await _context.Users.CountAsync(x => x.Department == departmentDto.Code && x.IsActive == true);

            foreach (var action in request.ActionCode)
            {
                if (action == "CREATE")
                {
                    //evaluate create
                }

                if (action == "EDIT")
                {
                    //evaluate edit
                }

                if (action == "DELETE")
                {
                    //evaluate delete
                    if (userCount > 0)
                    {
                        actionValidatorsOutput.Add(new ActionValidatorsOutput
                        {
                            ActionCode = action,
                            IsDisabled = false,
                            IsLocked = true,
                            IsVisible = true,
                            LockedMessage = "Cannot Delete Record due to there are active user under department"
                        });
                    }
                }
            }

            return actionValidatorsOutput;
        }

        public async Task<List<Validators>> GetDepartmentValidator(DataValidators<DepartmentDto> request)
        {
            var validators = new List<Validators>();
            var properties = typeof(DepartmentDto).GetProperties();
            var departmentDto = request.Data;
            var data = await _userRepo.GetAllDepartmentAsync();

            foreach (var property in properties)
            {
                var value = property.GetValue(departmentDto);
                string propertyName = property.Name;

                if (propertyName == "Code" && (request.TriggerType == Enums.ValidatorTriggerType.OnLoad || request.TriggerType == Enums.ValidatorTriggerType.OnChange))
                {

                    if (value is null or (object)"")
                    {
                        validators.Add(new Validators
                        {
                            Field = "Code",
                            IsValid = false,
                            Message = "Department is Mandatory.",
                            ValidatorsType = Enums.ValidatorsType.Mandatory,
                        });
                    }
                    else
                    {
                        bool isCodeUnique = data.Where(x => x.Code.ToUpper() == departmentDto.Code.ToUpper()).Any();
                        if (isCodeUnique && !departmentDto.Id.HasValue)
                        {
                            validators.Add(new Validators
                            {
                                Field = "Code",
                                IsValid = false,
                                Message = "Department code already exists.",
                                ValidatorsType = Enums.ValidatorsType.Error,
                            });
                        }
                    }
                }

                if (propertyName == "Name" && (value is not null) && request.TriggerType == Enums.ValidatorTriggerType.OnChange)
                {
                    bool isNameUnique = data.Where(x => x.Name.ToUpper() == departmentDto.Name.ToUpper()).Any();
                    if (isNameUnique)
                    {
                        validators.Add(new Validators
                        {
                            Field = "Name",
                            IsValid = true,
                            Message = "Department Name already exists.",
                            ValidatorsType = Enums.ValidatorsType.Warning,
                        });
                    }
                }
            }
            return validators;
        }
        #endregion

        #region Private Method User
        private async Task<long> CreateUser(UserDto input)
        {
            var user = _mapper.Map<KpImanUser>(input);

            user.CreatedBy = "System";
            user.CreatedOn = DateTime.Now;
            user.UpdatedBy = "System";
            user.UpdatedOn = DateTime.Now;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }

        private async Task<long> UpdateUser(UserDto input)
        {
            var user = await _userRepo.GetUserByIdAsync((long)input.Id);
            if (user == null) { return -1; }

            user.UpdatedBy = "Don";
            user.UpdatedOn = DateTime.Now;

            _mapper.Map(input, user);
            await _context.SaveChangesAsync();

            return user.Id;

        }

        private async Task<bool> CheckUserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
        }
        #endregion

        #region Private Method Department
        private async Task<long> CreateDepartment(DepartmentDto departmentDto)
        {
            var department = _mapper.Map<Department>(departmentDto);

            department.CreatedBy = "System";
            department.CreatedOn = DateTime.Now;
            department.UpdatedBy = "System";
            department.UpdatedOn = DateTime.Now;

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return department.Id;
        }

        private async Task<long> UpdateDepartment(DepartmentDto departmentDto)
        {
            var department = await _userRepo.GetDepartmentByIdAsync((long)departmentDto.Id);
            if (department == null) return -1;

            department.UpdatedBy = "Don";
            department.UpdatedOn = DateTime.Now;

            _mapper.Map(departmentDto, department);
            await _context.SaveChangesAsync();

            return department.Id;
        }
        #endregion
    }
}
