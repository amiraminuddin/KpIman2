using System.ComponentModel.DataAnnotations.Schema;

namespace KPImanDental.Model
{
    [Table("KpImanPosition")]
    public class Position
    {
        public long Id { get; set; } // Primary Key
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

        //Navigation
        public long DepartmentId { get; set; }
        public Department Department { get; set; } = null!;
    }
}
