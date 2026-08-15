using System.ComponentModel.DataAnnotations;
using Entity.Base;

namespace Entity.Pricing;

public class ExchangeRate  : BaseEntity
{
    public string CurrencyCode { get; set; } = null!;
    public decimal BuyingRate { get; set; }
    public decimal SellingRate { get; set; }

    public DateOnly Date { get; set; }
}
