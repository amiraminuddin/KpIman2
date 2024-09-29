using KPImanDental.Dto.LookupDto;
using KPImanDental.Dto.PatientDto;
using KPImanDental.Model.Patient;

namespace KPImanDental.Interfaces
{
    public interface IPatientRespository
    {
        Task<IEnumerable<PatientTreatmentDto>> GetPatientTreatmentAsync(long id);
        Task<PatientDto> GetPatientByIdAsync(long id);
        Task<PatientTreatment> GetPatientTreatmentByIdAsync(long id);
        Task<PatientLookupDto> GetPatientLookupDtoByIdAsync(long id);
    }
}
