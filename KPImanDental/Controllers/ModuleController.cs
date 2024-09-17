using AutoMapper;
using KPImanDental.Data;
using KPImanDental.Dto;
using KPImanDental.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KPImanDental.Controllers
{
    [Authorize]
    public class ModuleController : BaseAPIController
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ModuleController(DataContext context, IMapper mapper) 
        { 
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("GetAllModule")]
        public async Task<ActionResult<IEnumerable<ModuleDto>>> GetAllModule()
        {
            var modules = await _context.Modules.ToListAsync();

            //will use Repo if need to use add many place.
            var moduleListDto = _mapper.Map<IEnumerable<ModuleDto>>(modules).Select((module, index) =>
            {
                module.RowNumber = index + 1;
                return module;
            }).ToList();

            return moduleListDto;
        }

        [HttpGet("GetModuleById")]
        public async Task<ActionResult<Modules>> GetModuleForEdit([FromQuery] long Id)
        {
            var module = await _context.Modules.FindAsync(Id);
            return module;
        }

        [HttpPut("UpdateModule")]
        public async Task<ActionResult<Modules>> UpdateModule(ModuleUpdateDto moduledto)
        {
            var getModule = await _context.Modules.FirstOrDefaultAsync(x => x.Id == moduledto.Id);

            if (getModule == null) return NotFound("Module not Found!!");

            var updatedModule = _mapper.Map(moduledto, getModule);

            await _context.SaveChangesAsync();

            return Ok(updatedModule);
        }
    }
}
