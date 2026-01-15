using FinancialLedger.Communication.Response;
using Microsoft.AspNetCore.Mvc;

namespace FinancialLedger.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase {
  [HttpGet]
  [ProducesResponseType<ResponseAccount>(StatusCodes.Status200OK)]
  [ProducesResponseType<ResponseError>(StatusCodes.Status500InternalServerError)]
  public IActionResult GetAllAccount() {
    return Ok();
  }

  [HttpPost]
  [ProducesResponseType<ResponseAccount>(StatusCodes.Status201Created)]
  [ProducesResponseType<ResponseError>(StatusCodes.Status500InternalServerError)]
  public IActionResult CreateAccount() {
    return Ok();
  }
}
