namespace KPImanDental.Model.Lookup
{
    public class TreatmentLookup
    {
        public long Id { get; set; }
        public string TreatmentCode { get; set; }
        public string TreatmentName { get; set; }
        public string TreatmentDesc { get; set; }
        public Boolean IsActive { get; set; }
        public Double TreatmentPrice { get; set; }
        public string CreatedBy { get; set; } = "System"; //Default 
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
