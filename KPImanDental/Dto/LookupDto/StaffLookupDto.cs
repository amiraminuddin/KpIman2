namespace KPImanDental.Dto.LookupDto
{
    public class StaffLookupDto
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public LookupTemplateDto DepartmentL { get; set; }
        public LookupTemplateDto PositionL { get; set; }
        public string FullName { get; set; }
    }
}
