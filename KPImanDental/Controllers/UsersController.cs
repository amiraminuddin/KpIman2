using AutoMapper;
using KPImanDental.Data;
using KPImanDental.Dto;
using KPImanDental.Interfaces.Repositories;
using KPImanDental.Interfaces.Services;
using KPImanDental.Model;
using KPImanDental.Model.Validator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KPImanDental.Controllers
{
    [Authorize]
    public class UsersController: BaseAPIController
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepo;
        private readonly IPhotoService _photoService;

        public UsersController(DataContext context, IMapper mapper, IUserRepository userRepo, IPhotoService photoService) 
        {
            _context = context;
            _mapper = mapper;
            _userRepo = userRepo;
            _photoService = photoService;
        }

        #region CRUD For User Management Module - User
        [HttpGet("getAllUser")]
        public async Task<ActionResult<IEnumerable<UserListDto>>> GetUsers()
        {
            var usersListDto = await _userRepo.GetAllUsersAsync();

            return Ok(usersListDto);
        }

        [HttpGet("getUserById")]
        public async Task<ActionResult<UserDto>> GetUserFromId(long Id)
        {
            try
            {
                var user = await _userRepo.GetUserByIdAsync(Id);
                if (user != null)
                {
                    return _mapper.Map<UserDto>(user);
                }
                return NotFound("User Not Found!!");

            }catch (Exception)
            {
                return StatusCode(500, "Internal Error");
            }
        }

        [HttpPut("updateUser")]
        public async Task<ActionResult<KpImanUser>> UpdateUser(UserDto user)
        {
            var User = await _context.Users.FirstOrDefaultAsync(x => x.Id == user.Id);

            if (User == null) return NotFound("User not Found!!");

            _mapper.Map(user, User);

            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<KpImanUser>(user));
        }

        [HttpPost("saveUserPhoto")]
        public async Task<ActionResult<string>> SaveUserPhoto(IFormFile file)
        {

            var result = await _photoService.AddPhotoAsync(file);

            return result.SecureUrl.AbsoluteUri.ToString();
        }

        [HttpPost("checkUserChange")]
        public async Task<ActionResult<bool>> CheckUserChange(UserDto userInput)
        {
            var user = await _userRepo.GetUserDtoByIdAsync(userInput.Id);

            if (user == null) return NotFound("User Not Found!!");

            var properties = typeof(UserDto).GetProperties();
            foreach(var property in properties)
            {
                var previousValue = property.GetValue(user);
                var currentValue = property.GetValue(userInput);

                if (property.PropertyType == typeof(DateTime))
                {

                    if (!Equals(((DateTime)previousValue).Date, ((DateTime)currentValue).Date))
                    {
                        return Ok(false);
                    }
                }
                else
                {
                    if (!Equals(previousValue, currentValue))
                    {
                        return Ok(false);
                    }
                }                
            }            
            return Ok(true);
        }

        #endregion CRUD For User Management Module - User

        #region CRUD For User Management Module - Department
        [HttpPost("CreateOrUpdateDepartment")]
        public async Task<ActionResult> CreateOrUpdateDepartment(DepartmentDto departmentDto)
        {
            if (departmentDto.Id.HasValue)
            {
                var updateDepartment = await UpdateDepartment(departmentDto);
                return Ok(updateDepartment);
            }
            else
            {
                if (await CheckDepartment(departmentDto.Code) == true)
                {
                    return BadRequest("Department Code already exist");
                }
                var createDepartment = await CreateDepartment(departmentDto);
                return Ok(createDepartment);
            }
        }

        [HttpGet("GetAllDepartment")]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartments()
        {
            var Departments = await _userRepo.GetAllDepartmentAsync();

            return Ok(Departments);
        }

        [HttpGet("GetDepartmentById")]
        public async Task<ActionResult<DepartmentDto>> GetDepartmentById(long Id)
        {
            var department = await _userRepo.GetDepartmentByIdAsync(Id);
            if (department == null) { return BadRequest("No Record Found"); }

            var departmentDto = _mapper.Map<DepartmentDto>(department);
            
            return Ok(departmentDto);
        }

        [HttpDelete("DeleteDepartment")]
        public async Task<ActionResult<string>> DeleteDepartment(long Id)
        {
            var department = await _userRepo.GetDepartmentByIdAsync(Id);
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return Ok("Data Deleted");
        }

        [HttpGet("CanDeleteDepartment")] //change to action formula validation
        public async Task<ActionResult<DeletionCondition<DepartmentDto>>> CanDeleteDepartment(long DepartmentId)
        {
            var department = await _userRepo.GetDepartmentByIdAsync(DepartmentId);

            var position = _context.Posititon.Where(x => x.DepartmentId == DepartmentId);
            var postionCount = await position.CountAsync();
            var userCount = await _context.Users.CountAsync(x => x.Department == department.Code && x.IsActive == true);


            var departmentDeletionCondition = new DeletionCondition<DepartmentDto>
            {
                Entity = _mapper.Map<DepartmentDto>(department)
            };

            if (userCount > 0)
            {
                departmentDeletionCondition.DependenciesCount = userCount;
                departmentDeletionCondition.MessageType = Enums.MessageType.Error;
                departmentDeletionCondition.Message = $"There are {userCount} active user with position under {department.Name} department";
            }
            else if (postionCount > 0) 
            {
                var positionList = await position.ToListAsync();
                var message = "";
                foreach(var x in positionList)
                {
                    message += $"{x.Name}\n";
                }
                departmentDeletionCondition.DependenciesCount = postionCount;
                departmentDeletionCondition.MessageType = Enums.MessageType.Warning;
                departmentDeletionCondition.Message = $"Below Position(s) will be deleted:\n {message}";
            }
            else
            {
                departmentDeletionCondition.DependenciesCount = 0;
                departmentDeletionCondition.MessageType = Enums.MessageType.Information;
                departmentDeletionCondition.Message = "";
            }

            return Ok(departmentDeletionCondition);
        }

        [HttpPost("GetDepartmentActionValidator")]
        public async Task<ActionResult<List<ActionValidatorsOutput>>> GetDepartmentActionValidator(ActionValidatorsInput<DepartmentDto> request)
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
                    if(userCount > 0)
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

            return Ok(actionValidatorsOutput);
        }

        [HttpPost("GetDepartmentValidator")]
        public async Task<ActionResult<List<Validators>>> GetDepartmentValidator(DataValidators<DepartmentDto> request)
        {
            var validators = new List<Validators>();
            var properties = typeof(DepartmentDto).GetProperties();
            var departmentDto = request.Data;
            var data = await _userRepo.GetAllDepartmentAsync();

            foreach (var property in properties) { 
                var value = property.GetValue(departmentDto);
                string propertyName = property.Name;

                if (propertyName == "Code" && (request.TriggerType == Enums.ValidatorTriggerType.OnLoad || request.TriggerType == Enums.ValidatorTriggerType.OnChange)) {
                    
                    if(value is null or (object)"")
                    {
                        validators.Add(new Validators
                        {
                            Field = "Code",
                            IsValid = false,
                            Message = "Department is Mandatory.",
                            ValidatorsType = Enums.ValidatorsType.Mandatory,
                        });
                    }else
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
                    if(isNameUnique)
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
            return Ok(validators);
        }

        #endregion CRUD For User Management Module - Department

        #region CRUD For User Management Module - Position

        [HttpPost("CreateOrUpdatePosition")]
        public async Task<ActionResult> CreateOrUpdatePosition(PositionDto positionDto)
        {
            if(positionDto.Id.HasValue)
            {
                var updatePosition = await UpdatePosition(positionDto);
                return Ok(updatePosition);
            }
            else
            {
                var createPosition = await CreatePosition(positionDto);
                return Ok(createPosition);
            }
        }

        [HttpGet("GetAllPosition")]
        public async Task<ActionResult<IEnumerable<PositionDtoExt>>> GetAllPositions()
        {
            var positions = await _context.Posititon.ToListAsync();
            var positionList = _mapper.Map<IEnumerable<PositionDtoExt>>(positions);
            foreach (var item in positionList)
            {
                item.DepartmentName = await GetDepartmentName(item.DepartmentId);
            }

            return Ok(positionList);
        }

        [HttpGet("GetPositionById")]
        public async Task<ActionResult<PositionDtoExt>> GetPositionById(long id)
        {
            var position = await _context.Posititon.FindAsync(id);
            var positiondto = _mapper.Map<PositionDtoExt>(position);

            return Ok(positiondto);
        }

        [HttpGet("GetPositionByDeprtmId")]
        public async Task<ActionResult<IEnumerable<PositionDtoExt>>> GetPositionByDepartment(long departmentId)
        {
            var positions = await _context.Posititon
                .Where(p => p.DepartmentId == departmentId)
                .ToListAsync();

            if (positions == null) { return NotFound(); }

            var positionList = _mapper.Map<IEnumerable<PositionDtoExt>>(positions);

            return Ok(positionList);
        }

        [HttpDelete("DeletePosition")]
        public async Task<ActionResult<string>> DeletePosition(long id)
        {
            var position = await _context.Posititon.FindAsync(id);
            _context.Posititon.Remove(position);
            await _context.SaveChangesAsync();
            return Ok("Data Deleted");
        }

        [HttpGet("CanDeletePosition")]
        public async Task<ActionResult<DeletionCondition<Position>>> CanDeletePosition(long id)
        {
            //Check If Position already assign at user
            var position = await _context.Posititon.FirstOrDefaultAsync(x => x.Id == id);
            var userCount = await _context.Users.CountAsync(x => x.Position == position.Code && x.IsActive == true);

            var userDeletionCondition = new DeletionCondition<Position>
            {
                Entity = _mapper.Map<Position>(position),
                DependenciesCount = userCount,
                Message = ""
            };

            if (!userDeletionCondition.DeleteCondition())
            {
                userDeletionCondition.MessageType = Enums.MessageType.Error;
                userDeletionCondition.Message = $"There are {userCount} active user with {position.Name} position";
            }

            return Ok(userDeletionCondition);
        }
        #endregion CRUD For User Management Module - Position


        #region Private Method

        #region Department
        private async Task<bool> CheckDepartment(string departmentCode)
        {
            return await _context.Departments.AnyAsync(x => x.Code.ToLower() == departmentCode.ToLower());
        }

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

        private async Task<string> GetDepartmentName(long Id)
        {
            var department = await _userRepo.GetDepartmentByIdAsync(Id);
            return department.Name;
        }

        private async Task<bool> IsDepartmentCodeUnique(string input)
        {
            var data = await _userRepo.GetAllDepartmentAsync();
            var codeExists = data.Where(x => x.Code.ToUpper() == input.ToUpper());
            if(codeExists.Any()) return true;
            return false;
        }
        #endregion

        #region Position
        private async Task<long> CreatePosition(PositionDto positionDto)
        {
            var position = _mapper.Map<Position>(positionDto);

            position.CreatedBy = "System";
            position.CreatedOn = DateTime.Now;
            position.UpdatedBy = "System";
            position.UpdatedOn = DateTime.Now;

            _context.Posititon.Add(position);
            await _context.SaveChangesAsync();
            return position.Id;
        }

        private async Task<long> UpdatePosition(PositionDto positionDto)
        {
            var position = await _context.Posititon.FindAsync(positionDto.Id);
            if (position == null) return -1;

            position.UpdatedBy = "Don";
            position.UpdatedOn = DateTime.Now;

            _mapper.Map(positionDto, position);
            await _context.SaveChangesAsync();

            return position.Id;
        }
        #endregion
        #endregion
    }
}
