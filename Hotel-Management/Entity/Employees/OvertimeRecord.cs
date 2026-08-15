using System.ComponentModel.DataAnnotations;
using Entity.Base;

namespace Entity.Employees;

public class OvertimeRecord : BaseEntity
{
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public string? Reason { get; set; }
}
