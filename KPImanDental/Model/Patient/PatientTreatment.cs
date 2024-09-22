namespace KPImanDental.Model.Patient
{
    public class PatientTreatment
    {
        public long Id { get; set; }
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
        public Boolean? FollowUpReq { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public string CreatedBy { get; set; } = "System"; //Default 
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

    }
}
