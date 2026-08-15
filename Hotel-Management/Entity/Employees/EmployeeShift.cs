using System.ComponentModel.DataAnnotations;
using Entity.Base;
using Entity.Employees.@enum;

namespace Entity.Employees;

public class EmployeeShift : BaseEntity
{

    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public ShiftStatus Status { get; set; }
}
