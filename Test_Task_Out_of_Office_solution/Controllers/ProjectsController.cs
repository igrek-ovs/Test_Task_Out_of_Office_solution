using Microsoft.AspNetCore.Mvc;
using Test_Task_Out_of_Office_solution.dto_s;
using Test_Task_Out_of_Office_solution.services.interfaces;

namespace Test_Task_Out_of_Office_solution.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProjectDTO>>> GetProjects([FromQuery] ProjectFilterDTO filterDTO)
    {
        var projects = await _projectService.GetProjectsByFilter(filterDTO);
        return Ok(projects);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDTO>> GetProject(int id)
    {
        var project = await _projectService.GetProjectById(id);

        if (project == null) return NotFound();

        return Ok(project);
    }

    [HttpPost]
    public async Task<ActionResult> AddProject([FromBody] ProjectDTO projectDTO)
    {
        var response = await _projectService.AddProject(projectDTO);
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateProject([FromBody] ProjectDTO projectDTO)
    {
        var result = await _projectService.UpdateProject(projectDTO);

        if (!result) return NotFound();

        return NoContent();
    }

    [HttpPatch("{id}/deactivate")]
    public async Task<ActionResult> DeactivateProject(int id)
    {
        var result = await _projectService.DeactivateProject(id);

        if (!result) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProject(int id)
    {
        var result = await _projectService.DeleteProject(id);

        if (!result) return NotFound();

        return NoContent();
    }
}