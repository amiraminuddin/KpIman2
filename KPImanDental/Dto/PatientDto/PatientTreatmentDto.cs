namespace KPImanDental.Dto.PatientDto
{
    public class PatientTreatmentDto
    {
        public long? Id { get; set; }
        public long PatientId { get; set; }        
        public long DrID { get; set; } //Doctor in charge        
        public long DSAId { get; set; } //DSA in charge        
        public string TreatmentNo { get; set; }
        public string Condition { get; set; }
        public string Description { get; set; }
        public long TreatmentType { get; set; } //Treatment Lookup
        public Double TreatmentCost { get; set; } //Get from Treatment Lookup by default
        public DateTime TreatmentDate { get; set; }
        public string PrescribedMedical { get; set; }
        public string TreatmentNotes { get; set; }
        public Boolean FollowUpReq { get; set; }
        public DateTime? FollowUpDate { get; set; }
    }
}
