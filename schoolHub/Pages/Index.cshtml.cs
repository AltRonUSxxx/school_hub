using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using schoolHub.data;
using schoolHub.Models;

namespace schoolHub.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        [BindProperty]
        public string RegisterName { get; set; }
        [BindProperty]
        public string RegisterLogin { get; set; }
        [BindProperty]
        public string RegisterPassword { get; set; }
        [BindProperty]
        public string RegisterPasswordConfirm { get; set; }


        [BindProperty]
        public string LoginLogin { get; set; }
        [BindProperty]
        public string LoginPassword { get; set; }


        public bool IsAuthorized { get; set; }
        public string CurrentUserName { get; set; }
        public string Message { get; set; }

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            LoadUser();

        }

        public IActionResult OnPostLogin()
        {
            LoadUser();
            if (string.IsNullOrEmpty(LoginPassword) || string.IsNullOrEmpty(LoginLogin))
            {
                Message = "Fill all login's fields";
                return Page();
            }

            var loginExists = _context.Users.FirstOrDefault(x => x.login == LoginLogin);
            if (loginExists == null)
            {
                Message = "This username not found";
                return Page();
            }

            if(loginExists.password != LoginPassword)
            {
                Message = "Uncorrect password";
                return Page();
            }

            HttpContext.Session.SetInt32("UserId", loginExists.id);
            HttpContext.Session.SetString("UserName", loginExists.name);
            HttpContext.Session.SetString("UserLogin", loginExists.login);

            return RedirectToPage();
        }

        public IActionResult OnPostLogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToPage();
        }

        public IActionResult OnPostRegister()
        {
            LoadUser();
            if(string.IsNullOrEmpty(RegisterName) || string.IsNullOrEmpty(RegisterLogin) || string.IsNullOrEmpty(RegisterPassword) || string.IsNullOrEmpty(RegisterPasswordConfirm))
            {
                Message = "Fill all register's fields";
                return Page();
            }

            bool loginExists = _context.Users.Any(x => x.login == RegisterLogin);
            if(loginExists)
            {
                Message = "This username already taken";
                return Page();
            }

            if(RegisterPasswordConfirm != RegisterPassword)
            {
                Message = "Passwords should was same";
                return Page();
            }



            var user = new User
            {
                name = RegisterName,
                login = RegisterLogin,
                password = RegisterPassword
            };

            _context.Users.Add(user);
            _context.SaveChanges();
            HttpContext.Session.SetInt32("UserId", user.id);
            HttpContext.Session.SetString("UserName", user.name);
            HttpContext.Session.SetString("UserLogin", user.login);

            return RedirectToPage();
        }

        private void LoadUser()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var userName = HttpContext.Session.GetString("UserName");

            IsAuthorized = userId != null;
            if(!string.IsNullOrEmpty(userName))
            {
                CurrentUserName = userName;
            }
        }
    }
}
