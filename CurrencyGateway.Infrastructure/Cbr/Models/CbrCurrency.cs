using System.Xml.Serialization;

namespace CurrencyGateway.Infrastructure.Cbr.Models;

public class CbrCurrency
{
    [XmlElement("CharCode")]
    public string CharCode { get; set; } = null!;

    [XmlElement("Nominal")]
    public int Nominal { get; set; }

    [XmlElement("Name")]
    public string Name { get; set; } = null!;

    [XmlElement("Value")]
    public string Value { get; set; } = null!;
}