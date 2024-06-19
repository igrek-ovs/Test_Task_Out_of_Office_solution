using Test_Task_Out_of_Office_solution.dto_s;
using Test_Task_Out_of_Office_solution.models;

namespace Test_Task_Out_of_Office_solution.services.interfaces
{
    public interface IEmployeeService
    {
        Task AddEmployee(EmployeeDTO employeeDTO);
        Task<bool> DeleteEmployee(int employeeId);
        Task<bool> UpdateEmployee(EmployeeDTO employeeDTO);
        Task<bool> ToggleDeactivateEmployee(int employeeId);
        Task<List<EmployeeDTO>> GetEmployeesByFilter(EmployeeFilterDTO filterDTO);
        Task<List<EmployeeDTO>> GetHRManagers();
        Task<List<EmployeeDTO>> GetProjectManagers();
        Task<Employee> GetUserRole(string fullName, string position);
        Task<bool> AssignEmployeeToProject(int employeeId, int projectId);
        Task<EmployeeDTO> GetEmployeeById(int employeeId);
        Task<bool> UploadPhotoAsync(int employeeId, IFormFile photo);
    }
}