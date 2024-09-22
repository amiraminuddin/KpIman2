using AutoMapper;
using KPImanDental.Data;
using KPImanDental.Dto;
using KPImanDental.Dto.PatientDto;
using KPImanDental.Helpers;
using KPImanDental.Model.Lookup;
using KPImanDental.Model.Patient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace KPImanDental.Controllers
{
    [Authorize]
    public class PatientController : BaseAPIController
    {

        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public PatientController(DataContext context, IMapper mapper) {
        
            _dataContext = context;
            _mapper = mapper;
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

        [HttpPost("RegisterPatient")]
        public async Task<ActionResult<Patient>> RegisterPatient(PatientDto patientDto)
        {
            //Check Patient Exist
            var patient = await _dataContext.Patients.AnyAsync(x =>  x.FirstName.ToUpper() == patientDto.FirstName.ToUpper() && x.LastName.ToUpper() == patientDto.LastName.ToUpper());

            if (patient) return BadRequest("Patient already exist");

            var NewPatient = _mapper.Map<Patient>(patientDto);

            _dataContext.Patients.Add(NewPatient);
            await _dataContext.SaveChangesAsync();
            return Ok("Patient Created");
        }

        [HttpPut("UpdatePatient")]
        public async Task<ActionResult<Patient>> UpdatePatient(PatientDto patientDto)
        {
            var patient = await _dataContext.Patients.FindAsync(patientDto.Id);
            if (patient == null) return BadRequest("Patient Not Found");

            var UpdatedPatient = _mapper.Map<Patient>(patientDto);
            await _dataContext.SaveChangesAsync();
            return Ok("Patient Saved");
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
        public async Task<ActionResult<IEnumerable<PatientTreatmentDto>>> GetAllPatientTreament(long PatientId)
        {
            var patientTreaments = await _dataContext.PatientTreatments.Where(t => t.PatientId == PatientId).ToListAsync();

            if (patientTreaments == null) return BadRequest("No Treatment");

            var patientTreatmentList = _mapper.Map<IEnumerable<PatientTreatmentDto>>(patientTreaments);
            foreach (var item in patientTreatmentList)
            {
                item.DoctorName = await _dataContext.Users.FirstOrDefaultAsync(d => d.Id == item.DrID);
                item.DSAName = await _dataContext.Users.FirstOrDefaultAsync(d => d.Id == item.DSAId);
                item.PatientName = await _dataContext.Patients.FirstOrDefaultAsync(p => p.Id == item.PatientId);
                item.TreatmentName = await GetLookupTreatment(item.TreatmentType, "Name");
            }

            return Ok(patientTreatmentList);
        }

        [HttpGet("GetPatientTreatmentFormById")]
        public async Task<ActionResult<PatientTreamentFormDto>> GetPatientTreatmentFormById([FromQuery] long TreatmentId)
        {
            var patientTreament = await _dataContext.PatientTreatments.FirstOrDefaultAsync(x => x.Id == TreatmentId);
            if (patientTreament == null) return BadRequest("No Treatment");

            var patient = await _dataContext.Patients.FirstOrDefaultAsync(x => x.Id == patientTreament.PatientId);
            if (patient == null) return BadRequest("No patient");

            var dr = await _dataContext.Users.FirstOrDefaultAsync(d => d.Id == patientTreament.DrID);
            var dsa = await _dataContext.Users.FirstOrDefaultAsync(d => d.Id == patientTreament.DSAId);

            var patientTreatmentDto = _mapper.Map<PatientTreamentFormDto>(patientTreament);

            patientTreatmentDto.Patient = _mapper.Map<PatientDto>(patient);
            patientTreatmentDto.Dr = _mapper.Map<UserDto>(dr);
            patientTreatmentDto.DSA = _mapper.Map<UserDto>(dsa);

            return Ok(patientTreatmentDto);
        }

        [HttpPost("AddPatientTreatment")]
        public async Task<ActionResult<PatientTreatment>> AddPatientTreatment(PatientTreamentFormDto patientTreatmentFormDto)
        {
            var treatment = _mapper.Map<PatientTreatment>(patientTreatmentFormDto);
            int totalTreatment = await _dataContext.PatientTreatments.CountAsync(w => w.TreatmentType == treatment.TreatmentType);

            var dr = await _dataContext.Users.FirstOrDefaultAsync(d => d.Id == patientTreatmentFormDto.Dr.Id);
            var dsa = await _dataContext.Users.FirstOrDefaultAsync(d => d.Id == patientTreatmentFormDto.DSA.Id);
            var patient = await _dataContext.Patients.FirstOrDefaultAsync(d => d.Id == patientTreatmentFormDto.Patient.Id);

            treatment.DrID = dr.Id;
            treatment.DSAId = dsa.Id;
            treatment.PatientId = patient.Id;
            treatment.TreatmentNo = GenerateTreatmentNo(treatment.TreatmentType, totalTreatment).ToString();

            _dataContext.PatientTreatments.Add(treatment);
            await _dataContext.SaveChangesAsync();
            return Ok("Data Saved");
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

        #region Helper
        private async Task<string> GetLookupUser(long Id)
        {
            var result = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == Id);
            if (result == null) return "";
            return result.UserName.ToString();
        }

        private async Task<string> GetLookupTreatment(long TreatmentType, string type)
        {
            var result = "";
            var treatment = await _dataContext.TreatmentLookup.FirstOrDefaultAsync(t => t.Id == TreatmentType);
            if (treatment == null) return "";
            if(type == "Name")
            {
                result = treatment.TreatmentName.ToString();
            }
            if(type == "Code")
            {
                result = treatment.TreatmentCode.ToString();
            }

            return result;
        }

        private async Task<string> GenerateTreatmentNo(long treatmentType, int rowNum)
        {
            var treatmentCode = await GetLookupTreatment(treatmentType, "Code");
            int year = DateTime.Now.Year;

            return $"{treatmentCode}-{year}-{rowNum+1}";
        }

        private async Task<string> GetPatientName(long Id)
        {
            var result = await _dataContext.Patients.FirstOrDefaultAsync(p => p.Id == Id);
            if (result == null) return "";
            return result.FirstName.ToString() + ' ' + result.LastName.ToString();
        }
        #endregion
    }
}
