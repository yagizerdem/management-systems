using Entity.Rooms.@enum;
using System.ComponentModel.DataAnnotations;

namespace BilgeHotel.Domain.Rooms;

public class RoomBlock
{
    [Key]
    public Guid Id { get; set; }

    public int RoomId { get; set; }
    public Room Room { get; set; } = null!;

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public string Reason { get; set; } = null!;
    public RoomBlockType Type { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
