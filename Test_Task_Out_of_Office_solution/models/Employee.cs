namespace Test_Task_Out_of_Office_solution.models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Subdivision { get; set; }
        public string Position { get; set; }
        public bool IsActive { get; set; } = true;
        public int? PeoplePartnerId { get; set; }
        public Employee? PeoplePartner { get; set; }
        public int? OutOfOfficeBalance { get; set; }
        public byte[]? Photo { get; set; }
        public int? ProjectId { get; set; }
        public Project? Project { get; set; }

        public ICollection<LeaveRequest>? LeaveRequests { get; set; }
        public ICollection<ApprovalRequest>? ApprovalRequests { get; set; }
        public ICollection<Project>? Projects { get; set; }
    }
}
