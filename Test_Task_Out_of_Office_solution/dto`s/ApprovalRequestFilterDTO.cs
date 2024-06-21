namespace Test_Task_Out_of_Office_solution.dto_s;

public class ApprovalRequestFilterDTO
{
    public string? SortBy { get; set; }
    public bool? SortAscending { get; set; }
    public string? Status { get; set; }
    public int? RequestNumber { get; set; }
    public string? SearchByFullName { get; set; }
}