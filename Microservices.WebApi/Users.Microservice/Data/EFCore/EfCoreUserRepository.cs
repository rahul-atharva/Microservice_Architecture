using DataAccess.Api.EFCore;
using Users.Microservice.Entities;

namespace Users.Microservice.Data.EFCore
{
    public class EfCoreUserRepository : EfCoreRepository<User, UserAPIContext>
    {
        private UserAPIContext _context;
        private IConfiguration _config;

        public EfCoreUserRepository(UserAPIContext context, IConfiguration config) : base(context)
        {
            _context = context;
            _config = config;
        }

    }
}
