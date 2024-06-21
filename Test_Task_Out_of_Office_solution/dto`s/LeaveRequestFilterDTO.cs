namespace Test_Task_Out_of_Office_solution.dto_s;

public class LeaveRequestFilterDTO
{
    public string? SortBy { get; set; }
    public bool? SortAscending { get; set; }
    public string? AbsenceReason { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Status { get; set; }
    public int? RequestNumber { get; set; }
    public int? EmployeeId { get; set; }
}