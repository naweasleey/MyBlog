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
        public DbSet<User> User { get; set; }
        public DbSet<Role> Roles { get; set; } //relasi

    }
}
