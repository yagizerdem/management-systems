using BilgeHotel.Domain.Pricing;
using Entity.Base;

namespace Entity.Pricing;

public class Package : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<RoomRate> RoomRates { get; set; } = new List<RoomRate>();
}
