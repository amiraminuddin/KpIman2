using KPImanDental.Dto.LookupDto;
using KPImanDental.Interfaces;

namespace KPImanDental.Data.Repository
{
    public class LookupRepository : ILookupRepository
    {
        private readonly DataContext dataContext;
        private readonly IUserRepository userRespository;
        private readonly IPatientRepository patientRepository;

        public LookupRepository(DataContext dataContext, IUserRepository userRespository, IPatientRepository patientRepository) { 
            this.dataContext = dataContext;
            this.userRespository = userRespository;
            this.patientRepository = patientRepository;
        }
        public async Task<LookupTemplateDto> GetKPImanUserLookup(long Id)
        {
            var user = await userRespository.GetUserByIdAsync(Id);

            return new LookupTemplateDto
            {
                FieldValue = user.Id.ToString(),
                FieldDisplay = user.UserName
            };
        }

        public async Task<LookupTemplateDto> GetPatientLookup(long Id)
        {
            var patient = await patientRepository.GetPatientByIdAsync(Id);

            return new LookupTemplateDto
            {
                FieldValue = patient.Id.ToString(),
                FieldDisplay = patient.FirstName
            };
        }

        public async Task<LookupTemplateDto> GetTreatmentLookup(long Id)
        {
            var treatment = await dataContext.TreatmentLookup.FindAsync(Id);

            return new LookupTemplateDto 
            {
                FieldValue = treatment.Id.ToString(),
                FieldDisplay = treatment.TreatmentName
            };
        }
    }
}
