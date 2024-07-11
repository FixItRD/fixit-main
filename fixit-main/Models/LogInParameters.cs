using System.ComponentModel.DataAnnotations;

namespace fixit_main.Models
{
    public class LogInParameters
    {
        [Required]
        public string Role { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
