using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Rectorix.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion(1.0)]
    public abstract class RectorixBaseController : ControllerBase
    {
       
    }
}
