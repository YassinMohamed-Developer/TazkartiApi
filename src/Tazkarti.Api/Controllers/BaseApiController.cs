using Microsoft.AspNetCore.Mvc;

namespace Tazkarti.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
}
