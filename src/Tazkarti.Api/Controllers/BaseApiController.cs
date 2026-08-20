using Microsoft.AspNetCore.Mvc;

namespace Tazkarti.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public abstract class BaseApiController : ControllerBase
{
}
