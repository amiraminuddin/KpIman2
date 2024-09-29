using KPImanDental.Dto.LookupDto;

namespace KPImanDental.Dto.PatientDto
{
    public class PatientTreatmentDtoExt : PatientTreatmentDto
    {
        public StaffLookupDto Doctor { get; set; }
        public StaffLookupDto DSA { get; set; }
        public TreatmentLookupDto Treatment { get; set; }
        public string TreatmentDateDisplay
        {
            get
            {
                return TreatmentDate.ToString("dd-MM-yyyy");
            }
        }
    }
}
