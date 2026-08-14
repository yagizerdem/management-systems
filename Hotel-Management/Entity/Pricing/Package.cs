namespace BilgeHotel.Domain.Pricing;

public class Package
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<RoomRate> RoomRates { get; set; } = new List<RoomRate>();
}
