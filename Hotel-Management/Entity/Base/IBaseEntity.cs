using System.ComponentModel.DataAnnotations;

namespace Entity.Base
{

    public interface IBaseEntity
    {
        [Key]
        Guid Id { get; set; }

        DateTime CreatedAt { get; set; }

        DateTime? UpdatedAt { get; set; }

        EntityStatus EntityStatus { get; set; }
    }
}
