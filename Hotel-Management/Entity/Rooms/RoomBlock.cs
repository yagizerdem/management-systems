using Entity.Rooms.@enum;
using System.ComponentModel.DataAnnotations;
using Entity.Base;

namespace Entity.Rooms;

public class RoomBlock : BaseEntity
{

    public int RoomId { get; set; }
    public Room Room { get; set; } = null!;

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public string Reason { get; set; } = null!;
    public RoomBlockType Type { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
