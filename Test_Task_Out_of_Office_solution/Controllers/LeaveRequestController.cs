using Microsoft.AspNetCore.Mvc;
using Test_Task_Out_of_Office_solution.dto_s;
using Test_Task_Out_of_Office_solution.services.interfaces;

namespace Test_Task_Out_of_Office_solution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveRequestController : ControllerBase
    {
        private readonly ILeaveRequestService _leaveRequestService;

        public LeaveRequestController(ILeaveRequestService leaveRequestService)
        {
            _leaveRequestService = leaveRequestService;
        }

        [HttpGet]
        public async Task<ActionResult<List<LeaveRequestDTO>>> GetLeaveRequests([FromQuery] LeaveRequestFilterDTO filter)
        {
            var leaveRequests = await _leaveRequestService.GetLeaveRequestsByFilter(filter);
            return Ok(leaveRequests);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveRequestDTO>> GetLeaveRequestById(int id)
        {
            var leaveRequest = await _leaveRequestService.GetLeaveRequestById(id);
            if (leaveRequest == null)
            {
                return NotFound();
            }
            return Ok(leaveRequest);
        }

        [HttpPost]
        public async Task<IActionResult> AddLeaveRequest([FromBody] LeaveRequestDTO leaveRequestDTO)
        {
            var result = await _leaveRequestService.AddLeaveRequest(leaveRequestDTO);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLeaveRequest(int id, [FromBody] LeaveRequestDTO leaveRequestDTO)
        {
            if (id != leaveRequestDTO.Id)
            {
                return BadRequest();
            }

            var result = await _leaveRequestService.UpdateLeaveRequest(leaveRequestDTO);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeaveRequest(int id)
        {
            var result = await _leaveRequestService.DeleteLeaveRequest(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }

}
