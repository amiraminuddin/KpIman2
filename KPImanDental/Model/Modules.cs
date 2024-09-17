using System.ComponentModel.DataAnnotations.Schema;

namespace KPImanDental.Model
{
    [Table("KpImanModule")]
    public class Modules //Will create seed data for initialize the module data
    {
        public long Id { get; set; }
        public string ModuleCode { get; set; }
        public string ModuleName { get; set; }
        public string ModuleDescription { get; set; }
        public bool IsActive { get; set; }
        public byte[] ModuleIcon { get; set; }
        public byte[] CompressedModuleIcon { get; set; }
        public string CreatedBy { get; set; } = "System"; //Default 
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
