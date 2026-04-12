using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using schoolHub.data;
using schoolHub.Models;

namespace schoolHub.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        [BindProperty]
        public string RegisterName { get; set; }
        [BindProperty]
        public string RegisterLogin { get; set; }
        [BindProperty]
        public string RegisterPassword { get; set; }
        [BindProperty]
        public string RegisterPasswordConfirm { get; set; }

        [BindProperty]
        public int RegisterAge { get; set; }

        [BindProperty]
        public string LoginLogin { get; set; }
        [BindProperty]
        public string LoginPassword { get; set; }


        public bool IsAuthorized { get; set; }
        public string Message { get; set; }


        public string CurrentUserName { get; set; }
        public string CurrentUserLogin { get; set; }
        public int CurrentUserAge { get; set; }

        public IndexModel(AppDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
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
                Message = "Uncorrect username or password";
                return Page();
            }

            if(_passwordHasher.VerifyHashedPassword(loginExists, loginExists.hashed_password, LoginPassword) != PasswordVerificationResult.Success)
            {
                Message = "Uncorrect username or password";
                return Page();
            }

            HttpContext.Session.SetInt32("UserId", loginExists.id);
            HttpContext.Session.SetString("UserName", loginExists.name);
            HttpContext.Session.SetString("UserLogin", loginExists.login);
            HttpContext.Session.SetInt32("UserAge", loginExists.age);

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
            if(string.IsNullOrEmpty(RegisterName) ||
                string.IsNullOrEmpty(RegisterLogin) ||
                string.IsNullOrEmpty(RegisterPassword) ||
                string.IsNullOrEmpty(RegisterPasswordConfirm) ||
                RegisterAge == null)
            {
                Message = "Fill all register's fields";
                return Page();
            }

            if(RegisterAge <= 0)
            {
                Message = "Age should be was more than 0";
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
                age = RegisterAge
            };
            user.hashed_password = _passwordHasher.HashPassword(user, RegisterPassword);

            _context.Users.Add(user);
            _context.SaveChanges();
            HttpContext.Session.SetInt32("UserId", user.id);
            HttpContext.Session.SetString("UserName", user.name);
            HttpContext.Session.SetString("UserLogin", user.login);
            HttpContext.Session.SetInt32("UserAge", user.age);

            return RedirectToPage();
        }

        private void LoadUser()
        {
            //var userId = HttpContext.Session.GetInt32("UserId");
            //var userName = HttpContext.Session.GetString("UserName");
            //var userLogin = HttpContext.Session.GetString("UserLogin");
            //var userAge = HttpContext.Session.GetInt32("UserAge");

            //IsAuthorized = userId != null;
            //if(!string.IsNullOrEmpty(userName))
            //{
            //    CurrentUserName = userName;
            //    CurrentUserAge = Convert.ToInt32(userAge);
            //    CurrentUserLogin = userLogin;
            //}

            var userId = HttpContext.Session.GetInt32("UserId");
            if(userId == null)
            {
                IsAuthorized = false;
                return;
            }

            var user = _context.Users.FirstOrDefault(x => x.id == userId.Value);

            if(user == null)
            {
                IsAuthorized = false;
                HttpContext.Session.Clear();
                return;
            }

            IsAuthorized = true;
            CurrentUserName = user.name;
            CurrentUserLogin = user.login;
            CurrentUserAge = user.age;
        }
    }
}
