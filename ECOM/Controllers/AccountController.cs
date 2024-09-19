using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ECOM.Data;
using ECOM.Models;

namespace ECOM.Controllers
{

    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly ApplicationDbContext _context;
        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            return View(new UserInformationViewModel());
        }
        [HttpPost]
        public IActionResult Register(UserInformationViewModel model, string confirmPassword)
        {
            if (ModelState.IsValid)
            {
                var userExists = _context.Users.Any(u => u.username == model.username);
                var emailExist = _context.Users.Any(u => u.email == model.email);

                if (userExists)
                {
                    ModelState.AddModelError("Username", "Username is already taken.");
                }
                else if (emailExist)
                {
                    ModelState.AddModelError("Email", "Email is already taken.");
                }
                else if (model.password != confirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "The password and confirmation password do not match.");
                }
                else
                {
                    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.password);
                    model.password = hashedPassword;
                    var newUser = new UserInformationViewModel
                    {
                        firstname = model.firstname,
                        lastname = model.lastname,
                        middlename = model.middlename,
                        name_extension = model.name_extension,
                        birthday = model.birthday,
                        address = model.address,
                        contact = model.contact,
                        email = model.email,
                        username = model.username,
                        password = model.password,

                    };

                    _context.Users.Add(model);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Registration successful! Please log in.";

                    return RedirectToAction("Login", "Account");
                }
            }

            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("List", "Product");
            }

            return View(new RegisterViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Login(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.username == model.username);

                if (user == null)
                {
                    ModelState.AddModelError("username", "Invalid Username or Password.");
                    return View(model);
                }

                bool isValidPassword = BCrypt.Net.BCrypt.Verify(model.password, user.password);

                if (isValidPassword)
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.username),
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        // Optional
                    };

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    // Set welcome message and username in TempData
                    TempData["WelcomeMessage"] = $"Welcome, {user.username}!";
                    TempData["Username"] = user.username;

                    return RedirectToAction("List", "Ecom");
                }
                else
                {
                    ModelState.AddModelError("password", "Invalid password.");
                    return View(model);
                }
            }

            return View(model);
        }

    }

}
