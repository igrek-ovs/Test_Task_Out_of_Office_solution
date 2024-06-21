using Test_Task_Out_of_Office_solution.dto_s;
using Test_Task_Out_of_Office_solution.models;

namespace Test_Task_Out_of_Office_solution.services.interfaces;

public interface ILeaveRequestService
{
    Task<List<LeaveRequestDTO>> GetLeaveRequestsByFilter(LeaveRequestFilterDTO filterDTO);
    Task<LeaveRequestDTO> GetLeaveRequestById(int id);
    Task<LeaveRequest> AddLeaveRequest(LeaveRequestDTO leaveRequestDTO);
    Task<bool> UpdateLeaveRequest(LeaveRequestDTO leaveRequestDTO);
    Task<bool> DeleteLeaveRequest(int id);
    Task<bool> SubmitLeaveRequest(int id);
    Task<bool> CancelLeaveRequest(int id);
}