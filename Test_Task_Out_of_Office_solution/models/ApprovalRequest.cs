using System.ComponentModel;

namespace Test_Task_Out_of_Office_solution.models
{
    public class ApprovalRequest
    {
        public int Id { get; set; }
        public int ApproverId { get; set; }
        public Employee Approver { get; set; }
        public int LeaveRequestId { get; set; }
        public LeaveRequest LeaveRequest { get; set; }
        [ReadOnly(true)]
        public string Status { get; set; } = "New";
        public string? Comment { get; set; }
    }
}
