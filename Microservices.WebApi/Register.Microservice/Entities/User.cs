using DataAccess.Api;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Register.Microservice.Entities
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;

        [NotMapped]
        public string Password { get; set; } = string.Empty;

        [JsonIgnore]
        public string PasswordHash { get; set; } = string.Empty;

        public bool Status { get; set; }

        public int CreatedBy { get; set; }
    }
}
