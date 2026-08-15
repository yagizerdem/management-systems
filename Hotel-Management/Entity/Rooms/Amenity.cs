using System.ComponentModel.DataAnnotations;
using Entity.Base;

namespace Entity.Rooms;

public class Amenity : BaseEntity
{

    public string Name { get; set; } = null!;

    public ICollection<RoomAmenity> Rooms { get; set; } = new List<RoomAmenity>();
}
