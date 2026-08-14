using BilgeHotel.Domain.Reservations;

namespace BilgeHotel.Domain.Customers;

public class Customer
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public string? NationalIdentityNumber { get; set; }
    public string? PassportNumber { get; set; }

    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string? Address { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
