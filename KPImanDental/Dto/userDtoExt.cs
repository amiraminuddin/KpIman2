using KPImanDental.Dto.LookupDto;

namespace KPImanDental.Dto
{
    public class UserDtoExt : UserDto
    {
        public string ConfirmPassword {  get; set; }
        public LookupTemplateDto DepartmentL { get; set; }
        public LookupTemplateDto PositionL { get; set; }
        public LookupTemplateDto SupervisorNameL { get; set; }
        public string FormattedBirthDate
        {
            get
            {
                return Helpers.DateConversion.ConvertDateTime(BirthDate, "yyyy-MM-dd");
            }
        }
    }
}
