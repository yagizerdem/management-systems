namespace BilgeHotel.Domain.Pricing;

public class ExchangeRate
{
    public long Id { get; set; }

    public string CurrencyCode { get; set; } = null!;
    public decimal BuyingRate { get; set; }
    public decimal SellingRate { get; set; }

    public DateOnly Date { get; set; }
}
