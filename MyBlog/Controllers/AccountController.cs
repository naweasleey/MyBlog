using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Data;
using MyBlog.Models.VIewModels;
using System.Security.Claims;

namespace MyBlog.Controllers
{
    public class AccountController : Controller
    {
        private readonly MysqlConetext _context;

        public AccountController(MysqlConetext c)
        {
            _context = c;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromForm]Login data)
        {
            var user = _context.User
                .Where(x => x.Username == data.Username && x.Password == data.Password)
                .FirstOrDefault(); //mencari username dan password yang  dikirim dari form
            if (user != null)
            {
                var claims = new List<Claim>()
                {
                    new Claim("username", user.Username),
                    new Claim("name", user.Fullname),
                    new Claim("role", "Admin")
                };

                var identity = new ClaimsIdentity(claims, "Cookie", "name", "role");
                var principal = new ClaimsPrincipal(identity);

                //cara login jika usernya ketemu
                await HttpContext.SignInAsync(principal);

                return Redirect("/user/index");
            }
            //jika user tidak ketemu dibalikin ke halaman login
            return View();
        }
    }
}

//hal yang dilakukan saat login pertama yaitu memcari user
//Claims adalah data user yang sedang login, bisa banyak
//inspect - storage - cookie(firefoxx) - application(chorme) - jika di hapus maka akan logout karena cookie untuk menyimpan si user itu