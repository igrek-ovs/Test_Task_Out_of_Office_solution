namespace Test_Task_Out_of_Office_solution.dto_s;

public class EmployeeDTO
{
    public int? Id { get; set; }
    public string FullName { get; set; }
    public string Subdivision { get; set; }
    public string Position { get; set; }
    public int? PeoplePartnerId { get; set; }
    public byte[]? Photo { get; set; }
    public bool? IsActive { get; set; }
    public int? OutOfOfficeBalance { get; set; }
}