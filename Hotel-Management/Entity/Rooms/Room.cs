using BilgeHotel.Domain.Reservations;
using System.ComponentModel.DataAnnotations;

namespace BilgeHotel.Domain.Rooms;

public class Room
{
    [Key]
    public Guid Id { get; set; }

    public string RoomNumber { get; set; } = null!;
    public int Floor { get; set; }

    public int RoomTypeId { get; set; }
    public RoomType RoomType { get; set; } = null!;

    public bool IsActive { get; set; } = true;

    public ICollection<RoomAmenity> Amenities { get; set; } = new List<RoomAmenity>();
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    public ICollection<RoomBlock> Blocks { get; set; } = new List<RoomBlock>();
}
