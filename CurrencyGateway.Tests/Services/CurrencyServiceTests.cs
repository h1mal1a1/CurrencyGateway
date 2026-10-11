using CurrencyGateway.Application.Interfaces;
using CurrencyGateway.Application.Services;
using CurrencyGateway.Domain.Models;
using Moq;

namespace CurrencyGateway.Tests.Services;

public class CurrencyServiceTests
{
    [Fact]
    public async Task GetRatesAsync_WhenCodeIsNull_ReturnsAllRates()
    {
        var rates = new[]
        {
            new CurrencyRate
            {
                Code = "USD",
                Name = "Доллар США",
                Nominal = 1,
                Value = 80m,
                Date = new DateOnly(2026, 10, 8)
            },
            new CurrencyRate
            {
                Code = "EUR",
                Name = "Евро",
                Nominal = 1,
                Value = 90m,
                Date = new DateOnly(2026, 10, 8)
            }
        };

        var cbrClientMock = new Mock<ICbrClient>();

        cbrClientMock
            .Setup(x => x.GetRatesAsync(It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(rates);

        var service = new CurrencyService(cbrClientMock.Object);

        var result = await service.GetRatesAsync(new DateOnly(2026, 10, 8), null);

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetRatesAsync_WhenCodeSpecified_ReturnsOnlyRequestedCurrency()
    {
        var rates = new[]
        {
            new CurrencyRate
            {
                Code = "USD",
                Name = "Доллар США",
                Nominal = 1,
                Value = 80m,
                Date = new DateOnly(2026, 10, 8)
            },
            new CurrencyRate
            {
                Code = "EUR",
                Name = "Евро",
                Nominal = 1,
                Value = 90m,
                Date = new DateOnly(2026, 10, 8)
            }
        };

        var cbrClientMock = new Mock<ICbrClient>();

        cbrClientMock
            .Setup(x => x.GetRatesAsync(It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(rates);

        var service = new CurrencyService(cbrClientMock.Object);
        var result = await service.GetRatesAsync(new DateOnly(2026, 10, 8), "USD");
        var currency = Assert.Single(result);

        Assert.Equal("USD", currency.Code);
    }

    [Fact]
    public async Task GetRatesAsync_WhenCodeHasDifferentCase_ReturnsCurrency()
    {
        var rates = new[]
        {
            new CurrencyRate
            {
                Code = "USD",
                Name = "Доллар США",
                Nominal = 1,
                Value = 80m,
                Date = new DateOnly(2026, 10, 8)
            }
        };

        var cbrClientMock = new Mock<ICbrClient>();

        cbrClientMock
            .Setup(x => x.GetRatesAsync(It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(rates);

        var service = new CurrencyService(cbrClientMock.Object);
        var result = await service.GetRatesAsync(new DateOnly(2026, 10, 8), "usd");
        var currency = Assert.Single(result);

        Assert.Equal("USD", currency.Code);
    }

    [Fact]
    public async Task GetRatesAsync_WhenCurrencyNotFound_ReturnsEmptyCollection()
    {
        var rates = new[]
        {
            new CurrencyRate
            {
                Code = "USD",
                Name = "Доллар США",
                Nominal = 1,
                Value = 80m,
                Date = new DateOnly(2026, 10, 8)
            }
        };

        var cbrClientMock = new Mock<ICbrClient>();

        cbrClientMock
            .Setup(x => x.GetRatesAsync(It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(rates);

        var service = new CurrencyService(cbrClientMock.Object);

        var result = await service.GetRatesAsync(new DateOnly(2026, 10, 8), "EUR");

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetRatesAsync_WhenDateSpecified_PassesDateToCbrClient()
    {
        var expectedDate = new DateOnly(2026, 10, 8);

        var cbrClientMock = new Mock<ICbrClient>();

        cbrClientMock
            .Setup(x => x.GetRatesAsync(expectedDate, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Array.Empty<CurrencyRate>());

        var service = new CurrencyService(cbrClientMock.Object);

        await service.GetRatesAsync(expectedDate, null);

        cbrClientMock.Verify(x => x.GetRatesAsync(expectedDate, It.IsAny<CancellationToken>()), Times.Once);
    }
}