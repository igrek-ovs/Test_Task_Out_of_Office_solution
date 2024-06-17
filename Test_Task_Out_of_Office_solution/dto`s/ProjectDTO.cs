namespace Test_Task_Out_of_Office_solution.dto_s
{
    public class ProjectDTO
    {
        public int? Id { get; set; }
        public string ProjectType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ProjectManagerId { get; set; }
        public string? ProjectManagerName { get; set; }
        public string? Comment { get; set; }
        public bool Status { get; set; }
    }
}
