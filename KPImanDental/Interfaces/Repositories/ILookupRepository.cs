using KPImanDental.Dto.LookupDto;

namespace KPImanDental.Interfaces.Repositories
{
    public interface ILookupRepository
    {
        Task<LookupTemplateDto> GetKPImanUserLookup(long Id);
        Task<LookupTemplateDto> GetPatientLookup(long Id);
        Task<LookupTemplateDto> GetTreatmentLookup(long Id);
    }
}
