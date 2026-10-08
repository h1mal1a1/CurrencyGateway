using CurrencyGateway.Application.Interfaces;
using CurrencyGateway.Domain.Models;

namespace CurrencyGateway.Application.Services;

public class CurrencyService(ICbrClient cbrClient) : ICurrencyService
{
    private readonly ICbrClient _cbrClient = cbrClient;

    public async Task<IReadOnlyCollection<CurrencyRate>> GetRatesAsync(DateOnly? date, string? code,
        CancellationToken cancellationToken = default)
    {
        var targetDate = date ?? DateOnly.FromDateTime(DateTime.Today);

        var rates = await _cbrClient.GetRatesAsync(targetDate, cancellationToken);

        if (string.IsNullOrWhiteSpace(code))
            return rates;

        return rates
            .Where(x => string.Equals(x.Code, code, StringComparison.OrdinalIgnoreCase))
            .ToArray();
    }
}