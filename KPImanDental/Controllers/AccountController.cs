using KPImanDental.Model;
using Microsoft.AspNetCore.Mvc;
using KPImanDental.Authorization;
using KPImanDental.Data;
using KPImanDental.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using KPImanDental.Interfaces.Services;

namespace KPImanDental.Controllers
{
    public class AccountController : BaseAPIController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AccountController(
            DataContext context, 
            ITokenService tokenService,
            IUserService userService,
            IMapper mapper)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
            _userService = userService;
        }

        public AuthService AuthService = new AuthService();

        #region Request Method
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
        #endregion
    }
}
