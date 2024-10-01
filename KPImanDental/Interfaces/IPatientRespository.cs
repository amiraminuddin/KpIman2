using KPImanDental.Dto.LookupDto;
using KPImanDental.Dto.PatientDto;
using KPImanDental.Model.Patient;

namespace KPImanDental.Interfaces
{
    public interface IPatientRepository
    {
        Task<IEnumerable<PatientDto>> GetAllPatientDtoAsync();
        Task<PatientDto> GetPatientDtoByIdAsync(long id);

        Task<Patient> GetPatientByIdAsync(long id);

        Task<IEnumerable<PatientTreatment>> GetPatientTreatmentAsync(long id);
        Task<PatientTreatment> GetPatientTreatmentByIdAsync(long id);

        Task<PatientLookupDto> GetPatientLookupDtoByIdAsync(long id);
    }
}
