using AutoMapper;
using KPImanDental.Data;
using KPImanDental.Dto.LookupDto;
using KPImanDental.Dto.PatientDto;
using KPImanDental.Interfaces;
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
        private readonly IPatientRespository _patientRepo;

        public PatientController(DataContext context, IMapper mapper, IUserRepository userRepository, IPatientRespository patientRespository) {
        
            _dataContext = context;
            _mapper = mapper;
            _userRepo = userRepository;
            _patientRepo = patientRespository;
        }

        #region Patient
        [HttpGet("GetAllPatients")]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetAllPatients()
        {
            var patients = await _dataContext.Patients.ToListAsync();
            if (patients == null) return BadRequest("No Record");

            var patientList = _mapper.Map<IEnumerable<PatientDto>>(patients);

            return Ok(patientList);
        }

        [HttpGet("GetPatientsById")]
        public async Task<ActionResult<PatientDto>> GetPatientById(long Id)
        {
            var patient = await _dataContext.Patients.FindAsync(Id);
            if (patient == null) return BadRequest("Patient Not Found");

            var patientDto = _mapper.Map<PatientDto>(patient);
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
            var patient = await _dataContext.Patients.FirstOrDefaultAsync(p => p.Id == Id);
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
            var patientTreaments = await _dataContext.PatientTreatments.Where(t => t.PatientId == PatientId).ToListAsync();

            if (patientTreaments == null) return BadRequest("No Treatment");

            var patientTreatmentList = _mapper.Map<IEnumerable<PatientTreatmentDtoExt>>(patientTreaments);
            foreach (var item in patientTreatmentList)
            {
                item.Doctor = await _userRepo.GetUserLookupDtoByIdAsync(item.PatientId);
                item.DSA = await _userRepo.GetUserLookupDtoByIdAsync(item.DSAId);
                item.Treatment = await GetLookupTreatment(item.TreatmentType);
            }

            return Ok(patientTreatmentList);
        }

        [HttpGet("GetPatientTreatmentFormById")]
        public async Task<ActionResult<PatientTreatmentDto>> GetPatientTreatmentFormById(long TreatmentId)
        {
            var patientTreament = await _patientRepo.GetPatientTreatmentByIdAsync(TreatmentId);
            if (patientTreament == null) return BadRequest("No Treatment");

            var patientTreatmentDto = _mapper.Map<PatientTreatmentDto>(patientTreament);

            return Ok(patientTreatmentDto);
        }

        [HttpPost("CreateOrUpdatePatientTreatment")]
        public async Task<ActionResult<PatientTreatment>> CreateOrUpdatePatientTreatment(PatientTreatmentDto patientTreatmentDto)
        {
            if(patientTreatmentDto.Id.HasValue)
            {
                var updateData = await CreatePatientTreatment(patientTreatmentDto);
                return Ok(updateData);
            }
            else
            {
                var createDate = await CreatePatientTreatment(patientTreatmentDto);
                return Ok(createDate);
            }
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

        //private async Task<string> GenerateTreatmentNo(long treatmentType, int rowNum)
        //{
        //    var treatmentCode = await GetLookupTreatment(treatmentType, "Code");
        //    int year = DateTime.Now.Year;

        //    return $"{treatmentCode}-{year}-{rowNum+1}";
        //}

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
            var patient = await _dataContext.Patients.FindAsync(patientDto.Id);
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
