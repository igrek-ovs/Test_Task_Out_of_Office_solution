namespace Test_Task_Out_of_Office_solution.dto_s
{
    public class LeaveRequestFilterDTO
    {
        public string? SortBy { get; set; } // "employeeName", "startDate", "endDate", "status"
        public bool? SortAscending { get; set; } // true for ascending, false for descending
        public string? AbsenceReason { get; set; } // filter by absence reason
        public DateTime? StartDate { get; set; } // filter by start date
        public DateTime? EndDate { get; set; } // filter by end date
        public string? Status { get; set; } // filter by status
        public int? RequestNumber { get; set; } // search by request number (ID)
    }
}
