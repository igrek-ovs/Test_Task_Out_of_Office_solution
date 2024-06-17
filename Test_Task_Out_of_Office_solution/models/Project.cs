using System.ComponentModel;

namespace Test_Task_Out_of_Office_solution.models
{
    public class Project
    {
        public int Id { get; set; }
        public string ProjectType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ProjectManagerId { get; set; }
        public Employee ProjectManager { get; set; }
        public string? Comment { get; set; }

        [ReadOnly(true)] public bool Status { get; set; } = true;
        //public ICollection<Employee> Employees { get; set; }
    }
}
