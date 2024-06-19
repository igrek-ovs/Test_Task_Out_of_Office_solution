using Microsoft.AspNetCore.Mvc;
using Test_Task_Out_of_Office_solution.dto_s;
using Test_Task_Out_of_Office_solution.services.interfaces;

namespace Test_Task_Out_of_Office_solution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees([FromQuery] EmployeeFilterDTO filterDTO)
        {
            var employees = await _employeeService.GetEmployeesByFilter(filterDTO);
            return Ok(employees);
        }

        [HttpGet("get-hrs")]
        public async Task<IActionResult> GetHRs()
        {
            var employees = await _employeeService.GetHRManagers();
            return Ok(employees);
        }

        [HttpGet("get-project-managers")]
        public async Task<IActionResult> GetProjectManagers()
        {
            var prs = await _employeeService.GetProjectManagers();
            return Ok(prs);
        }

        [HttpGet("get-user-role")]
        public async Task<IActionResult> GetUserRole(string fullName, string position)
        {
            var userRole = await _employeeService.GetUserRole(fullName, position);
            return Ok(userRole);
        }

        [HttpGet("get-employee-by-id/{id}")]
        public async Task<IActionResult> GetEmployeeById(int employeeId)
        {
            var employee = await _employeeService.GetEmployeeById(employeeId);
            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeDTO employeeDTO)
        {
            await _employeeService.AddEmployee(employeeDTO);
            return Ok();
        }

        [HttpPost("assign-to-project")]
        public async Task<IActionResult> AssignEmployeeToProject(int employeeId, int projectId)
        {
            var response = await _employeeService.AssignEmployeeToProject(employeeId, projectId);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeDTO employeeDTO)
        {
            var result = await _employeeService.UpdateEmployee(employeeDTO);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }
        
        [HttpPost("upload-photo/{employeeId}")]
        public async Task<IActionResult> UploadPhoto(int employeeId, [FromForm] IFormFile photo)
        {
            var result = await _employeeService.UploadPhotoAsync(employeeId, photo);
            if (result)
            {
                return Ok();
            }

            return BadRequest("Unable to upload photo");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var result = await _employeeService.DeleteEmployee(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpPost("toggle-deactivate/{id}")]
        public async Task<IActionResult> ToggleDeactivateEmployee(int id)
        {
            var result = await _employeeService.ToggleDeactivateEmployee(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
