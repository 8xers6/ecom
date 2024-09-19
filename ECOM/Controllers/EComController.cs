using ECOM.Data;
using ECOM.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authorization;

namespace ECOM.Controllers
{
    [Authorize]
    public class EComController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public EComController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var user = await dbContext.Users.ToListAsync();

            return View(user);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(UserInformationViewModel viewModel)
        {

            var user = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x=>x.Id==viewModel.Id);
            if (user is not null) {
                dbContext.Users.Remove(viewModel);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("List", "ECom");
        }



        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Logout()
        {
            // Sign the user out of the authentication scheme
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to the login page after logout
            return RedirectToAction("Login", "Account");
        }



    }

  }

        






