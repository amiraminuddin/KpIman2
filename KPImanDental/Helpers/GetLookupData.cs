using AutoMapper;
using KPImanDental.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KPImanDental.Helpers
{
    public class GetLookupData
    {
        private readonly DataContext _dataContext;

        public GetLookupData(DataContext context)
        {

            _dataContext = context;
        }

        public async Task<string> GetDoctorLookup(long Id)
        {
            var result = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == Id);
            if (result == null) return ""; 
            return result.UserName.ToString();
        }
    }
}
