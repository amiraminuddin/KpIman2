using Microsoft.AspNetCore.Mvc;

namespace KPImanDental.Controllers
{
    [ApiController]//API Controller attribute
    [Route("api/[controller]")] //create endpoint
    [Produces("application/json")]
    public class BaseAPIController : ControllerBase
    {

    }
}
