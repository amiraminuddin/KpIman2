using AutoMapper;
using KPImanDental.Data;
using KPImanDental.Dto;
using KPImanDental.Dto.UserDto;
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

        private readonly IUserService _userService;

        public UsersController(
            DataContext context, 
            IMapper mapper, 
            IUserRepository userRepo, 
            IPhotoService photoService,
            IUserService userService
            ) 
        {
            _context = context;
            _mapper = mapper;
            _userRepo = userRepo;
            _photoService = photoService;
            _userService = userService;
        }

        #region CRUD For User Management Module - User
        [HttpPost("CreateOrUpdateUser")]
        public async Task<ActionResult<long>> CreateOrUpdateUser(UserCreateDto input)
        {
            var result = await _userService.CreateOrUpdateUser(input);
            return result;
        }

        [HttpGet("GetUserForEdit")]
        public async Task<ActionResult<UserCreateDto>> GetUserForEdit(long id)
        {
            var result = await _userService.GetUserForEdit(id);
            if (result == null) { return BadRequest("No Record Found"); }
            return Ok(result);
        }

        [HttpGet("getAllUser")]
        public async Task<ActionResult<IEnumerable<UserDtoExt>>> GetUsers()
        {
            var usersListDto = await _userService.GetUsers();

            return Ok(usersListDto);
        }

        [HttpGet("getUserById")]
        public async Task<ActionResult<UserDtoExt>> GetUserFromId(long Id)
        {
            try
            {
                var user = await _userService.GetUserFromId(Id);
                if (user != null)
                {
                    return user;
                }
                return NotFound("User Not Found!!");

            }catch (Exception)
            {
                return StatusCode(500, "Internal Error");
            }
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
            var user = await _userRepo.GetUserDtoByIdAsync((long)userInput.Id);

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

        [HttpDelete("deleteUser")]
        public async Task<ActionResult<bool>> Delete(long Id)
        {
            var User = await _context.Users.FirstOrDefaultAsync(x => x.Id == Id);
            _context.Remove(User);
            await _context.SaveChangesAsync();

            return true;
        }

        [HttpPost("GetUserValidator")]
        public async Task<ActionResult<List<Validators>>> GetUserValidator(DataValidators<UserCreateDto> request)
        {
            var result = await _userService.GetUserValidator(request);
            return Ok(result);
        }

        #endregion CRUD For User Management Module - User

        #region CRUD For User Management Module - Department
        [HttpPost("CreateOrUpdateDepartment")]
        public async Task<ActionResult> CreateOrUpdateDepartment(DepartmentDto departmentDto)
        {
            var result = await _userService.CreateOrUpdateDepartment(departmentDto);
            return Ok(result);
        }

        [HttpGet("GetAllDepartment")]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartments()
        {
            var result = await _userService.GetDepartments();

            return Ok(result);
        }

        [HttpGet("GetDepartmentById")]
        public async Task<ActionResult<DepartmentDto>> GetDepartmentById(long Id)
        {
            var result = await _userService.GetDepartmentById(Id);
            if (result == null) { return BadRequest("No Record Found"); }
            return Ok(result);
        }

        [HttpDelete("DeleteDepartment")]
        public async Task<ActionResult<string>> DeleteDepartment(long Id)
        {
            var result = await _userService.DeleteDepartment(Id);
            return Ok(result);
        }

        [HttpPost("GetDepartmentActionValidator")]
        public async Task<ActionResult<List<ActionValidatorsOutput>>> GetDepartmentActionValidator(ActionValidatorsInput<DepartmentDto> request)
        {
            var result = await _userService.GetDepartmentActionValidator(request);
            return result;
        }

        [HttpPost("GetDepartmentValidator")]
        public async Task<ActionResult<List<Validators>>> GetDepartmentValidator(DataValidators<DepartmentDto> request)
        {
            var result = await _userService.GetDepartmentValidator(request);
            return Ok(result);
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
        private async Task<string> GetDepartmentName(long Id)
        {
            var department = await _userRepo.GetDepartmentByIdAsync(Id);
            return department.Name;
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
