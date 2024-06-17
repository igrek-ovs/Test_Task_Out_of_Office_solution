using Microsoft.EntityFrameworkCore;
using Test_Task_Out_of_Office_solution.data;
using Test_Task_Out_of_Office_solution.dto_s;
using Test_Task_Out_of_Office_solution.models;
using Test_Task_Out_of_Office_solution.services.interfaces;

namespace Test_Task_Out_of_Office_solution.services
{
    public class ProjectService : IProjectService
    {
        private readonly TaskContext _context;

        public ProjectService(TaskContext context)
        {
            _context = context;
        }

        public async Task<List<ProjectDTO>> GetProjectsByFilter(ProjectFilterDTO filterDTO)
        {
            var query = _context.Projects.Include(p => p.ProjectManager).AsQueryable();

            // Применение фильтров
            if (!string.IsNullOrEmpty(filterDTO.ProjectType))
            {
                query = query.Where(p => p.ProjectType.Contains(filterDTO.ProjectType));
            }

            if (filterDTO.StartDateFrom.HasValue && filterDTO.StartDateTo.HasValue)
            {
                query = query.Where(lr =>
                    lr.StartDate >= filterDTO.StartDateFrom.Value && lr.EndDate <= filterDTO.StartDateTo.Value);
            }
            else if (filterDTO.StartDateFrom.HasValue)
            {
                query = query.Where(lr => lr.StartDate >= filterDTO.StartDateFrom.Value);
            }
            else if (filterDTO.StartDateTo.HasValue)
            {
                query = query.Where(lr => lr.EndDate <= filterDTO.StartDateTo.Value);
            }

            if (filterDTO.Status.HasValue)
            {
                query = query.Where(p => p.Status == filterDTO.Status.Value);
            }

            if (filterDTO.ProjectNumber.HasValue)
            {
                query = query.Where(p => p.Id == filterDTO.ProjectNumber.Value);
            }

            // Сортировка
            if (!string.IsNullOrEmpty(filterDTO.SortBy))
            {
                switch (filterDTO.SortBy)
                {
                    case "name":
                        query = filterDTO.SortAscending == true ? query.OrderBy(p => p.ProjectType) : query.OrderByDescending(p => p.ProjectType);
                        break;
                    case "startDate":
                        query = filterDTO.SortAscending == true ? query.OrderBy(p => p.StartDate) : query.OrderByDescending(p => p.StartDate);
                        break;
                    case "status":
                        query = filterDTO.SortAscending == true ? query.OrderBy(p => p.Status) : query.OrderByDescending(p => p.Status);
                        break;
                    default:
                        break;
                }
            }

            return await query.Select(p => new ProjectDTO
            {
                Id = p.Id,
                ProjectType = p.ProjectType,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                ProjectManagerId = p.ProjectManagerId,
                ProjectManagerName = p.ProjectManager.FullName,
                Comment = p.Comment,
                Status = p.Status
            }).ToListAsync();
        }

        public async Task<ProjectDTO> GetProjectById(int id)
        {
            var project = await _context.Projects.Include(p => p.ProjectManager).FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
            {
                return null;
            }

            return new ProjectDTO
            {
                Id = project.Id,
                ProjectType = project.ProjectType,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                ProjectManagerId = project.ProjectManagerId,
                ProjectManagerName = project.ProjectManager.FullName,
                Comment = project.Comment,
                Status = project.Status
            };
        }

        public async Task<Project> AddProject(ProjectDTO projectDTO)
        {
            var project = new Project
            {
                ProjectType = projectDTO.ProjectType,
                StartDate = projectDTO.StartDate,
                EndDate = projectDTO.EndDate,
                ProjectManagerId = projectDTO.ProjectManagerId,
                Comment = projectDTO.Comment
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task<bool> UpdateProject(ProjectDTO projectDTO)
        {
            var project = await _context.Projects.FindAsync(projectDTO.Id);

            if (project == null)
            {
                return false;
            }

            project.ProjectType = projectDTO.ProjectType;
            project.StartDate = projectDTO.StartDate;
            project.EndDate = projectDTO.EndDate;
            project.ProjectManagerId = projectDTO.ProjectManagerId;
            project.Comment = projectDTO.Comment;
            project.Status = projectDTO.Status;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeactivateProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return false;
            }

            project.Status = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return false;
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
