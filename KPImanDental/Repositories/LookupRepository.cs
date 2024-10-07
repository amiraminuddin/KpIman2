using KPImanDental.Data;
using KPImanDental.Dto.LookupDto;
using KPImanDental.Interfaces.Repositories;

namespace KPImanDental.Repositories
{
    public class LookupRepository : ILookupRepository
    {
        private readonly DataContext dataContext;
        private readonly IUserRepository userRespository;
        private readonly IPatientRepository patientRepository;

        public LookupRepository(DataContext dataContext, IUserRepository userRespository, IPatientRepository patientRepository)
        {
            this.dataContext = dataContext;
            this.userRespository = userRespository;
            this.patientRepository = patientRepository;
        }

        public async Task<LookupTemplateDto> GetDepartmentLookup(string code)
        {
            var department = await userRespository.GetDepartmentByCodeAsync(code);

            return new LookupTemplateDto
            {
                FieldValue = department.Code,
                FieldDisplay = department.Name,
            };
        }

        public async Task<LookupTemplateDto> GetKPImanUserLookup(long Id)
        {
            var user = await userRespository.GetUserByIdAsync(Id);

            if (user != null)
            {
                return new LookupTemplateDto
                {
                    FieldValue = user.Id.ToString(),
                    FieldDisplay = user.UserName
                };
            }

            return new LookupTemplateDto();

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

        public async Task<LookupTemplateDto> GetPositionLookup(string code)
        {
            var position = await userRespository.GetPositionByCodeAsync(code);
            return new LookupTemplateDto
            {
                FieldValue = position.Code.ToString(),
                FieldDisplay = position.Name.ToString()
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
