using Microsoft.AspNetCore.Mvc;
using SchoolHub.Services;
using SchoolHub.ViewModels;

namespace SchoolHub.Controllers
{
    public class AdminProjectsController: Controller    
    {
        private readonly IProjectService _projectService;
        private readonly ICurrentUserService _currentUserService;

        public AdminProjectsController(IProjectService projectService, ICurrentUserService currentUserService)
        {
            _projectService = projectService;
            _currentUserService = currentUserService;
        }
        public IActionResult Index() 
        {
            if(!_currentUserService.IsAuthenticated(HttpContext)) 
            {
                return RedirectToPage("/Ibdex");
            }
            var project = _projectService.GetAllProjects();
            return View(project);
        }
        public IActionResult Edit(int id)
        {
            if (!_currentUserService.IsAuthenticated(HttpContext))
            {
                return RedirectToPage("/Ibdex");
            }
            var project = _projectService.GetProjectById(id);
            if (project == null) 
            {
                return RedirectToAction("/Ibdex");
            }
            var viewModel = new AdminProjectEditViewModel
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                Category = project.Category,
                Status = project.Status
            };
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AdminProjectEditViewModel model) 
        {
            if (!_currentUserService.IsAuthenticated(HttpContext))
            {
                return RedirectToPage("/Ibdex");
            }
            if (ModelState.IsValid) 
            {
                return View(model);
            }

            var project = _projectService.GetProjectById(model.Id);
            if (project == null)
            {
                return RedirectToAction("Index");
            }

            project.Title = model.Title;
            project.Description = model.Description;
            project.Category = model.Category;
            project.Status = model.Status;

            _projectService.UpdateProject(project);
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)    
        {
            if (!_currentUserService.IsAuthenticated(HttpContext))
            {
                return RedirectToPage("/Ibdex");
            }
            var project = _projectService.GetProjectById(id);
            if (project == null)
            {
                return RedirectToAction("/Ibdex");
            }

            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(AdminProjectEditViewModel model)
        {
            if (!_currentUserService.IsAuthenticated(HttpContext))
            {
                return RedirectToPage("/Ibdex");
            }
            if (ModelState.IsValid)
            {
                return View(model);
            }

            var project = _projectService.GetProjectById(model.Id);
            if (project == null)
            {
                return RedirectToAction("Index");
            }

            _projectService.DeleteProject(project);
            return RedirectToAction("Index");
        }

        

    }
}
