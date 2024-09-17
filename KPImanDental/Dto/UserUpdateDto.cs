using System.ComponentModel.DataAnnotations;

namespace KPImanDental.Dto
{
    public class UserUpdateDto
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsSupervisor { get; set; } = false;
        public string UserPhoto { get; set; }
    }
}
