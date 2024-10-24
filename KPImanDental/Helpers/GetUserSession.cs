using System.Security.Claims;

namespace KPImanDental.Helpers
{
    public class GetUserSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetUserSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public string GetUser()
        {
            return _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
