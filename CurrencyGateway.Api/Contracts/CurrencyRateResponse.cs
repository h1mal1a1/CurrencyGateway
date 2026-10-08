namespace CurrencyGateway.Api.Contracts;

public class CurrencyRateResponse
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int Nominal { get; set; }
    public decimal Value { get; set; }
    public DateOnly Date { get; set; }
}