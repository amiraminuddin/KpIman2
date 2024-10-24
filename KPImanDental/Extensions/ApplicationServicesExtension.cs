using KPImanDental.Data;
using KPImanDental.Helpers;
using KPImanDental.Interfaces.Repositories;
using KPImanDental.Interfaces.Services;
using KPImanDental.Repositories;
using KPImanDental.Services;
using Microsoft.EntityFrameworkCore;


namespace KPImanDental.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            
            services.AddCors();
            services.AddHttpContextAccessor();
            services.AddScoped<GetUserSession>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<ILookupRepository, LookupRepository>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<ILookupService, LookupService>();
            services.AddControllersWithViews();
            services.AddDbContext<DataContext>(option =>
                option.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))); //Required EntityFramework.SqlServer

            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarSettings"));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}
