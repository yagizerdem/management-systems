using Entity.Base;
using Entity.Identity;
using Entity.Reservations;

namespace Entity.Customers;

public class Customer : AppUser
{

    public string? NationalIdentityNumber { get; set; }
    
    public string? PassportNumber { get; set; }
    
    public DateOnly? BirthDate { get; set; }
    public string? Address { get; set; }

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
