using KPImanDental.Model;
using KPImanDental.Model.Patient;

namespace KPImanDental.Dto.PatientDto
{
    public class PatientTreatmentDto
    {
        public long Id { get; set; }
        public long PatientId { get; set; }
        //public string PatientName { get; set; }
        public Patient PatientName { get; set; }
        public long DrID { get; set; } //Doctor in charge
        //public string DoctorName { get; set; }
        public KpImanUser DoctorName { get; set; }
        public long DSAId { get; set; } //DSA in charge
        //public string DSAName { get; set; }
        public KpImanUser DSAName { get; set; }
        public string TreatmentNo { get; set; }
        public string Condition { get; set; }
        public string Description { get; set; }
        public long TreatmentType { get; set; } //Treatment Lookup
        public string TreatmentName { get; set; }
        public Double TreatmentCost { get; set; } //Get from Treatment Lookup by default
        public DateTime TreatmentDate { get; set; }
        public string PrescribedMedical { get; set; }
        public string TreatmentNotes { get; set; }
        public Boolean FollowUpReq { get; set; }
        public DateTime FollowUpDate { get; set; }
        public string CreatedBy { get; set; } = "System"; //Default 
        public string UpdatedBy { get; set; }
    }
}
