using KPImanDental.Model;

namespace KPImanDental.Dto
{
    public class DepartmentDto
    {
        public long Id { get; set; } // Primary Key
        public string Code { get; set; }
        public string Name { get; set; } // Department Name
        public string Description { get; set; } // Department Description (optional)
        public ICollection<PositionDto> Position { get; set; }
    }
}
