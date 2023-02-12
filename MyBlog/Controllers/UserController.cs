using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data;
using MyBlog.Models;
using MyBlog.Models.VIewModels;
using System.Security.Claims;


namespace MyBlog.Controllers
{
	[Authorize]
	//harus login untuk akases ke sebuah url, maka ditambahin authorize diatas class
	public class UserController : Controller
	{
		private readonly MysqlConetext _context;
		private readonly IWebHostEnvironment _env;
		public UserController(MysqlConetext context, IWebHostEnvironment env)
		{
			_context = context;
			_env = env;
		}

		public IActionResult Index()
		{
			var users = _context.User
				.Include(x => x.Roles)
				.ToList();
			return View(users);
		}

		public IActionResult Detail(int id)
		{
			var users = _context.User.FirstOrDefault(x => x.Id == id);
			return View(users);
		}

        public IActionResult Download(int id) // cara ngambil file terus di download
        {
            var users = _context.User.FirstOrDefault(x => x.Id == id);
			var filepath = Path.Combine(_env.WebRootPath, "Upload", users.Photo);

            return File(
				System.IO.File.ReadAllBytes(filepath), "image/png", 
				Path.GetFileName(filepath));
        }



        public IActionResult Create()
        {
            ViewBag.Roles = _context.Roles.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });
            ;
            return View();
        }

        [HttpPost]
		public IActionResult Create([FromForm] UserForm data, IFormFile Photo)
		{
			if (Photo.Length > 100000)
			{
				ModelState.AddModelError(nameof(data.Photo), "Fotonya kegedean coy");
			}

			if (!ModelState.IsValid)
			{
				return View();
			}
			var filename = "photo_" + data.Username + Path.GetExtension(Photo.FileName);
			var filepath = Path.Combine(_env.WebRootPath, "Upload", filename);

			using (var stream = System.IO.File.Create(filepath))
			{
				Photo.CopyTo(stream); //menyimpan foto ke folder (proses menyimpan file)
			}
			data.Photo = filename;

            var role = _context.Roles.FirstOrDefault(x => x.Id == data.Role);
            var user = new User()
            {
                Username = data.Username,
                Password = data.Password,
                Fullname = data.Fullname,
                Photo = filename,
                Roles = role
            };

            _context.User.Add(user);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}

	


		public IActionResult Edit(int id)
		{
			var user = _context.User.FirstOrDefault(x => x.Id == id);
			return View(user);
		}

		[HttpPost]
		public IActionResult Edit([FromForm] User data)
		{
			_context.User.Update(data);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}

		public IActionResult Delete(int id)
		{
			var user = _context.User.FirstOrDefault(x => x.Id == id);
			_context.User.Remove(user);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}
