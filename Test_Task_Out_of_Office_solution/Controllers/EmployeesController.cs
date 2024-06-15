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

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeDTO employeeDTO)
        {
            await _employeeService.AddEmployee(employeeDTO);
            return Ok();
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
