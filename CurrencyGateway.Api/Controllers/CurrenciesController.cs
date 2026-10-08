using CurrencyGateway.Api.Contracts;
using CurrencyGateway.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyGateway.Api.Controllers;

[ApiController]
[Route("api/currencies")]
public class CurrenciesController(ICurrencyService currencyService) : ControllerBase
{
    private readonly ICurrencyService _currencyService = currencyService;

    [HttpGet]
    [ProducesResponseType(typeof(CurrencyRateResponse[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync([FromQuery] DateOnly? date, [FromQuery] string? code,
        CancellationToken cancellationToken)
    {
        if (date.HasValue && date.Value > DateOnly.FromDateTime(DateTime.Today))
            return BadRequest("Date cannot be in the future.");

        code = code?.Trim().ToUpperInvariant();

        if (!string.IsNullOrWhiteSpace(code) && code.Length != 3)
            return BadRequest("Currency code must contain 3 characters.");

        var rates = await _currencyService.GetRatesAsync(date, code, cancellationToken);

        if (!string.IsNullOrWhiteSpace(code) && rates.Count == 0)
            return NoContent();

        var response = rates
            .Select(x => new CurrencyRateResponse
            {
                Code = x.Code,
                Name = x.Name,
                Nominal = x.Nominal,
                Value = x.Value,
                Date = x.Date
            })
            .ToArray();

        return Ok(response);
    }
}