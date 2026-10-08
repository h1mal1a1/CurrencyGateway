using System.Xml.Serialization;

namespace CurrencyGateway.Infrastructure.Cbr.Models;

[XmlRoot("ValCurs")]
public class CbrResponse
{
    [XmlAttribute("Date")]
    public string Date { get; set; } = null!;

    [XmlElement("Valute")]
    public List<CbrCurrency> Currencies { get; set; } = [];
}