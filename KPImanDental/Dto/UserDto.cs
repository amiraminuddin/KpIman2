namespace KPImanDental.Dto
{
    public class UserDto
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsSupervisor { get; set; } = false;
        public string Gender { get; set; }
        public string UserPhoto { get; set; }
        public string FormattedBirthDate
        {
            get
            {
                return  Helpers.DateConversion.ConvertDateTime(BirthDate, "yyyy-MM-dd");
            }
        }
    }
}
