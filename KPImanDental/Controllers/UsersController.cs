using AutoMapper;
using KPImanDental.Data;
using KPImanDental.Data.Repository;
using KPImanDental.Dto;
using KPImanDental.Interfaces;
using KPImanDental.Model;
using KPImanDental.Services;
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
        //<snippet_GetAll>
        [HttpGet("getAllUser")]
        public async Task<ActionResult<IEnumerable<UserListDto>>> GetUsers()
        {
            //var users = await _context.Users.ToListAsync();
            //var usersListDto = _mapper.Map<IEnumerable<UserListDto>>(users).Select((user, index) =>
            //{
            //    user.RowNumber = index + 1;
            //    return user;
            //}).ToList();
            var usersListDto = await _userRepo.GetAllUsersAsync();

            return Ok(usersListDto);
        }

        // <snippet_GetById>
        [HttpGet("getUserById")]
        public async Task<ActionResult<UserDto>> GetUserFromId(long Id)//[FromQuery] fo passing parameter
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
        [HttpGet("getAllDepartment")]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartments()
        {
            var Departments = await _context.Departments.Include(d => d.Position).ToListAsync();

            var DepartmentList = _mapper.Map<IEnumerable<DepartmentDto>>(Departments);

            return Ok(DepartmentList);
        }

        [HttpPost("registerDeparment")]
        public async Task<ActionResult<Department>> RegisterDepartment(DepartmentDto departmentDto)
        {
            if (await CheckDepartment(departmentDto.Code))
            { return BadRequest("Department Already Exists"); }

            var deparment = _mapper.Map<Department>(departmentDto);

            _context.Departments.Add(deparment);
            await _context.SaveChangesAsync();
            return deparment;
        }

        [HttpPut("updateDepartment")]
        public async Task<ActionResult<Department>> UpdateDepartment(DepartmentDto departmentDto)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(x => x.Id == departmentDto.Id);

            if(department == null) { return BadRequest("Department not Found!!"); }

            var updatedDepartment = _mapper.Map<Department>(departmentDto);

            await _context.SaveChangesAsync();

            return Ok(updatedDepartment);
        }

        #endregion CRUD For User Management Module - Department

        #region CRUD For User Management Module - Position

        [HttpGet("getPositionRegister")]
        public async Task<ActionResult<IEnumerable<PositionDto>>> GetAllPositions()
        {
            var positions = await _context.Posititon.ToListAsync();
            var positionList = _mapper.Map<IEnumerable<PositionDto>>(positions);

            return Ok(positionList);
        }
        [HttpGet("getPositionByDeprtmId")]
        public async Task<ActionResult<IEnumerable<PositionDto>>> GetPositionByDepartment(string departmentCode)
        {
            var positions = await _context.Posititon
                .Where(p => p.Department.Code == departmentCode)
                .Include(d => d.Department)
                .ToListAsync();

            if (positions == null) { return NotFound(); }

            var positionList = _mapper.Map<IEnumerable<PositionDto>>(positions);

            return Ok(positionList);
        }
        #endregion CRUD For User Management Module - Position


        #region Method
        private async Task<bool> CheckDepartment(string departmentCode)
        {
            return await _context.Departments.AnyAsync(x => x.Code.ToLower() == departmentCode.ToLower());
        }
        #endregion
    }
}
