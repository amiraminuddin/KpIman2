using AutoMapper;
using KPImanDental.Data;
using KPImanDental.Dto;
using KPImanDental.Dto.GridDto;
using KPImanDental.Dto.LookupDto;
using KPImanDental.Dto.UserDto;
using KPImanDental.Helpers;
using KPImanDental.Interfaces.Repositories;
using KPImanDental.Interfaces.Services;
using KPImanDental.Model.Lookup;
using Microsoft.EntityFrameworkCore;

namespace KPImanDental.Services
{
    public class LookupService : ILookupService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly ILookupRepository _lookupRepository;

        public LookupService(DataContext dataContext, IMapper mapper, ILookupRepository lookupRepository)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _lookupRepository = lookupRepository;
        }

        public async Task<long> CreateOrUpdateTreatmentLookup(TreatmentLookupDto treatmentLookupDto)
        {
            if (treatmentLookupDto.Id.HasValue)
            {
                var updateTreament = await UpdateTreament(treatmentLookupDto);
                return updateTreament;
            }
            else
            {
                var createTreatment = await CreateTreatment(treatmentLookupDto);
                return createTreatment;
            }
        }

        public async Task<string> DeleteTreatment(long id)
        {
            var treatmentLookup = await _dataContext.TreatmentLookup.FindAsync(id);
            _dataContext.TreatmentLookup.Remove(treatmentLookup);
            await _dataContext.SaveChangesAsync();
            return "Data Deleted!";
        }

        public async Task<IEnumerable<TreatmentLookupDto>> GetAllTreatment()
        {
            var treatmentLookupList = await _dataContext.TreatmentLookup.ToListAsync();
            var treatmentLookupDtoList = _mapper.Map<IEnumerable<TreatmentLookupDto>>(treatmentLookupList);
            return treatmentLookupDtoList;
        }

        public async Task<GridDto<IEnumerable<TreatmentLookupDto>>> GetGridTreatment(GridInputDto gridInput)
        {
            var result = await GridHelper.GetGridDataAsync<TreatmentLookup, TreatmentLookupDto>(
                _dataContext.TreatmentLookup.AsQueryable(),
                _mapper,
                gridInput
            );

            return result;
        }

        public async Task<TreatmentLookupDto> GetTreatmentById(long id)
        {
            var treatmentLookup = await _dataContext.TreatmentLookup.FirstOrDefaultAsync(x => x.Id == id);
            var treatmentLookupDto = _mapper.Map<TreatmentLookupDto>(treatmentLookup);

            return treatmentLookupDto;
        }

        public async Task<IEnumerable<StaffLookupDto>> GetUserLookup(string position)
        {
            var user = await _dataContext.Users.Where(x => x.Position == position).ToListAsync();
            var userDtoList = _mapper.Map<IEnumerable<StaffLookupDto>>(user);
            return userDtoList;
        }

        public async Task<IEnumerable<StaffLookupDto>> GetUserLookupByHierachyLevel(int hierachyLevel)
        {
            var targetHierarchyLevel = hierachyLevel - 1;
            var users = await _dataContext.Users.Where(x => x.HierarchyLevel == targetHierarchyLevel).ToListAsync();
            var userDtoList = _mapper.Map<IEnumerable<StaffLookupDto>>(users);
            foreach(var user in userDtoList)
            {
                var userData = users.Find(x => x.Id == user.Id);
                user.DepartmentL = await _lookupRepository.GetDepartmentLookup(userData.Department);
                user.PositionL = await _lookupRepository.GetPositionLookup(userData.Position);
            }
            return userDtoList;
        }

        #region Private Method
        private async Task<long> CreateTreatment(TreatmentLookupDto treatmentLookupDto)
        {
            var treatment = _mapper.Map<TreatmentLookup>(treatmentLookupDto);

            treatment.CreatedBy = "System";
            treatment.UpdatedOn = DateTime.Now;
            treatment.UpdatedBy = "System";
            treatment.UpdatedOn = DateTime.Now;

            _dataContext.TreatmentLookup.Add(treatment);
            await _dataContext.SaveChangesAsync();
            return treatment.Id;
        }

        private async Task<long> UpdateTreament(TreatmentLookupDto treatmentLookupDto)
        {
            var treatment = await _dataContext.TreatmentLookup.FindAsync(treatmentLookupDto.Id);
            if (treatment == null) return -1;

            treatment.UpdatedBy = "Don";
            treatment.UpdatedOn = DateTime.Now;

            _mapper.Map(treatmentLookupDto, treatment);
            await _dataContext.SaveChangesAsync();

            return treatment.Id;
        }
        #endregion
    }
}
