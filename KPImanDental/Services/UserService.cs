using AutoMapper;
using KPImanDental.Authorization;
using KPImanDental.Data;
using KPImanDental.Dto;
using KPImanDental.Dto.ChartDto;
using KPImanDental.Dto.UserDto;
using KPImanDental.Interfaces.Repositories;
using KPImanDental.Interfaces.Services;
using KPImanDental.Model;
using KPImanDental.Model.Validator;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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

        public AuthService AuthService = new AuthService();

        #region User
        public async Task<IEnumerable<UserDtoExt>> GetUsers()
        {
            var usersListDto = await _userRepo.GetAllUsersAsync();
            foreach(var data in usersListDto)
            {
                data.SupervisorNameL = await _lookupRepo.GetKPImanUserLookup((long)data.SupervisorId);
                data.DepartmentL = await _lookupRepo.GetDepartmentLookup(data.Department);
                data.PositionL = await _lookupRepo.GetPositionLookup(data.Position);
            }
            
            return usersListDto;
        }

        public async Task<UserDtoExt> GetUserFromId(long Id)
        {
            var user = await _userRepo.GetUserByIdAsync(Id);
            var userDto =  _mapper.Map<UserDtoExt>(user);
            userDto.DepartmentL = await _lookupRepo.GetDepartmentLookup(userDto.Department);
            userDto.PositionL = await _lookupRepo.GetPositionLookup(userDto.Position);
            userDto.SupervisorNameL = await _lookupRepo.GetKPImanUserLookup((long)userDto.SupervisorId);

            return userDto;
        }
        public async Task<UserCreateDto> GetUserForEdit(long Id)
        {
            var user = await _userRepo.GetUserByIdAsync(Id);
            if( user == null)
            {
                return null;
            }
            var result = _mapper.Map<UserCreateDto>(user);
            result.DepartmentL = await _lookupRepo.GetDepartmentLookup(result.Department);
            result.PositionL = await _lookupRepo.GetPositionLookup(result.Position);
            result.SupervisorNameL = await _lookupRepo.GetKPImanUserLookup((long)result.SupervisorId);

            return result;
        }

        public async Task<long> CreateOrUpdateUser(UserCreateDto UserCreateDtoInput)
        {
            if (UserCreateDtoInput.Id.HasValue)
            {
                var updatedUser = await UpdateUser(UserCreateDtoInput);
                return updatedUser;
            }
            else
            {
                var createdUser = await CreateUser(UserCreateDtoInput);
                return createdUser;
            }
        }

        public async Task<List<Validators>> GetUserValidator(DataValidators<UserCreateDto> request)
        {
            var validators = new List<Validators>();
            var properties = typeof(UserCreateDto).GetProperties();
            var userDto = request.Data;
            var data = await _userRepo.GetAllKPImanUsersAsync();

            foreach(var property in properties)
            {
                var value = property.GetValue(userDto);
                string propertyName = property.Name;

                if (propertyName == "UserName" && (request.TriggerType == Enums.ValidatorTriggerType.OnLoad || request.TriggerType == Enums.ValidatorTriggerType.OnChange))
                {
                    if (value is null or (object)"")
                    {
                        validators.Add(new Validators()
                        {
                            Field = propertyName,
                            IsValid = false,
                            Message = "User name is Mandatory",
                            ValidatorsType = Enums.ValidatorsType.Mandatory
                        });
                    }else
                    {
                        bool isUserNameExist = data.Where(x => x.UserName.ToUpper() == userDto.UserName.ToUpper()).Any();
                        if (isUserNameExist && !userDto.Id.HasValue)
                        {
                            validators.Add(new Validators()
                            {
                                Field = propertyName,
                                IsValid = false,
                                Message = "User name already exists",
                                ValidatorsType = Enums.ValidatorsType.Error
                            });
                        }
                    }
                }

                if (propertyName == "Department" && (request.TriggerType == Enums.ValidatorTriggerType.OnLoad || request.TriggerType == Enums.ValidatorTriggerType.OnChange))
                {
                    if (value is null or (object)"")
                    {
                        validators.Add(new Validators()
                        {
                            Field = propertyName,
                            IsValid = false,
                            Message = "Department is Mandatory",
                            ValidatorsType = Enums.ValidatorsType.Mandatory
                        });
                    }
                }

                if (propertyName == "Position" && (request.TriggerType == Enums.ValidatorTriggerType.OnLoad || request.TriggerType == Enums.ValidatorTriggerType.OnChange))
                {
                    if (value is null or (object)"")
                    {
                        validators.Add(new Validators()
                        {
                            Field = propertyName,
                            IsValid = false,
                            Message = "Position is Mandatory",
                            ValidatorsType = Enums.ValidatorsType.Mandatory
                        });
                    }
                }

                if (propertyName == "HierarchyLevel" && request.TriggerType == Enums.ValidatorTriggerType.OnChange)
                {
                    bool isCEOExist = data.Where(x => x.HierarchyLevel == 1).Any();

                    if (isCEOExist && userDto.HierarchyLevel == 1)
                    {
                        validators.Add(new Validators()
                        {
                            Field = propertyName,
                            IsValid = false,
                            Message = "Top Hierarchy Level already exist",
                            ValidatorsType = Enums.ValidatorsType.Error
                        });
                    }
                }

                if (propertyName == "SupervisorId" && request.TriggerType == Enums.ValidatorTriggerType.OnChange)
                {
                    if (value is null or (object)"" && userDto.HierarchyLevel > 1)
                    {
                        validators.Add(new Validators()
                        {
                            Field = propertyName,
                            IsValid = false,
                            Message = "Reported to are required",
                            ValidatorsType = Enums.ValidatorsType.Mandatory
                        });
                    }
                }
            }

            return validators;
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

        #region User Organization Chart
        public async Task<List<OrganizationChartDto>> GetOrganizationChart()
        {
            var users = await _userRepo.GetAllUsersAsync();
            var orgNodes = new List<OrganizationChartDto>();

            var userList = users.Where(x => x.HierarchyLevel != 0).OrderBy(x => x.HierarchyLevel).ToList();
            foreach (var x in userList) 
            {
                x.PositionL = await _lookupRepo.GetPositionLookup(x.Position);
            }
            var user = userList.Find(x => x.HierarchyLevel == 1);

            var node = BuildOrgChartNode(user, userList);  // Build each node and its children
            orgNodes.Add(node);


            return orgNodes;

        }
        #endregion

        #region Private Method User
        private async Task<long> CreateUser(UserCreateDto input)
        {
            var user = _mapper.Map<KpImanUser>(input);
            var passwordCrypt = AuthService.GetPasswordHasher(input.Password);

            user.PasswordHash = passwordCrypt.PasswordHash;
            user.PasswordSalt = passwordCrypt.PasswordSalt;
            user.CreatedBy = "System";
            user.CreatedOn = DateTime.Now;
            user.UpdatedBy = "System";
            user.UpdatedOn = DateTime.Now;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }

        private async Task<long> UpdateUser(UserCreateDto input)
        {
            var user = await _userRepo.GetUserByIdAsync((long)input.Id);
            if (user == null) { return -1; }

            var passwordCrypt = AuthService.GetPasswordHasher(input.Password);

            user.PasswordHash = passwordCrypt.PasswordHash;
            user.PasswordSalt = passwordCrypt.PasswordSalt;
            user.UpdatedBy = "Don";
            user.UpdatedOn = DateTime.Now;

            _mapper.Map(input, user);
            await _context.SaveChangesAsync();

            return user.Id;

        }

        private static OrganizationChartDto BuildOrgChartNode(UserDtoExt userDto, List<UserDtoExt> users)
        {
            //TODO : spilt between insert department and user
            var node = new OrganizationChartDto
            {
                Label = userDto.PositionL.FieldDisplay,
                Type = "user",
                StyleClass = "",
                Expanded = true,
                Data = new OrganizationChartDataDto
                {
                    Name = userDto.FullName,
                    ProfilePicture = ""
                },
                Children = new List<OrganizationChartDto>()
            };

            var subordinates = users.Where(x => x.SupervisorId == userDto.Id).OrderBy(x => x.HierarchyLevel).ToList();

            foreach (var subordinate in subordinates)
            {
                var childNode = BuildOrgChartNode(subordinate, users);  // Recursive call to build children
                node.Children.Add(childNode);
            }

            return node;
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
