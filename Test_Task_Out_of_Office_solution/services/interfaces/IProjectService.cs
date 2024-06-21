using Test_Task_Out_of_Office_solution.dto_s;
using Test_Task_Out_of_Office_solution.models;

namespace Test_Task_Out_of_Office_solution.services.interfaces;

public interface IProjectService
{
    Task<List<ProjectDTO>> GetProjectsByFilter(ProjectFilterDTO filterDTO);
    Task<ProjectDTO> GetProjectById(int id);
    Task<Project> AddProject(ProjectDTO projectDTO);
    Task<bool> UpdateProject(ProjectDTO projectDTO);
    Task<bool> DeactivateProject(int id);
    Task<bool> DeleteProject(int id);
}