using AutoMapper;
using KPImanDental.Data;
using KPImanDental.Dto.LookupDto;
using KPImanDental.Interfaces;
using KPImanDental.Model.Lookup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KPImanDental.Controllers
{
    [Authorize]
    public class LookupController : BaseAPIController
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly ILookupRepository _lookupRepository;
        public LookupController(DataContext dataContext, IMapper mapper, ILookupRepository lookupRepository) {
            _dataContext = dataContext;
            _mapper = mapper;
            _lookupRepository = lookupRepository;
        }

        #region User Lookup
        [HttpGet("GetUserLookup")]
        public async Task<ActionResult<IEnumerable<StaffLookupDto>>> GetUserLookup(string position)
        {
            var user = await _dataContext.Users.Where(x => x.Position == position).ToListAsync();
            var userDtoList = _mapper.Map<IEnumerable<StaffLookupDto>>(user);
            return Ok(userDtoList);
        }
        #endregion

        #region Treatment Lookup
        [HttpPost("CreateOrUpdateTreatmentLookup")]
        public async Task<ActionResult> CreateOrUpdateTreatmentLookup(TreatmentLookupDto treatmentLookupDto)
        {
            if (treatmentLookupDto.Id.HasValue)
            {
                var updateTreament = await UpdateTreament(treatmentLookupDto);
                return Ok(updateTreament);
            }
            else
            {
                var createTreatment = await CreateTreatment(treatmentLookupDto);
                return Ok(createTreatment);
            }
        }

        [HttpGet("GetAllTreatment")]
        public async Task<ActionResult<IEnumerable<TreatmentLookupDto>>> GetAllTreatment()
        {
            var treatmentLookupList = await _dataContext.TreatmentLookup.ToListAsync();
            if (treatmentLookupList == null) { BadRequest("No Record Found"); }
            var treatmentLookupDtoList = _mapper.Map<IEnumerable<TreatmentLookupDto>>(treatmentLookupList);
            return Ok(treatmentLookupDtoList);
        }

        [HttpGet("GetTreatmentById")]
        public async Task<ActionResult<TreatmentLookupDto>> GetTreatmentById(long Id)
        {
            var treatmentLookup = await _dataContext.TreatmentLookup.FirstOrDefaultAsync(x => x.Id == Id);
            if (treatmentLookup == null) { BadRequest("No Record Found"); }
            var treatmentLookupDto = _mapper.Map<TreatmentLookupDto>(treatmentLookup);

            return Ok(treatmentLookupDto);
        }

        [HttpDelete("DeleteTreatment")]
        public async Task<ActionResult<string>> DeleteTreatment(long Id)
        {
            var treatmentLookup = await _dataContext.TreatmentLookup.FindAsync(Id);
            _dataContext.TreatmentLookup.Remove(treatmentLookup);
            await _dataContext.SaveChangesAsync();
            return Ok("Data Deleted!");
        }
        #endregion

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
