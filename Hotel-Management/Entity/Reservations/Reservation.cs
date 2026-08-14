using BilgeHotel.Domain.Customers;
using BilgeHotel.Domain.Pricing;
using BilgeHotel.Domain.Rooms;
using System.ComponentModel.DataAnnotations;

namespace BilgeHotel.Domain.Reservations;

public class Reservation
{
    [Key]
    public Guid Id { get; set; }

    public string ReservationNumber { get; set; } = null!;

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public int RoomId { get; set; }
    public Room Room { get; set; } = null!;

    public int PackageId { get; set; }
    public Package Package { get; set; } = null!;

    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }

    public ReservationStatus Status { get; set; }
    public ReservationChannel Channel { get; set; }

    public decimal BasePrice { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalPrice { get; set; }

    public string CurrencyCode { get; set; } = "TRY";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CheckedInAt { get; set; }
    public DateTime? CheckedOutAt { get; set; }

    public ICollection<ExtraCharge> ExtraCharges { get; set; } = new List<ExtraCharge>();
}
