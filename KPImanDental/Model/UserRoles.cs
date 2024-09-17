using System.ComponentModel.DataAnnotations.Schema;

namespace KPImanDental.Model
{
    [Table("KpImanUserRoles")]
    public class UserRoles
    {
        public long Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;


        //Navigation Properties
        [Column("UserId")]
        public long KpImanUserId { get; set; }
        public KpImanUser KpImanUser { get; set; } = null!; //Relation on UserId
        [Column("RoleId")]
        public long RolesId { get; set; }
        public Roles Roles { get; set; } = null!; //Relation on RoleId
    }
}
