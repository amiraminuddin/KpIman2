using AutoMapper;
using KPImanDental.Authorization;
using KPImanDental.Dto;
using KPImanDental.Dto.LookupDto;
using KPImanDental.Dto.PatientDto;
using KPImanDental.Model;
using KPImanDental.Model.Lookup;
using KPImanDental.Model.Patient;

namespace KPImanDental.Helpers
{
    public class AutoMappingProfile: Profile
    {
        public AuthService AuthService = new AuthService();
        public AutoMappingProfile() 
        {

            #region Mapper Register
            CreateMap<UserRegisterDto, KpImanUser>()
                .ForMember(dest => dest.PasswordHash, option =>
                    option.MapFrom(src => AuthService.GetPasswordHasher(src.Password).PasswordHash))
                .ForMember(dest => dest.PasswordSalt, option =>
                    option.MapFrom(src => AuthService.GetPasswordHasher(src.Password).PasswordSalt))
                .ForMember(dest => dest.CreatedBy, option =>
                    option.MapFrom(src => src.UserName))
                .ForMember(dest => dest.CreatedOn, option =>
                    option.MapFrom(dest => DateTime.Now));
            #endregion


            #region Mapper User
            CreateMap<KpImanUser, UserListDto>();
            CreateMap<KpImanUser, UserDto>();
            CreateMap<UserDto, KpImanUser>();
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
