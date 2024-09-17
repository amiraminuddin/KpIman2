using AutoMapper;
using KPImanDental.Dto;
using KPImanDental.Interfaces;
using KPImanDental.Model;
using Microsoft.EntityFrameworkCore;

namespace KPImanDental.Data.Repository
{
    public class UserRespository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRespository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<UserListDto>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            var usersListDto = _mapper.Map<IEnumerable<UserListDto>>(users).Select((user, index) =>
            {
                user.RowNumber = index + 1;
                return user;
            }).ToList();

            return usersListDto;
        }

        public async Task<KpImanUser> GetUserByIdAsync(long id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<UserDto> GetUserDtoByIdAsync(long id)
        {
            var user = await _context.Users.FindAsync(id);
            return _mapper.Map<UserDto>(user);
        }
    }
}
