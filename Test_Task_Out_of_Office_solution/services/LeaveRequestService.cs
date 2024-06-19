using Microsoft.EntityFrameworkCore;
using Test_Task_Out_of_Office_solution.data;
using Test_Task_Out_of_Office_solution.dto_s;
using Test_Task_Out_of_Office_solution.models;
using Test_Task_Out_of_Office_solution.services.interfaces;

namespace Test_Task_Out_of_Office_solution.services
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly TaskContext _context;

        public LeaveRequestService(TaskContext context)
        {
            _context = context;
        }

        public async Task<List<LeaveRequestDTO>> GetLeaveRequestsByFilter(LeaveRequestFilterDTO filterDTO)
        {
            var query = _context.LeaveRequests.AsQueryable();

            if (!string.IsNullOrEmpty(filterDTO.AbsenceReason))
            {
                query = query.Where(lr => lr.AbsenceReason == filterDTO.AbsenceReason);
            }

            if (filterDTO.EmployeeId.HasValue)
            {
                query = query.Where(lr => lr.EmployeeId == filterDTO.EmployeeId);
            }

            if (filterDTO.StartDate.HasValue && filterDTO.EndDate.HasValue)
            {
                query = query.Where(lr =>
                    lr.StartDate >= filterDTO.StartDate.Value && lr.EndDate <= filterDTO.EndDate.Value);
            }
            else if (filterDTO.StartDate.HasValue)
            {
                query = query.Where(lr => lr.StartDate >= filterDTO.StartDate.Value);
            }
            else if (filterDTO.EndDate.HasValue)
            {
                query = query.Where(lr => lr.EndDate <= filterDTO.EndDate.Value);
            }


            if (!string.IsNullOrEmpty(filterDTO.Status))
            {
                query = query.Where(lr => lr.Status == filterDTO.Status);
            }

            if (filterDTO.RequestNumber.HasValue)
            {
                query = query.Where(lr => lr.Id == filterDTO.RequestNumber.Value);
            }

            if (!string.IsNullOrEmpty(filterDTO.SortBy))
            {
                switch (filterDTO.SortBy.ToLower())
                {
                    case "employeename":
                        query = filterDTO.SortAscending ?? true
                            ? query.OrderBy(lr => lr.Employee.FullName)
                            : query.OrderByDescending(lr => lr.Employee.FullName);
                        break;
                    case "startdate":
                        query = filterDTO.SortAscending ?? true
                            ? query.OrderBy(lr => lr.StartDate)
                            : query.OrderByDescending(lr => lr.StartDate);
                        break;
                    case "enddate":
                        query = filterDTO.SortAscending ?? true
                            ? query.OrderBy(lr => lr.EndDate)
                            : query.OrderByDescending(lr => lr.EndDate);
                        break;
                    case "status":
                        query = filterDTO.SortAscending ?? true
                            ? query.OrderBy(lr => lr.Status)
                            : query.OrderByDescending(lr => lr.Status);
                        break;
                }
            }

            var leaveRequests = await query
                .Select(lr => new LeaveRequestDTO
                {
                    Id = lr.Id,
                    EmployeeId = lr.EmployeeId,
                    EmployeeName = lr.Employee.FullName,
                    AbsenceReason = lr.AbsenceReason,
                    StartDate = lr.StartDate,
                    EndDate = lr.EndDate,
                    Comment = lr.Comment,
                    Status = lr.Status,
                    ApprovalRequestId = lr.ApprovalRequest != null ? lr.ApprovalRequest.Id : (int?)null,
                    ApprovalStatus = lr.ApprovalRequest != null ? lr.ApprovalRequest.Status : null
                }).ToListAsync();

            return leaveRequests;
        }

        public async Task<LeaveRequestDTO> GetLeaveRequestById(int id)
        {
            var leaveRequest = await _context.LeaveRequests
                .Include(lr => lr.Employee)
                .Include(lr => lr.ApprovalRequest)
                .FirstOrDefaultAsync(lr => lr.Id == id);

            if (leaveRequest == null)
            {
                return null;
            }

            return new LeaveRequestDTO
            {
                Id = leaveRequest.Id,
                EmployeeId = leaveRequest.EmployeeId,
                EmployeeName = leaveRequest.Employee.FullName,
                AbsenceReason = leaveRequest.AbsenceReason,
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                Comment = leaveRequest.Comment,
                Status = leaveRequest.Status,
                ApprovalRequestId = leaveRequest.ApprovalRequest != null ? leaveRequest.ApprovalRequest.Id : (int?)null,
                ApprovalStatus = leaveRequest.ApprovalRequest != null ? leaveRequest.ApprovalRequest.Status : null
            };
        }

        public async Task<LeaveRequest> AddLeaveRequest(LeaveRequestDTO leaveRequestDTO)
        {
            var leaveRequest = new LeaveRequest
            {
                EmployeeId = leaveRequestDTO.EmployeeId,
                AbsenceReason = leaveRequestDTO.AbsenceReason,
                StartDate = leaveRequestDTO.StartDate,
                EndDate = leaveRequestDTO.EndDate,
                Comment = leaveRequestDTO.Comment
            };

            _context.LeaveRequests.Add(leaveRequest);
            await _context.SaveChangesAsync();

            // var approver = await _context.Employees.Where(em => em.Id == leaveRequest.EmployeeId).FirstOrDefaultAsync();
            //
            // var approvalRequest = new ApprovalRequest
            // {
            //     ApproverId = approver.Id, // Assuming PeoplePartnerId is the approver
            //     LeaveRequestId = leaveRequest.Id,
            //     Status = "New",
            // };
            //
            // _context.ApprovalRequests.Add(approvalRequest);
            // await _context.SaveChangesAsync();

            return leaveRequest;
        }

        public async Task<bool> UpdateLeaveRequest(LeaveRequestDTO leaveRequestDTO)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(leaveRequestDTO.Id);

            if (leaveRequest == null)
            {
                return false;
            }

            leaveRequest.EmployeeId = leaveRequestDTO.EmployeeId;
            leaveRequest.AbsenceReason = leaveRequestDTO.AbsenceReason;
            leaveRequest.StartDate = leaveRequestDTO.StartDate;
            leaveRequest.EndDate = leaveRequestDTO.EndDate;
            leaveRequest.Comment = leaveRequestDTO.Comment;
            leaveRequest.Status = leaveRequestDTO.Status;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteLeaveRequest(int id)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(id);

            if (leaveRequest == null)
            {
                return false;
            }

            _context.LeaveRequests.Remove(leaveRequest);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SubmitLeaveRequest(int id)
        {
            var leaveRequest = await _context.LeaveRequests.Where(lr => lr.Id == id).FirstOrDefaultAsync();
            if (leaveRequest == null)
                return false;
            leaveRequest.Status = "Submitted";
            await _context.SaveChangesAsync();

            var employee = await _context.Employees.FindAsync(leaveRequest.EmployeeId);
            var approver = await _context.Employees.FindAsync(employee.PeoplePartnerId);


            var approvalRequest = new ApprovalRequest
            {
                ApproverId = approver.Id,
                Approver = approver,
                Comment = leaveRequest.Comment,
                LeaveRequest = leaveRequest,
                LeaveRequestId = leaveRequest.Id,
                Status = "New"
            };

            await _context.ApprovalRequests.AddAsync(approvalRequest);
            await _context.SaveChangesAsync();
                
            return true;
        }

        public async Task<bool> CancelLeaveRequest(int id)
        {
            var leaveRequest = await _context.LeaveRequests.Where(lr => lr.Id == id).FirstOrDefaultAsync();
            if (leaveRequest == null)
                return false;
            
            leaveRequest.Status = "Canceled";
            await _context.SaveChangesAsync();
            
            return true;
        }
    }
}