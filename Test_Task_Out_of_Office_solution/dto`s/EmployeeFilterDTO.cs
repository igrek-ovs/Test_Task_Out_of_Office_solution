namespace Test_Task_Out_of_Office_solution.dto_s
{
    public class EmployeeFilterDTO
    {
        public string? SortBy { get; set; }
        public bool? SortAscending { get; set; }
        public string? Subdivision { get; set; }
        public bool? IsActive { get; set; }
        public int? OutOfOfficeBalanceLeft { get; set; }
        public int? OutOfOfficeBalanceRight { get; set; }
        public string? SearchByName { get; set; }
    }
}
