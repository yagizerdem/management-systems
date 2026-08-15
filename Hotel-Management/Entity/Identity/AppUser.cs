using Microsoft.AspNetCore.Identity;

namespace Entity.Identity;

public class AppUser : IdentityUser<Guid>
{

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public DateTime LastLogin = DateTime.UtcNow;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}
