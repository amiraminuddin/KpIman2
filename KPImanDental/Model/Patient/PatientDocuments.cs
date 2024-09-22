namespace KPImanDental.Model.Patient
{
    public class PatientDocuments
    {
        public long Id { get; set; }
        public long PatientId { get; set; } //will act as treamentId
        public string DocumentNo { get; set; }
        public string DocumentType { get; set; }
        public string FileFormat { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; } = "System"; //Default 
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
