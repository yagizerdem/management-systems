namespace BilgeHotel.Domain.Employees;

public class OvertimeRecord
{
    public long Id { get; set; }

    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public string? Reason { get; set; }
}
