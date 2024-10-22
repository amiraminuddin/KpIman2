using AutoMapper;
using KPImanDental.Data;
using KPImanDental.Dto.GridDto;
using KPImanDental.Dto.LookupDto;
using KPImanDental.Interfaces.Repositories;
using KPImanDental.Interfaces.Services;
using KPImanDental.Model.Lookup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KPImanDental.Controllers
{
    [Authorize]
    public class LookupController : BaseAPIController
    {
        private readonly ILookupService _lookupService;
        public LookupController(ILookupService lookupService) {
            _lookupService = lookupService;
        }

        #region User Lookup
        [HttpGet("GetUserLookup")]
        public async Task<ActionResult<IEnumerable<StaffLookupDto>>> GetUserLookup(string position)
        {
            var result = await _lookupService.GetUserLookup(position);
            return Ok(result);
        }

        [HttpGet("GetUserLookupByHierachyLevel")]
        public async Task<ActionResult<IEnumerable<StaffLookupDto>>> GetUserLookupByHierachyLevel(int hierachyLevel)
        {
            var result = await _lookupService.GetUserLookupByHierachyLevel(hierachyLevel);
            return Ok(result);
        }

        [HttpPost("GetGridTreatment")]
        public async Task<ActionResult<GridDto<IEnumerable<TreatmentLookupDto>>>> GetGridTreatment(GridInputDto gridInput)
        {
            var response = await _lookupService.GetGridTreatment(gridInput);
            return Ok(response);
        }
        #endregion

        #region Treatment Lookup
        [HttpPost("CreateOrUpdateTreatmentLookup")]
        public async Task<ActionResult> CreateOrUpdateTreatmentLookup(TreatmentLookupDto treatmentLookupDto)
        {
            var result = await _lookupService.CreateOrUpdateTreatmentLookup(treatmentLookupDto);
            return Ok(result);
        }

        [HttpGet("GetAllTreatment")]
        public async Task<ActionResult<IEnumerable<TreatmentLookupDto>>> GetAllTreatment()
        {
            var result = await _lookupService.GetAllTreatment();
            return Ok(result);
        }

        [HttpGet("GetTreatmentById")]
        public async Task<ActionResult<TreatmentLookupDto>> GetTreatmentById(long Id)
        {
            var result = await _lookupService.GetTreatmentById(Id);
            return Ok(result);
        }

        [HttpDelete("DeleteTreatment")]
        public async Task<ActionResult<string>> DeleteTreatment(long Id)
        {
            var result = await _lookupService.DeleteTreatment(Id);
            return Ok(result);
        }

        #endregion
    }
}
