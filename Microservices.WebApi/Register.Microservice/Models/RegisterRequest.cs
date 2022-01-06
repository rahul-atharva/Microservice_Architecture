using System.ComponentModel.DataAnnotations;

namespace Register.Microservice.Models
{
    public class RegisterRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string EmailAddress { get; set; } = string.Empty;

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public bool Status { get; set; }

        public int CreatedBy { get; set; }
    }
}
