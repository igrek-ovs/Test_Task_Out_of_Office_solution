namespace Test_Task_Out_of_Office_solution.dto_s;

public class LeaveRequestDTO
{
    public int? Id { get; set; }
    public int EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public string AbsenceReason { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Comment { get; set; }
    public string? Status { get; set; }
    public int? ApprovalRequestId { get; set; }
    public string? ApprovalStatus { get; set; }
}