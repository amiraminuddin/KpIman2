namespace KPImanDental.Dto
{
    public class UserListDto
    {
        public long Id { get; set; }
        public int RowNumber { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsSupervisor { get; set; } = false;
        public string ConvertDateTime
        {
            get
            {
                return BirthDate.ToString("dd/MM/yyyy");
            }
        }
    }
}
