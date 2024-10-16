using AutoMapper;
using KPImanDental.Dto;
using KPImanDental.Dto.LookupDto;
using KPImanDental.Dto.PatientDto;
using KPImanDental.Dto.UserDto;
using KPImanDental.Model;
using KPImanDental.Model.Lookup;
using KPImanDental.Model.Patient;

namespace KPImanDental.Helpers
{
    public class AutoMappingProfile: Profile
    {
        public AutoMappingProfile() 
        {

            #region Mapper User Register
            CreateMap<UserCreateDto, KpImanUser>();
            #endregion


            #region Mapper User
            //View in list and form
            CreateMap<KpImanUser, UserDtoExt>();
            CreateMap<KpImanUser, UserCreateDto>();
            CreateMap<UserDto, KpImanUser>();

            //CreateMap<KpImanUser, UserDto>();
            //CreateMap<UserDto, KpImanUser>();
            //CreateMap<KpImanUser, UserDtoExt>();
            #endregion

            #region Mapper Module
            CreateMap<Modules, ModuleDto>();
            CreateMap<ModuleUpdateDto, Modules>();
            #endregion

            #region Department and Position
            CreateMap<Department, DepartmentDto>();
            CreateMap<DepartmentDto, Department>();
            CreateMap<Position, PositionDto>(); // Mapping DepartmentCode
            CreateMap<PositionDto, Position>();
            CreateMap<Position, PositionDtoExt>();
            #endregion

            #region Patient
            CreateMap<PatientDto, Patient>(); //Register
            CreateMap<Patient, PatientDto>(); //ToList

            CreateMap<PatientDocumentDto, PatientDocuments>();
            CreateMap<PatientDocuments, PatientDocumentDto>();

            CreateMap<PatientTreatmentDto, PatientTreatment>();
            CreateMap<PatientTreatment, PatientTreatmentDto>();

            CreateMap<PatientTreatment, PatientTreatmentDtoExt>();
            #endregion

            #region Lookup
            CreateMap<TreatmentLookupDto, TreatmentLookup>();
            CreateMap<TreatmentLookup, TreatmentLookupDto>();

            CreateMap<Patient, PatientLookupDto>();
            CreateMap<KpImanUser, StaffLookupDto>();
            #endregion
        }
    }
}
