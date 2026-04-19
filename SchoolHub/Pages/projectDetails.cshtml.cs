using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolHub.Data;
using SchoolHub.Models;

namespace SchoolHub.Pages
{
    public class projectDetailsModel : PageModel
    {
        private readonly AppDbContext _context;
        public Project? ProjectItem { get; set; }

        public projectDetailsModel(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int id)
        {
            ProjectItem = _context.Projects
                .Include(x => x.Author)
                .FirstOrDefault(x => x.Id == id);

            if(ProjectItem == null)
            {
                return RedirectToPage("/Projects");
            }

            return Page();
        }
    }
}
