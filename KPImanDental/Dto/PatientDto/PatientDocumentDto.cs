namespace KPImanDental.Dto.PatientDto
{
    public class PatientDocumentDto
    {
        public long Id { get; set; }
        public long PatientId { get; set; } //will act as treamentId
        public string DocumentNo { get; set; }
        public string DocumentType { get; set; }
        public string FileFormat { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; } = "System"; //Default 
        public string UpdatedBy { get; set; }
    }
}
