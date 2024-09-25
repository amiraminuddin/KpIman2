using KPImanDental.Model;
using Microsoft.AspNetCore.Mvc;
using KPImanDental.Authorization;
using KPImanDental.Data;
using KPImanDental.Dto;
using Microsoft.EntityFrameworkCore;
using KPImanDental.Interfaces;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace KPImanDental.Controllers
{
    public class AccountController : BaseAPIController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(
            DataContext context, 
            ITokenService tokenService, 
            IMapper mapper)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public AuthService AuthService = new AuthService();

        #region Request Method
        [Authorize]
        [HttpPost("register")]
        public async Task<ActionResult<KpImanUser>> Register(UserRegisterDto userRegisterDto)
        {
            if (await CheckUserExists(userRegisterDto.UserName))
            {
                return BadRequest("Username is taken!!");
            }
            var user = _mapper.Map<KpImanUser>(userRegisterDto);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserTokenDto>> Login(UserLoginDto userLoginDto)
        {
            var User = await _context.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == userLoginDto.UserName.ToLower());
            if (User == null) return Unauthorized("Invalid Username");
            var PasswordCheck = AuthService.LoginAuthorization(User, userLoginDto.Password);

            if (PasswordCheck)
            {
                return new UserTokenDto
                {
                    UserName = userLoginDto.UserName,
                    UserToken = _tokenService.CreateToken(User),
                };
            }else
            {
                return Unauthorized("Invalid Password");
            }
        }

        [Authorize]
        [HttpDelete("deleteUser")]
        public async Task<ActionResult<bool>> Delete(long Id)
        {
            var User = await _context.Users.FirstOrDefaultAsync(x => x.Id == Id);
            _context.Remove(User);
            await _context.SaveChangesAsync();

            return true;
        }

        [Authorize]
        [HttpPut("updateUser")]
        public async Task<ActionResult<KpImanUser>> UpdateUser(UserUpdateDto userUpdateDto)
        {
            var User = await _context.Users.FirstOrDefaultAsync(x => x.Id == userUpdateDto.Id);

            if (User == null) return NotFound("User not Found!!");

            var passwordHash = AuthService.GetPasswordHasher(userUpdateDto.Password);

            User.Password = userUpdateDto.Password;
            User.PasswordHash = passwordHash.PasswordHash;
            User.PasswordSalt = passwordHash.PasswordSalt;
            User.Email = userUpdateDto.Email;
            User.Address = userUpdateDto.Address;
            User.BirthDate = userUpdateDto.BirthDate;
            User.Position = userUpdateDto.Position;
            User.Department = userUpdateDto.Department;
            User.IsActive = userUpdateDto.IsActive;
            User.IsSupervisor = userUpdateDto.IsSupervisor;
            User.UpdatedBy = "Admin";
            User.UpdatedOn = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(User);
        }

        //TODO: Edit User and Delete User
        #endregion

        #region Method
        private async Task<bool> CheckUserExists (string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
        }
        #endregion
    }
}
