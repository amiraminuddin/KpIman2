using System.ComponentModel.DataAnnotations;

namespace KPImanDental.Dto
{
    public class UserLoginDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
