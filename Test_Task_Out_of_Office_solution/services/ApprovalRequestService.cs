using Test_Task_Out_of_Office_solution.data;
using Test_Task_Out_of_Office_solution.dto_s;
using Test_Task_Out_of_Office_solution.services.interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.IdentityModel.Tokens;
using Test_Task_Out_of_Office_solution.models;

namespace Test_Task_Out_of_Office_solution.services
{
    public class ApprovalRequestService : IApprovalRequestService
    {
        private readonly TaskContext _context;

        public ApprovalRequestService(TaskContext context)
        {
            _context = context;
        }

        public async Task<List<ApprovalRequestDTO>> GetApprovalRequestsByFilter(ApprovalRequestFilterDTO filterDTO)
        {
            var query = _context.ApprovalRequests.Include(ar => ar.Approver).Include(ar => ar.LeaveRequest).AsQueryable();

            if (!string.IsNullOrEmpty(filterDTO.Status))
            {
                query = query.Where(ar => ar.Status == filterDTO.Status);
            }

            if (filterDTO.RequestNumber.HasValue)
            {
                query = query.Where(ar => ar.Id == filterDTO.RequestNumber.Value);
            }

            if (!filterDTO.SearchByFullName.IsNullOrEmpty())
            {
                query = query.Where(ar => ar.LeaveRequest.Employee.FullName.Contains(filterDTO.SearchByFullName));
            }

            if (!string.IsNullOrEmpty(filterDTO.SortBy))
            {
                if (filterDTO.SortBy.Equals("ApproverName", StringComparison.OrdinalIgnoreCase))
                {
                    query = filterDTO.SortAscending.GetValueOrDefault() ?
                        query.OrderBy(ar => ar.Approver.FullName) :
                        query.OrderByDescending(ar => ar.Approver.FullName);
                }
                else
                {
                    Expression<Func<ApprovalRequest, object>> sortExpression = filterDTO.SortBy switch
                    {
                        "LeaveRequestId" => ar => ar.LeaveRequestId,
                        "Status" => ar => ar.Status,
                        _ => ar => ar.Id,
                    };

                    query = filterDTO.SortAscending.GetValueOrDefault() ?
                        query.OrderBy(sortExpression) :
                        query.OrderByDescending(sortExpression);
                }
            }

            var approvalRequests = await query.ToListAsync();

            return approvalRequests.Select(ar => new ApprovalRequestDTO
            {
                Id = ar.Id,
                ApproverId = ar.ApproverId,
                ApproverName = ar.Approver.FullName,
                LeaveRequestId = ar.LeaveRequestId,
                LeaveRequestDetails = $"From {ar.LeaveRequest.StartDate.ToShortDateString()} to {ar.LeaveRequest.EndDate.ToShortDateString()}",
                Status = ar.Status,
                Comment = ar.Comment,
                EmployeeName = ar.LeaveRequest.Employee.FullName,
            }).ToList();
        }

        public async Task<ApprovalRequestDTO> GetApprovalRequestById(int id)
        {
            var approvalRequest = await _context.ApprovalRequests
                                                .Include(ar => ar.Approver)
                                                .Include(ar => ar.LeaveRequest)
                                                .FirstOrDefaultAsync(ar => ar.Id == id);

            if (approvalRequest == null) return null;

            return new ApprovalRequestDTO
            {
                Id = approvalRequest.Id,
                ApproverId = approvalRequest.ApproverId,
                ApproverName = approvalRequest.Approver.FullName,
                LeaveRequestId = approvalRequest.LeaveRequestId,
                LeaveRequestDetails = $"From {approvalRequest.LeaveRequest.StartDate.ToShortDateString()} to {approvalRequest.LeaveRequest.EndDate.ToShortDateString()}",
                Status = approvalRequest.Status,
                Comment = approvalRequest.Comment
            };
        }

        public async Task<bool> ApproveRequest(int id)
        {
            var approvalRequest = await _context.ApprovalRequests
                                                .Include(ar => ar.LeaveRequest)
                                                .FirstOrDefaultAsync(ar => ar.Id == id);

            if (approvalRequest == null) return false;

            approvalRequest.Status = "Approved";
            approvalRequest.LeaveRequest.Status = "Approved";
            _context.ApprovalRequests.Update(approvalRequest);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RejectRequest(int id, string comment)
        {
            var approvalRequest = await _context.ApprovalRequests
                                                .Include(ar => ar.LeaveRequest)
                                                .FirstOrDefaultAsync(ar => ar.Id == id);

            if (approvalRequest == null) return false;

            approvalRequest.Status = "Rejected";
            approvalRequest.LeaveRequest.Status = "Rejected";
            approvalRequest.Comment = comment;
            _context.ApprovalRequests.Update(approvalRequest);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteApprovalRequest(int id)
        {
            var approvalRequest = await _context.ApprovalRequests
                                                .Include(ar => ar.LeaveRequest)
                                                .FirstOrDefaultAsync(ar => ar.Id == id);

            if (approvalRequest == null) return false;

            _context.LeaveRequests.Remove(approvalRequest.LeaveRequest);
            _context.ApprovalRequests.Remove(approvalRequest);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateApprovalRequest(ApprovalRequestDTO approvalRequestDTO)
        {
            var approvalRequest = await _context.ApprovalRequests
                                                .FirstOrDefaultAsync(ar => ar.Id == approvalRequestDTO.Id);

            if (approvalRequest == null) return false;

            approvalRequest.ApproverId = approvalRequestDTO.ApproverId;
            approvalRequest.LeaveRequestId = approvalRequestDTO.LeaveRequestId;
            approvalRequest.Status = approvalRequestDTO.Status;
            approvalRequest.Comment = approvalRequestDTO.Comment;

            _context.ApprovalRequests.Update(approvalRequest);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
