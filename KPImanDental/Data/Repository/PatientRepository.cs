using AutoMapper;
using KPImanDental.Dto.LookupDto;
using KPImanDental.Dto.PatientDto;
using KPImanDental.Interfaces;
using KPImanDental.Model.Patient;

namespace KPImanDental.Data.Repository
{
    public class PatientRepository : IPatientRespository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public PatientRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PatientDto> GetPatientByIdAsync(long id)
        {
            var patient = await _context.Patients.FindAsync(id);
            var patientDto = _mapper.Map<PatientDto>(patient);

            return patientDto;
        }

        public async Task<PatientLookupDto> GetPatientLookupDtoByIdAsync(long id)
        {
            var patient = await _context.Patients.FindAsync(id);
            var patientLookupDto = _mapper.Map<PatientLookupDto>(patient);
            return patientLookupDto;
        }

        public Task<IEnumerable<PatientTreatmentDto>> GetPatientTreatmentAsync(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<PatientTreatment> GetPatientTreatmentByIdAsync(long id)
        {
            var patientTreatment = await _context.PatientTreatments.FindAsync(id);

            return patientTreatment;
        }
    }
}
