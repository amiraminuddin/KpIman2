using System.ComponentModel.DataAnnotations.Schema;

namespace KPImanDental.Model
{
    [Table("KpImanDepartment")]
    public class Department
    {
        public long Id { get; set; } // Primary Key
        public string Code { get; set; }
        public string Name { get; set; } // Department Name
        public string Description { get; set; } // Department Description (optional)
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }


        //Relation
        public ICollection<Position> Position { get; set; }
    }
}
