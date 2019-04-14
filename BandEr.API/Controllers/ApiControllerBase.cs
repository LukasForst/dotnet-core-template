using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BandEr.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize()]
    public abstract class ApiControllerBase : ControllerBase
    {
    }
}
