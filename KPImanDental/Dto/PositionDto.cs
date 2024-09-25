using KPImanDental.Model;

namespace KPImanDental.Dto
{
    public class PositionDto
    {
        public long? Id { get; set; } // Primary Key
        public long DepartmentId {  get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


    }
}
