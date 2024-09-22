namespace KPImanDental.Dto.PatientDto
{
    public class PatientDto
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public Boolean IsActive { get; set; }
        public string CreatedBy { get; set; } = "System"; //Default 
        public string UpdatedBy { get; set; }
    }
}
