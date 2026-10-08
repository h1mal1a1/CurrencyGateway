using System.Globalization;
using System.Xml.Serialization;
using CurrencyGateway.Application.Exceptions;
using CurrencyGateway.Application.Interfaces;
using CurrencyGateway.Domain.Models;
using CurrencyGateway.Infrastructure.Cbr.Models;

namespace CurrencyGateway.Infrastructure.Cbr;

public class CbrClient(HttpClient httpClient) : ICbrClient
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<IReadOnlyCollection<CurrencyRate>> GetRatesAsync(DateOnly date, CancellationToken cancellationToken = default)
    {
        try
        {
            var requestUri = $"scripts/XML_daily.asp?date_req={date:dd/MM/yyyy}";

            await using var stream = await _httpClient.GetStreamAsync(requestUri, cancellationToken);

            var serializer = new XmlSerializer(typeof(CbrResponse));

            var response = (CbrResponse?)serializer.Deserialize(stream);

            if (response is null)
                return Array.Empty<CurrencyRate>();

            return response.Currencies
                .Select(x => new CurrencyRate
                {
                    Code = x.CharCode,
                    Name = x.Name,
                    Nominal = x.Nominal,
                    Value = decimal.Parse(
                        x.Value,
                        NumberStyles.Number,
                        CultureInfo.GetCultureInfo("ru-RU")),
                    Date = date
                })
                .ToArray();
        }
        catch (Exception ex) when (ex is HttpRequestException or InvalidOperationException or FormatException)
        {
            throw new ExternalServiceException("Failed to receive currency rates from the Bank of Russia.", ex);
        }
    }
}