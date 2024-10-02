using AutoMapper;
using KPImanDental.Data;
using KPImanDental.Dto.LookupDto;
using KPImanDental.Dto.PatientDto;
using KPImanDental.Interfaces.Repositories;
using KPImanDental.Model.Patient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KPImanDental.Controllers
{
    [Authorize]
    public class PatientController : BaseAPIController
    {

        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepo;
        private readonly IPatientRepository _patientRepo;
        private readonly ILookupRepository _lookupRepo;

        public PatientController(
            DataContext context, 
            IMapper mapper, 
            IUserRepository userRepository, 
            IPatientRepository patientRespository,
            ILookupRepository lookupRepository) {
        
            _dataContext = context;
            _mapper = mapper;
            _userRepo = userRepository;
            _patientRepo = patientRespository;
            _lookupRepo = lookupRepository;
        }

        #region Patient
        [HttpGet("GetAllPatients")]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetAllPatients()
        {
            var patientList = await _patientRepo.GetAllPatientDtoAsync();

            return Ok(patientList);
        }

        [HttpGet("GetPatientsById")]
        public async Task<ActionResult<PatientDto>> GetPatientById(long Id)
        {
            var patientDto = await _patientRepo.GetPatientDtoByIdAsync(Id);
            return Ok(patientDto);
        }

        [HttpPost("CreateOrUpdatePatient")]
        public async Task<ActionResult> CreateOrUpdatePatient(PatientDto patientDto)
        {
            if (patientDto.Id.HasValue)
            {
                var updatedPatient = await UpdatePatient(patientDto);
                return Ok(updatedPatient);
            }
            else
            {
                var createPatient = await CreatePatient(patientDto);
                return Ok(createPatient);
            }
        }

        [HttpDelete("DeletePatient")]
        public async Task<ActionResult<string>> DeletePatient(long Id)
        {
            var patient = await _patientRepo.GetPatientByIdAsync(Id);
            if (patient == null) return BadRequest("Patient Not Found");
            _dataContext.Remove(patient);
            await _dataContext.SaveChangesAsync();
            return Ok("Patient Deleted");
        }
        #endregion

        #region Patient Treatment
        [HttpGet("GetAllPatientTreatment")]
        public async Task<ActionResult<IEnumerable<PatientTreatmentDtoExt>>> GetAllPatientTreament(long PatientId)
        {
            var patientTreaments = await _patientRepo.GetPatientTreatmentAsync(PatientId);

            if (patientTreaments == null) return BadRequest("No Treatment");

            var patientTreatmentList = _mapper.Map<IEnumerable<PatientTreatmentDtoExt>>(patientTreaments);
            foreach (var item in patientTreatmentList)
            {
                item.Doctor = await _lookupRepo.GetKPImanUserLookup(item.DrID);
                item.DSA = await _lookupRepo.GetKPImanUserLookup(item.DSAId);
                item.Treatment = await _lookupRepo.GetTreatmentLookup(item.TreatmentType);
            }

            return Ok(patientTreatmentList);
        }

        [HttpGet("GetPatientTreatmentFormById")]
        public async Task<ActionResult<PatientTreatmentDtoExt>> GetPatientTreatmentFormById(long TreatmentId)
        {
            var patientTreament = await _patientRepo.GetPatientTreatmentByIdAsync(TreatmentId);
            if (patientTreament == null) return BadRequest("No Treatment");

            var patientTreatmentDto = _mapper.Map<PatientTreatmentDtoExt>(patientTreament);

            var patientDto = await _patientRepo.GetPatientByIdAsync(patientTreatmentDto.PatientId);
            patientTreatmentDto.PatientName = patientDto.FirstName;
            patientTreatmentDto.Doctor = await _lookupRepo.GetKPImanUserLookup(patientTreatmentDto.DrID);
            patientTreatmentDto.DSA = await _lookupRepo.GetKPImanUserLookup(patientTreatmentDto.DSAId);
            patientTreatmentDto.Treatment = await _lookupRepo.GetTreatmentLookup(patientTreatmentDto.TreatmentType);

            return Ok(patientTreatmentDto);
        }

        [HttpPost("CreateOrUpdatePatientTreatment")]
        public async Task<ActionResult<PatientTreatment>> CreateOrUpdatePatientTreatment(PatientTreatmentDto patientTreatmentDto)
        {
            if(patientTreatmentDto.Id.HasValue)
            {
                var updateData = await UpdatePatientTreatment(patientTreatmentDto);
                return Ok(updateData);
            }
            else
            {
                var createDate = await CreatePatientTreatment(patientTreatmentDto);
                return Ok(createDate);
            }
        }

        [HttpDelete("DeletePatientTreatment")]
        public async Task<ActionResult> DeletePatientTreatment(long Id)
        {
            var patientTreatment = await _patientRepo.GetPatientTreatmentByIdAsync(Id);
            if (patientTreatment == null) return BadRequest("Record Not Found");
            _dataContext.Remove(patientTreatment);
            await _dataContext.SaveChangesAsync();
            return Ok("Data Deleted");
        }
        #endregion

        #region Document
        [HttpGet("GetDocumentByTreatmentId")]
        public async Task<ActionResult<IEnumerable<PatientDocumentDto>>> GetDocumentByTreatmentId(long TreatmentId)
        {
            var documents = await _dataContext.PatientDocuments.Where(d => d.PatientId == TreatmentId).ToListAsync();

            if (documents.Count == 0) return BadRequest("No Document");

            var documentList = _mapper.Map<IEnumerable<PatientDocumentDto>>(documents);

            return Ok(documentList);
        }

        [HttpPost("AddPatientDocument")]
        public async Task<ActionResult<PatientDocuments>> AddPatientDocument(PatientDocumentDto documentDto)
        {
            var document = _mapper.Map<PatientDocuments>(documentDto);
            await _dataContext.SaveChangesAsync();
            //TODO : Save into Cloud
            return Ok(document);
        }

        [HttpDelete("DeletePatientDocument")]
        public async Task<ActionResult<string>> DeletePatientDocument(long Id)
        {
            var document = _dataContext.PatientDocuments.FirstOrDefaultAsync(d => d.Id == Id);
            if (document == null) return BadRequest("Document Not Found");

            _dataContext.Remove(document);
            await _dataContext.SaveChangesAsync();
            //TODO: delete from cloud

            return Ok("Document Deleted");

        }

        [HttpPut("UpdatePatientDocument")]
        public async Task<ActionResult<PatientDocuments>> UpdatePatientDocument(PatientDocumentDto documentDto)
        {
            var document = _dataContext.PatientDocuments.FirstOrDefaultAsync(d => d.Id == documentDto.Id);
            if (document == null) return BadRequest("Document Not Found");

            var updatedDocument = _mapper.Map<PatientDocuments>(documentDto);
            await _dataContext.SaveChangesAsync();

            return Ok(updatedDocument);
        }
        #endregion

        #region Private Method
        private async Task<TreatmentLookupDto> GetLookupTreatment(long TreatmentId)
        {
            var treatmentLookup = await _dataContext.TreatmentLookup.FirstOrDefaultAsync(t => t.Id == TreatmentId);
            
            var treatmentLookupDto = _mapper.Map<TreatmentLookupDto>(treatmentLookup);

            return treatmentLookupDto;
        }

        private async Task<string> GenerateTreatmentNo(long treatmentType)
        {
            int nextNumber = 1; // Default value if no records exist
            var treatmentCode = await GetLookupTreatment(treatmentType);
            int year = DateTime.Now.Year;
            var treatmentCount = await _dataContext.PatientTreatments.CountAsync(x => x.TreatmentType == treatmentType && x.TreatmentDate.Year == year);
            var lastRecord = await _dataContext.PatientTreatments.Where(x => x.TreatmentType == treatmentType && x.TreatmentDate.Year == year)
                .OrderByDescending(r => r.Id).FirstOrDefaultAsync();

            if (lastRecord != null) {
                var lastNumber = lastRecord.TreatmentNo.Substring(lastRecord.TreatmentNo.Length - 4);
                if (int.TryParse(lastNumber, out int lastCount))
                {
                    nextNumber = lastCount + 1; // Increment the number
                }
            }

            var treatmentNo = nextNumber.ToString("D4");

            return $"TRM-{treatmentCode.TreatmentCode}-{year}-{treatmentNo}";
        }

        private async Task<long> CreatePatient(PatientDto patientDto)
        {
            var patient = _mapper.Map<Patient>(patientDto);

            patient.CreatedBy = "System";
            patient.CreatedOn = DateTime.Now;
            patient.UpdatedBy = "System";
            patient.UpdatedOn = DateTime.Now;

            _dataContext.Patients.Add(patient);
            await _dataContext.SaveChangesAsync();
            return patient.Id;
        }

        private async Task<long> UpdatePatient(PatientDto patientDto)
        {
            var patient = await _patientRepo.GetPatientByIdAsync((long)patientDto.Id);
            if (patient == null) return -1;

            patient.UpdatedBy = "System";
            patient.UpdatedOn = DateTime.Now;

            _mapper.Map(patientDto, patient);
            await _dataContext.SaveChangesAsync();

            return patient.Id;
        }

        private async Task<long> CreatePatientTreatment(PatientTreatmentDto patientTreatmentDto)
        {
            var patientTreatment = _mapper.Map<PatientTreatment>(patientTreatmentDto);

            patientTreatment.TreatmentNo = await GenerateTreatmentNo(patientTreatmentDto.TreatmentType);
            patientTreatment.CreatedBy = "System";
            patientTreatment.CreatedOn = DateTime.Now;
            patientTreatment.UpdatedBy = "System";
            patientTreatment.UpdatedOn = DateTime.Now;

            _dataContext.PatientTreatments.Add(patientTreatment);
            await _dataContext.SaveChangesAsync();
            return patientTreatment.Id;
        }

        private async Task<long> UpdatePatientTreatment(PatientTreatmentDto patientTreatmentDto)
        {
            var patientTreatment = await _patientRepo.GetPatientTreatmentByIdAsync((long)patientTreatmentDto.Id);
            if (patientTreatment == null) return -1;

            patientTreatment.UpdatedBy = "System";
            patientTreatment.UpdatedOn = DateTime.Now;

            _mapper.Map(patientTreatmentDto, patientTreatment);
            await _dataContext.SaveChangesAsync();

            return patientTreatment.Id;
        }
        #endregion
    }
}
