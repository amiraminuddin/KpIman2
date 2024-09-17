using System.ComponentModel.DataAnnotations;

namespace KPImanDental.Dto
{
    public class UserRegisterDto
    {
        [Required]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsSupervisor { get; set; } = false;
        public string CreatedBy { get; set; }
    }
}
