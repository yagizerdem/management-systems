using System.ComponentModel.DataAnnotations;
using Entity.Base;

namespace Entity.Rooms;

public class RoomAmenity : BaseEntity
{
    public Guid RoomId { get; set; }
    public Room Room { get; set; } = null!;

    public Guid AmenityId { get; set; }
    public Amenity Amenity { get; set; } = null!;
}
