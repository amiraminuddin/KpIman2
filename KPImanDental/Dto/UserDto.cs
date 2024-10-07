namespace KPImanDental.Dto
{
    public class UserDto //base dto
    {
        public long? Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsSupervisor { get; set; } = false;
        public long SupervisorId { get; set; }
        public string Gender { get; set; }
        public string UserPhoto { get; set; }
        
    }
}
