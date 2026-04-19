using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolHub.Data;

namespace SchoolHub.Pages
{
    public class editProjectModel : PageModel
    {
        private readonly AppDbContext _context;
        
        public editProjectModel(AppDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public int Id { get; set; }
        [BindProperty]
        public string Title { get; set; } = string.Empty;
        [BindProperty]
        public string Description { get; set; } = string.Empty;
        [BindProperty]
        public string Category { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public List<string> Categories { get; } = new()
        {
            "Programming",
            "Games",
            "Sites",
            "Mobile apps",
            "Design",
            "Other"
        };
        public IActionResult OnGet(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if(userId == null)
            {
                return RedirectToPage("/Index");
            }

            var project = _context.Projects.FirstOrDefault(x => x.Id == id);
            if(project == null)
            {
                return RedirectToPage("/Index");
            }

            if(project.AuthorId == userId)
            {
                return RedirectToPage("/Projects");
            }

            Id = project.Id;
            Title = project.Title;
            Description = project.Description;
            Category = project.Category;

            return Page();
        }
    }
}
