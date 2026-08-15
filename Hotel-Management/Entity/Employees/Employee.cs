using Entity.Employees.@enum;
using Entity.Identity;

namespace Entity.Employees;

public class Employee : AppUser
{
    public string? Address { get; set; }

    public int DepartmentId { get; set; }
    public Department Department { get; set; } = null!;

    public SalaryType SalaryType { get; set; }

    public decimal? HourlyRate { get; set; }
    public decimal? MonthlySalary { get; set; }

    public DateOnly HireDate { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<EmployeeShift> Shifts { get; set; } = new List<EmployeeShift>();
    public ICollection<OvertimeRecord> OvertimeRecords { get; set; } = new List<OvertimeRecord>();
}
