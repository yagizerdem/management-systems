using System.ComponentModel.DataAnnotations;

namespace BilgeHotel.Domain.Rooms;

public class RoomAmenity
{
    [Key]
    public Guid RoomId { get; set; }
    public Room Room { get; set; } = null!;

    public int AmenityId { get; set; }
    public Amenity Amenity { get; set; } = null!;
}
