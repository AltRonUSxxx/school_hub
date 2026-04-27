using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SchoolHub.Dtos;
using SchoolHub.Models;
using SchoolHub.Services;
using System.ComponentModel;
using System.Reflection;

namespace SchoolHub.Controllers.Api
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectsApiController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ICurrentUserService _currentService;

        public ProjectsApiController
            (
                IProjectService projectService,
                ICurrentUserService currentUserService
            )
            {
                _projectService = projectService;
                _currentService = currentUserService;
            }

        [HttpGet]
        public ActionResult<List<ProjectDto>> GetAll()
        {
            var projects = _projectService.GetAllProjects();
            var result = projects.Select(project => ToDto(project)).ToList();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<List<ProjectDto>> GetById(int id)
        {
            var project = _projectService.GetProjectById(id);
            if(project == null)
            {
                return BadRequest(new 
                {
                    message = "Project not found"
                });
            }
            return Ok(ToDto(project));
        }

        [HttpPost]
        public ActionResult<ProjectDto> Create(CreateProjectDto dto)
        {
            var userId = _currentService.GetCurrentUserId(HttpContext);

            if(userId == null)
            {
                return Unauthorized(new
                {
                    message = "For creation, u should logined"
                });
            }

            var project = new Project
            {
                Title = dto.Title,
                Description = dto.Description,
                Status = dto.Status,
                CreatedAt = DateTime.Now,
                AuthorId = userId.Value
            };

            _projectService.AddProject(project);

            var createdProject = _projectService.GetProjectById(project.Id);
            if(createdProject == null)
            {
                return BadRequest(new
                {
                    message = "Project was created, but doesn't uplouded"
                });
            }

            return CreatedAtAction
                (
                nameof(GetById),
                new { id = createdProject.Id},
                ToDto(createdProject)
                );
        }


        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateProjectDto dto)
        {
            var userId = _currentService.GetCurrentUserId(HttpContext);

            if (userId == null)
            {
                return Unauthorized(new
                {
                    message = "For redaction, u should logined"
                });
            }

            var project = _projectService.GetProjectById(id);
            if(project == null)
            {
                return NotFound(new {message = "Project not found"});
            }

            if(project.AuthorId != userId.Value)
            {
                return Forbid();
            }

            if(project.Status == "Завершён")
            {
                return BadRequest(new {message = "Completed project unredactable"});
            }

            project.Title = dto.Title;
            project.Description = dto.Description;
            project.Status = dto.Status;
            project.Category = dto.Category;

            _projectService.UpdateProject(project);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var userId = _currentService.GetCurrentUserId(HttpContext);

            if (userId == null)
            {
                return Unauthorized(new
                {
                    message = "For delete, u should logined"
                });
            }

            var project = _projectService.GetProjectById(id);
            if (project == null)
            {
                return NotFound(new { message = "Project not found" });
            }

            if (project.AuthorId != userId.Value)
            {
                return Forbid();
            }

            _projectService.DeleteProject(project);
            return NoContent();
        }

        public static ProjectDto ToDto(Project project)
        {
            return new ProjectDto
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                Category = project.Category,
                Status = project.Status,
                CreatedAt = project.CreatedAt,
                AuthorId = project.AuthorId,
                Author = project.Author?.Name
            };
        }


    }
}
