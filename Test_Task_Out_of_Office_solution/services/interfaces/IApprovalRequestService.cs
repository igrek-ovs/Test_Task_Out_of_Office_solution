using Test_Task_Out_of_Office_solution.dto_s;

namespace Test_Task_Out_of_Office_solution.services.interfaces
{
    public interface IApprovalRequestService
    {
        Task<List<ApprovalRequestDTO>> GetApprovalRequestsByFilter(ApprovalRequestFilterDTO filterDTO);
        Task<ApprovalRequestDTO> GetApprovalRequestById(int id);
        Task<bool> ApproveRequest(int id);
        Task<bool> RejectRequest(int id, string comment);
        Task<bool> DeleteApprovalRequest(int id);  // New method for deleting a request
        Task<bool> UpdateApprovalRequest(ApprovalRequestDTO approvalRequestDTO);  // New method for updating a request
    }
}
