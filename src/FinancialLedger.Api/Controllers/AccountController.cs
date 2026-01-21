using FinancialLedger.Application.UseCases.Account.Create;
using FinancialLedger.Application.UseCases.Account.CreateEntriesByAccountId;
using FinancialLedger.Application.UseCases.Account.GetById;
using FinancialLedger.Communication.Requests;
using FinancialLedger.Communication.Response;
using Microsoft.AspNetCore.Mvc;

namespace FinancialLedger.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase {
  [HttpPost]
  [ProducesResponseType<ResponseAccount>(StatusCodes.Status201Created)]
  [ProducesResponseType<ResponseError>(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> CreateAccount([FromServices] ICreateAccountUseCase useCase) {
    var response = await useCase.Execute();
    return Created($"/api/account/{response.Id}", response);
  }

  [HttpGet]
  [Route("{id}")]
  [ProducesResponseType<ResponseAccount>(StatusCodes.Status200OK)]
  [ProducesResponseType<ResponseError>(StatusCodes.Status404NotFound)]
  [ProducesResponseType<ResponseError>(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> GetAccountById([FromServices] IGetAccountByIdUseCase useCase, [FromRoute] long id) {
    var response = await useCase.Execute(id);
    return Ok(response);
  }

  [HttpPost]
  [Route("{accountId}/entries")]
  [ProducesResponseType<ResponseAccount>(StatusCodes.Status201Created)]
  [ProducesResponseType<ResponseError>(StatusCodes.Status400BadRequest)]
  [ProducesResponseType<ResponseError>(StatusCodes.Status409Conflict)]
  [ProducesResponseType<ResponseError>(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> CreateAccountEntry([FromServices] ICreateEntryByAccountIdUseCase useCase, [FromRoute] long accountId, [FromBody] RequestCreateEntry request) {
    var response = await useCase.Execute(accountId, request);
    return Created($"/api/account/entries/${response.Id}", response);
  }
}
