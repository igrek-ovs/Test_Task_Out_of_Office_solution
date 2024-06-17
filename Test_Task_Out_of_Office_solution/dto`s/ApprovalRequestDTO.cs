using System.ComponentModel;
using Test_Task_Out_of_Office_solution.models;

namespace Test_Task_Out_of_Office_solution.dto_s
{
    public class ApprovalRequestDTO
    {
        public int? Id { get; set; }
        public int ApproverId { get; set; }
        public string? ApproverName { get; set; }
        public int LeaveRequestId { get; set; }
        public string? LeaveRequestDetails { get; set; }
        public string Status { get; set; }
        public string? Comment { get; set; }
        public string? EmployeeName { get; set; }
    }
}
