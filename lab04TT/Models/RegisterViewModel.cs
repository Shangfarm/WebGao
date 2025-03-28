using System.ComponentModel.DataAnnotations;

namespace lab04TT.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string FullName { get; set; }

        public string? Address { get; set; }

        public string? Age { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

}
