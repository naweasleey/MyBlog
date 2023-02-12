using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBlog.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //[Required] //tidak boleh kosong kolomnya
        [StringLength(12, MinimumLength = 3, 
            ErrorMessage = "Username tidak boleh lebih dari 12 karakter atau kurang dari 3 karakter")]
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string? Photo { get; set; } //boleh kosong
        public Role Roles { get; set; }
  
    }

}
