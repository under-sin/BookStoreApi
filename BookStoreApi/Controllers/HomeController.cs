using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers;

[ApiController]
[Route("")]
public class HomeController : ControllerBase
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("")]
    public IActionResult Index() => Ok("Healthy");
}
