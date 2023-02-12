using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MyBlog.Data;
using MyBlog.Models;

namespace BlogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly MysqlConetext _context;

        public PostController(MysqlConetext c)
        {
            _context = c;
        }

        [HttpGet("")] //method untuk meminta data ke aplikasi
        public IActionResult GetAllPost()
        {
            var posts = _context.Posts.ToList();
            return Ok(posts);
        }

        [HttpGet("id")] //method untuk meminta data ke aplikasi
        public IActionResult GetPostbyId(int id)
        {
            var posts = _context.Posts.FirstOrDefault(x => x.Id == id);
            if (posts == null)
            {
                return NotFound("Data post tidak ketemu"); //bisa pake BadRequest atau NotFound
            }
            return Ok(posts);
        }

        [HttpPost("create")]
        public IActionResult AddPost([FromBody] Post data)
        {
            _context.Posts.Add(data);
            _context.SaveChanges();

            //return Ok(data);
            return NoContent();
        }
        //[HttpPut("update")] //kalo put bawa semua property kalo sebagian httppatch
        [HttpPatch("update")]
        public IActionResult UpdatePost([FromBody] Post data)
        {
            _context.Posts.Update(data);
            _context.SaveChanges();

            //return Ok(data);
            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeletePost(int id)
        {
            var post = _context.Posts.FirstOrDefault(x => x.Id == id);
            _context.Posts.Remove(post);
            _context.SaveChanges();

            //return Ok(data);
            return NoContent();
        }

        [HttpGet("search")]
        public IActionResult SearchPost([FromQuery] string keyword)
        {
            var post = _context.Posts.
                Where(x => x.Title.ToLower().Contains(keyword) ||
                    //x.Title.ToLower().StartsWith(keyword) ||
                    //x.Title.ToLower().EndsWith(keyword) ||
                    x.Title.ToLower().Contains(keyword))
                .ToList();

            //contains == %like%
            //startwith == like%
            //endwith == %like


            return Ok(post);
            //return NoContent();
        }
    }
}

//api di pakai jika frontend sama backend dibikin terpisah
//minimal tau https status code 
//kalo awalnya angka 4 error dari frontend
//kalo awalnya angka 5 error dari backend
//kalo nanti kalian di backend tapi errornya angka 4 maki - maki balik aja - a iyan



