using CurrencyGateway.Domain.Models;

namespace CurrencyGateway.Application.Interfaces;

public interface ICurrencyService
{
    Task<IReadOnlyCollection<CurrencyRate>> GetRatesAsync(DateOnly? date, string? code,
        CancellationToken cancellationToken = default);
}