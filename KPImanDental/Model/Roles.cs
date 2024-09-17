using System.ComponentModel.DataAnnotations.Schema;

namespace KPImanDental.Model
{
    [Table("KpImanRoles")]
    public class Roles
    {
        public long Id { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

        //Relation
        public ICollection<UserRoles> UserRoles { get; set; }
    }
}
