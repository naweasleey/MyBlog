using Microsoft.EntityFrameworkCore;
using MyBlog.Models;

namespace MyBlog.Data
{
    public class MysqlConetext : DbContext
    {
        public MysqlConetext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Post> Posts { get; set; }
    }
}
