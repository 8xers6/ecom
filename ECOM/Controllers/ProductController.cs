using ECOM.Data;
using ECOM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace ECOM.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment environment;

        public ProductController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            this.environment = environment;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> List()
        {
            var product = await _context.Product.ToListAsync();

            return View(product);
        }
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductViewModel viewModel)
        {
            if (viewModel.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "The Image file is required");
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            // Get the currently logged-in user's username
            string username = User.Identity.Name;

            if (username == null)
            {
                return Unauthorized();
            }

            // Generate a unique file name for the uploaded image
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(viewModel.ImageFile!.FileName);
            string imageFullPath = Path.Combine(environment.WebRootPath, "productimage", newFileName);

            // Save the uploaded image file to the server
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                await viewModel.ImageFile.CopyToAsync(stream);
            }

            // Create a new Product entity
            var product = new ProductViewModel
            {
                product_owner = username,
                product_image = newFileName,
                product_name = viewModel.product_name,
                product_description = viewModel.product_description,
                product_stocks = viewModel.product_stocks,
                product_variant = viewModel.product_variant,
                product_price = viewModel.product_price
            };

            // Add the product to the database
            await _context.Product.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Add", "Product");
        }
    }


    }
