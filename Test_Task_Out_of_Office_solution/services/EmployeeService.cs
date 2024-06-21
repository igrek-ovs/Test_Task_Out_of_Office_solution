using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test_Task_Out_of_Office_solution.data;
using Test_Task_Out_of_Office_solution.dto_s;
using Test_Task_Out_of_Office_solution.models;
using Test_Task_Out_of_Office_solution.services.interfaces;

namespace Test_Task_Out_of_Office_solution.services;

public class EmployeeService : IEmployeeService
{
    private readonly TaskContext _context;

    public EmployeeService(TaskContext context)
    {
        _context = context;
    }

    public async Task AddEmployee(EmployeeDTO employeeDTO)
    {
        var employee = new Employee
        {
            FullName = employeeDTO.FullName,
            Subdivision = employeeDTO.Subdivision,
            Position = employeeDTO.Position,
            PeoplePartnerId = employeeDTO.PeoplePartnerId,
            Photo = employeeDTO.Photo,
            OutOfOfficeBalance = employeeDTO.OutOfOfficeBalance
        };
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteEmployee(int employeeId)
    {
        var employee = await _context.Employees.FindAsync(employeeId);
        if (employee == null) return false;
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateEmployee(EmployeeDTO employeeDTO)
    {
        var employee = await _context.Employees.FindAsync(employeeDTO.Id);
        if (employee == null) return false;
        employee.FullName = employeeDTO.FullName;
        employee.Subdivision = employeeDTO.Subdivision;
        employee.Position = employeeDTO.Position;
        employee.PeoplePartnerId = employeeDTO.PeoplePartnerId;
        employee.Photo = employeeDTO.Photo;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleDeactivateEmployee(int employeeId)
    {
        var employee = await _context.Employees.FindAsync(employeeId);
        if (employee == null) return false;
        employee.IsActive = !employee.IsActive;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<EmployeeDTO>> GetEmployeesByFilter(EmployeeFilterDTO filterDTO)
    {
        var query = _context.Employees.AsQueryable();

        if (!string.IsNullOrEmpty(filterDTO.Position)) query = query.Where(e => e.Position == filterDTO.Position);

        if (!string.IsNullOrEmpty(filterDTO.SearchByName))
            query = query.Where(e => e.FullName.Contains(filterDTO.SearchByName));

        if (!string.IsNullOrEmpty(filterDTO.Subdivision))
            query = query.Where(e => e.Subdivision == filterDTO.Subdivision);

        if (filterDTO.IsActive.HasValue) query = query.Where(e => e.IsActive == filterDTO.IsActive.Value);

        if (filterDTO.OutOfOfficeBalanceLeft.HasValue)
            query = query.Where(e => e.OutOfOfficeBalance >= filterDTO.OutOfOfficeBalanceLeft.Value);

        if (filterDTO.OutOfOfficeBalanceRight.HasValue)
            query = query.Where(e => e.OutOfOfficeBalance <= filterDTO.OutOfOfficeBalanceRight.Value);

        if (!string.IsNullOrEmpty(filterDTO.SortBy))
            switch (filterDTO.SortBy.ToLower())
            {
                case "fullname":
                    query = filterDTO.SortAscending == true
                        ? query.OrderBy(e => e.FullName)
                        : query.OrderByDescending(e => e.FullName);
                    break;
                case "isactive":
                    query = filterDTO.SortAscending == true
                        ? query.OrderBy(e => e.IsActive)
                        : query.OrderByDescending(e => e.IsActive);
                    break;
                case "outofofficebalance":
                    query = filterDTO.SortAscending == true
                        ? query.OrderBy(e => e.OutOfOfficeBalance)
                        : query.OrderByDescending(e => e.OutOfOfficeBalance);
                    break;
                default:
                    query = filterDTO.SortAscending == true
                        ? query.OrderBy(e => e.Id)
                        : query.OrderByDescending(e => e.Id);
                    break;
            }

        var employees = await query.ToListAsync();

        return employees.Select(e => new EmployeeDTO
        {
            Id = e.Id,
            FullName = e.FullName,
            Subdivision = e.Subdivision,
            Position = e.Position,
            PeoplePartnerId = e.PeoplePartnerId,
            Photo = e.Photo,
            IsActive = e.IsActive,
            OutOfOfficeBalance = e.OutOfOfficeBalance
        }).ToList();
    }

    public async Task<List<EmployeeDTO>> GetHRManagers()
    {
        var hrs = await _context.Employees.Where(em => em.Position == "HR Manager").ToListAsync();
        return hrs.Select(e => new EmployeeDTO
        {
            Id = e.Id,
            FullName = e.FullName,
            Subdivision = e.Subdivision,
            Position = e.Position,
            PeoplePartnerId = e.PeoplePartnerId,
            Photo = e.Photo,
            IsActive = e.IsActive,
            OutOfOfficeBalance = e.OutOfOfficeBalance
        }).ToList();
    }

    public async Task<List<EmployeeDTO>> GetProjectManagers()
    {
        var projectManagers = await _context.Employees.Where(em => em.Position == "Project Manager").ToListAsync();
        return projectManagers.Select(e => new EmployeeDTO
        {
            Id = e.Id,
            FullName = e.FullName,
            Subdivision = e.Subdivision,
            Position = e.Position,
            PeoplePartnerId = e.PeoplePartnerId,
            Photo = e.Photo,
            IsActive = e.IsActive,
            OutOfOfficeBalance = e.OutOfOfficeBalance
        }).ToList();
    }

    public async Task<Employee> GetUserRole(string fullName, string position)
    {
        var user = _context.Employees.Where(em => em.FullName == fullName && em.Position == position)
            .FirstOrDefaultAsync().Result;
        return user;
    }

    public async Task<bool> AssignEmployeeToProject(int employeeId, int projectId)
    {
        var employee = await _context.Employees
            .Include(e => e.Projects)
            .FirstOrDefaultAsync(e => e.Id == employeeId);
        var project = await _context.Projects
            .Include(p => p.Employees)
            .FirstOrDefaultAsync(p => p.Id == projectId);
        if (employee != null && project != null)
        {
            if (employee.Projects == null) employee.Projects = new List<Project>();
            if (!employee.Projects.Contains(project))
            {
                employee.Projects.Add(project);
                await _context.SaveChangesAsync();
            }

            return true;
        }

        return false;
    }


    public async Task<EmployeeDTO> GetEmployeeById(int employeeId)
    {
        var employee = await _context.Employees.Where(e => e.Id == employeeId).FirstOrDefaultAsync();
        return new EmployeeDTO
        {
            Id = employee.Id,
            FullName = employee.FullName,
            IsActive = employee.IsActive,
            OutOfOfficeBalance = employee.OutOfOfficeBalance,
            PeoplePartnerId = employee.PeoplePartnerId,
            Position = employee.Position,
            Photo = employee.Photo,
            Subdivision = employee.Subdivision
        };
    }

    public async Task<bool> UploadPhotoAsync(int employeeId, IFormFile photo)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);
        if (employee == null) return false;

        if (photo != null && photo.Length > 0)
        {
            using (var ms = new MemoryStream())
            {
                await photo.CopyToAsync(ms);
                employee.Photo = ms.ToArray();
            }

            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<FileStreamResult> GetPhotoAsync(int employeeId)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);
        if (employee == null || employee.Photo == null) return null;

        var photoStream = new MemoryStream(employee.Photo);
        return new
            FileStreamResult(photoStream,
                "image/jpeg")
            {
                FileDownloadName = $"{employeeId}_photo.jpg"
            };
    }
}