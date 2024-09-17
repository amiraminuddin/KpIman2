using KPImanDental.Model;

namespace KPImanDental.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(KpImanUser user);
    }
}
