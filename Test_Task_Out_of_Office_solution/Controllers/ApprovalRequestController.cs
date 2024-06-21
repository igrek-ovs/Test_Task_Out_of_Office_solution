using Microsoft.AspNetCore.Mvc;
using Test_Task_Out_of_Office_solution.dto_s;
using Test_Task_Out_of_Office_solution.services.interfaces;

namespace Test_Task_Out_of_Office_solution.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApprovalRequestController : ControllerBase
{
    private readonly IApprovalRequestService _approvalRequestService;

    public ApprovalRequestController(IApprovalRequestService approvalRequestService)
    {
        _approvalRequestService = approvalRequestService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ApprovalRequestDTO>>> GetApprovalRequests(
        [FromQuery] ApprovalRequestFilterDTO filter)
    {
        var approvalRequests = await _approvalRequestService.GetApprovalRequestsByFilter(filter);
        return Ok(approvalRequests);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApprovalRequestDTO>> GetApprovalRequestById(int id)
    {
        var approvalRequest = await _approvalRequestService.GetApprovalRequestById(id);
        if (approvalRequest == null) return NotFound();
        return Ok(approvalRequest);
    }

    [HttpPost("approve/{id}")]
    public async Task<IActionResult> ApproveRequest(int id)
    {
        var result = await _approvalRequestService.ApproveRequest(id);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpPost("reject/{id}")]
    public async Task<IActionResult> RejectRequest(int id, [FromBody] string comment)
    {
        var result = await _approvalRequestService.RejectRequest(id, comment);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteApprovalRequest(int id)
    {
        var result = await _approvalRequestService.DeleteApprovalRequest(id);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateApprovalRequest(int id, [FromBody] ApprovalRequestDTO approvalRequestDTO)
    {
        if (id != approvalRequestDTO.Id) return BadRequest();

        var result = await _approvalRequestService.UpdateApprovalRequest(approvalRequestDTO);
        if (!result) return NotFound();
        return NoContent();
    }
}