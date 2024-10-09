using KPImanDental.Dto.LookupDto;

namespace KPImanDental.Dto.UserDto
{
    public class UserDtoExt : UserDto //extend for display lookup
    {
        public int RowNumber { get; set; }
        public LookupTemplateDto DepartmentL { get; set; }
        public LookupTemplateDto PositionL { get; set; }
        public LookupTemplateDto SupervisorNameL { get; set; }
        public string ConvertDateTime
        {
            get
            {
                return BirthDate.ToString("dd/MM/yyyy");

            }
        }
    }
}
