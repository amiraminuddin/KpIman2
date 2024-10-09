namespace KPImanDental.Dto.UserDto
{
    public class UserCreateDto: UserDtoExt //only for create user
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
