using BilgeHotel.Domain.Rooms;
using System.ComponentModel.DataAnnotations;

namespace BilgeHotel.Domain.Pricing;

public class RoomRate
{
    [Key]
    public Guid Id { get; set; }

    public int RoomTypeId { get; set; }
    public RoomType RoomType { get; set; } = null!;

    public int PackageId { get; set; }
    public Package Package { get; set; } = null!;

    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }

    public decimal PricePerNight { get; set; }
    public string CurrencyCode { get; set; } = "TRY";
}
