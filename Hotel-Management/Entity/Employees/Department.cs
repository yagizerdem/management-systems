using System.ComponentModel.DataAnnotations;

namespace BilgeHotel.Domain.Employees;

public class Department
{
    [Key]
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
