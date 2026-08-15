using System;
using System.ComponentModel.DataAnnotations;

namespace Entity.Base;

public abstract class BaseEntity : IBaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public EntityStatus EntityStatus { get; set; } = EntityStatus.ACTIVE;
}