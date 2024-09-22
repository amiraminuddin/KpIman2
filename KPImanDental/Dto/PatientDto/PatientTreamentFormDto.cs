namespace KPImanDental.Dto.PatientDto
{
    public class PatientTreamentFormDto
    {
        public long Id { get; set; }
        //public long PatientId { get; set; }
        //public long DrID { get; set; } //Doctor in charge
        //public long DSAId { get; set; } //DSA in charge
        public PatientDto Patient { get; set; }
        public UserDto Dr { get; set; }
        public UserDto DSA { get; set; }
        public string TreatmentNo { get; set; }
        public string Condition { get; set; }
        public string Description { get; set; }
        public long TreatmentType { get; set; } //Treatment Lookup
        public Double TreatmentCost { get; set; } //Get from Treatment Lookup by default
        public DateTime TreatmentDate { get; set; }
        public string PrescribedMedical { get; set; }
        public string TreatmentNotes { get; set; }
        public Boolean? FollowUpReq { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public string CreatedBy { get; set; } = "System"; //Default 
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
    }
}
