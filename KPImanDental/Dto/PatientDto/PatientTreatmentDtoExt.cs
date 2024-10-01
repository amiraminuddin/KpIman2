using KPImanDental.Dto.LookupDto;

namespace KPImanDental.Dto.PatientDto
{
    public class PatientTreatmentDtoExt : PatientTreatmentDto
    {
        public string PatientName { get; set; }
        public LookupTemplateDto Doctor { get; set; }
        public LookupTemplateDto DSA { get; set; }
        public LookupTemplateDto Treatment { get; set; }
        public string TreatmentDateDisplay
        {
            get
            {
                return TreatmentDate.ToString("dd-MM-yyyy");
            }
        }
    }
}
