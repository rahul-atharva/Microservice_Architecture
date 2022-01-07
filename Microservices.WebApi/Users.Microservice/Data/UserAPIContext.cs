using Microsoft.EntityFrameworkCore;
using Users.Microservice.Entities;

namespace Users.Microservice.Data
{
    public class UserAPIContext : DbContext
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public UserAPIContext(DbContextOptions<UserAPIContext> options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
