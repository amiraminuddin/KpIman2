using AutoMapper;
using KPImanDental.Dto;
using KPImanDental.Dto.LookupDto;
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

        public async Task<Department> GetDepartmentByIdAsync(long id)
        {
            return await _context.Departments.FindAsync(id);
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllDepartmentAsync()
        {
            var departmentList = await _context.Departments.ToListAsync();
            var departmentListDto = _mapper.Map<IEnumerable<DepartmentDto>>(departmentList);

            return departmentListDto;
        }

        public async Task<StaffLookupDto> GetUserLookupDtoByIdAsync(long id)
        {
            var user = await GetUserByIdAsync(id);
            var userLookupDto = _mapper.Map<StaffLookupDto>(user);

            return userLookupDto;
        }
    }
}
