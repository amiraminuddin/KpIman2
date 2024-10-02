using AutoMapper;
using KPImanDental.Data;
using KPImanDental.Dto.LookupDto;
using KPImanDental.Dto.PatientDto;
using KPImanDental.Interfaces.Repositories;
using KPImanDental.Model.Patient;
using Microsoft.EntityFrameworkCore;

namespace KPImanDental.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public PatientRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PatientDto>> GetAllPatientDtoAsync()
        {
            var patients = await _context.Patients.ToListAsync();
            var patientList = _mapper.Map<IEnumerable<PatientDto>>(patients);
            return patientList;
        }

        public async Task<PatientDto> GetPatientDtoByIdAsync(long id)
        {
            var patient = await _context.Patients.FindAsync(id);
            var patientDto = _mapper.Map<PatientDto>(patient);

            return patientDto;
        }

        public async Task<Patient> GetPatientByIdAsync(long id)
        {
            var patient = await _context.Patients.FindAsync(id);
            return patient;
        }

        public async Task<IEnumerable<PatientTreatment>> GetPatientTreatmentAsync(long id)
        {
            var patientTreatment = await _context.PatientTreatments.Where(t => t.PatientId == id).ToListAsync();
            return patientTreatment;
        }

        public async Task<PatientTreatment> GetPatientTreatmentByIdAsync(long id)
        {
            var patientTreatment = await _context.PatientTreatments.FindAsync(id);

            return patientTreatment;
        }

        public async Task<PatientLookupDto> GetPatientLookupDtoByIdAsync(long id)
        {
            var patient = await _context.Patients.FindAsync(id);
            var patientLookupDto = _mapper.Map<PatientLookupDto>(patient);
            return patientLookupDto;
        }


    }
}
