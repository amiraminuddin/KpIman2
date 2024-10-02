using KPImanDental.Model;

namespace KPImanDental.Interfaces.Services
{
    public interface ITokenService
    {
        string CreateToken(KpImanUser user);
    }
}
