using System.ComponentModel.DataAnnotations;

namespace BilgeHotel.Domain.Rooms;

public class Amenity
{
    [Key]
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<RoomAmenity> Rooms { get; set; } = new List<RoomAmenity>();
}
