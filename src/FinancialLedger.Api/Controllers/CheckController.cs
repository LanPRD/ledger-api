using Microsoft.AspNetCore.Mvc;

namespace FinancialLedger.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CheckController : ControllerBase {
  [HttpGet]
  public IActionResult CheckApplication() {
    return Ok();
  }
}
