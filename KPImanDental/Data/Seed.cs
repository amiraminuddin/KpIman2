using KPImanDental.Model;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace KPImanDental.Data
{
    public class Seed
    {
        public static async Task SeedModules(DataContext context)
        {
            if (await context.Modules.AnyAsync()) return;

            var ModuleData = await File.ReadAllTextAsync("Data/ModuleSeedData.json");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var Modules = JsonSerializer.Deserialize<List<Modules>>(ModuleData, options);

            //if (Modules == null) return;
            
            foreach (var module in Modules)
            {
                context.Modules.Add(module);
            }
            await context.SaveChangesAsync();
        }
    }
}
