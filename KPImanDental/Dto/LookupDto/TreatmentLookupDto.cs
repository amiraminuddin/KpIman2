namespace KPImanDental.Dto.LookupDto
{
    public class TreatmentLookupDto
    {
        public long? Id { get; set; }
        public string TreatmentCode { get; set; }
        public string TreatmentName { get; set; }
        public string TreatmentDesc { get; set; }
        public bool IsActive { get; set; }
        public double TreatmentPrice { get; set; }

    }
}
