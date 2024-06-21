namespace Test_Task_Out_of_Office_solution.dto_s;

public class ProjectFilterDTO
{
    public string? SortBy { get; set; }
    public bool? SortAscending { get; set; }
    public string? ProjectType { get; set; }
    public DateTime? StartDateFrom { get; set; }
    public DateTime? StartDateTo { get; set; }
    public bool? Status { get; set; }
    public int? ProjectNumber { get; set; }
    public int? AssignedEmployeeId { get; set; }
}