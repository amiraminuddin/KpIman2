using System.ComponentModel.DataAnnotations;

namespace KPImanDental.Model
{
    public class KpImanUser
    {
        public long Id { get; set; }
        [Required]
        public string UserName { get; set; }
        public string FullName { get; set; }
        [Required]
        public string Password { get; set; } //This for references.
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }    
        public string Email { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        [Required]
        public bool IsActive { get; set; } = true;
        [Required]
        public bool IsSupervisor { get; set; } = false;
        public long SupervisorId { get; set; } //Fo making organization chart
        public string UserPhoto { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }


        //Relation
        public ICollection<UserRoles> UserRoles { get; set; }
    }
}
