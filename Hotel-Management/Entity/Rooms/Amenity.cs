namespace BilgeHotel.Domain.Rooms;

public class Amenity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<RoomAmenity> Rooms { get; set; } = new List<RoomAmenity>();
}
