using System.Text.Json.Serialization;

namespace Test_Task_Out_of_Office_solution.models;

public class LeaveRequest
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }

    [JsonIgnore] public Employee Employee { get; set; }

    public string AbsenceReason { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Comment { get; set; }
    public string Status { get; set; } = "New";
    public ApprovalRequest? ApprovalRequest { get; set; }
}