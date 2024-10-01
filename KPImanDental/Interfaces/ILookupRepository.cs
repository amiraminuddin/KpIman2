using KPImanDental.Dto.LookupDto;

namespace KPImanDental.Interfaces
{
    public interface ILookupRepository
    {
        Task<LookupTemplateDto> GetKPImanUserLookup(long Id);
        Task<LookupTemplateDto> GetPatientLookup(long Id);
        Task<LookupTemplateDto> GetTreatmentLookup(long Id);
    }
}
