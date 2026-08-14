using System.ComponentModel.DataAnnotations;

namespace BilgeHotel.Domain.Pricing;

public class ExchangeRate
{
    [Key]
    public Guid Id { get; set; }

    public string CurrencyCode { get; set; } = null!;
    public decimal BuyingRate { get; set; }
    public decimal SellingRate { get; set; }

    public DateOnly Date { get; set; }
}
