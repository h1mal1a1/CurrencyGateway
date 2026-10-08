using CurrencyGateway.Domain.Models;

namespace CurrencyGateway.Application.Interfaces;

public interface ICbrClient
{
    Task<IReadOnlyCollection<CurrencyRate>> GetRatesAsync(DateOnly date, CancellationToken cancellationToken = default);
}