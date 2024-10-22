using KPImanDental.Dto.GridDto;
using KPImanDental.Dto.LookupDto;

namespace KPImanDental.Interfaces.Services
{
    public interface ILookupService
    {

        Task<IEnumerable<StaffLookupDto>> GetUserLookup(string position);
        Task<long> CreateOrUpdateTreatmentLookup(TreatmentLookupDto treatmentLookupDto);
        Task<IEnumerable<TreatmentLookupDto>> GetAllTreatment();
        Task<GridDto<IEnumerable<TreatmentLookupDto>>> GetGridTreatment(GridInputDto gridInputDto);
        Task<TreatmentLookupDto> GetTreatmentById(long id);
        Task<string> DeleteTreatment(long id);
        Task<IEnumerable<StaffLookupDto>> GetUserLookupByHierachyLevel(int hierachyLevel);
    }
}
