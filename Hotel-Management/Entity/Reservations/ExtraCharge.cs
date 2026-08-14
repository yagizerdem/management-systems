namespace BilgeHotel.Domain.Reservations;

public class ExtraCharge
{
    public long Id { get; set; }

    public long ReservationId { get; set; }
    public Reservation Reservation { get; set; } = null!;

    public string Description { get; set; } = null!;
    public decimal Amount { get; set; }
    public int Quantity { get; set; } = 1;

    public DateTime ChargedAt { get; set; } = DateTime.UtcNow;
}
