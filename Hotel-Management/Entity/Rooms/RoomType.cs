using Entity.Base;

namespace Entity.Rooms;

public class RoomType : BaseEntity
{

    public string Name { get; set; } = null!;
    public int Capacity { get; set; }
    public int SingleBedCount { get; set; }
    public int DoubleBedCount { get; set; }
    public string? Description { get; set; }

    public ICollection<Room> Rooms { get; set; } = new List<Room>();
}
