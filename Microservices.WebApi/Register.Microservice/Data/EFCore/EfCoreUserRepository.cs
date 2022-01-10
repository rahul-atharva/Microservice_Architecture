using BCryptNet = BCrypt.Net.BCrypt;
using DataAccess.Api.EFCore;
using Register.Microservice.Entities;
using Register.Microservice.Models;
using Register.Microservice.Helpers;
using AutoMapper;
using Microsoft.Extensions.Options;

namespace Register.Microservice.Data.EFCore
{
    public class EfCoreUserRepository
    {
        private IConfiguration _config;
        private UserAPIContext _context;
        private readonly IMapper _mapper;
        public EfCoreUserRepository(UserAPIContext context, IMapper mapper, IConfiguration config)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model, IOptions<Audience> _settings)
        {
            var user = _context.Users.SingleOrDefault(x => x.Username == model.Username);

            // validate
            if (user == null || !BCryptNet.Verify(model.Password, user.PasswordHash))
                throw new AppException("Username or password is incorrect");

            JWTHandler jWTHandler = new JWTHandler();

            // authentication successful
            var response = _mapper.Map<AuthenticateResponse>(user);
            var JWTResponse  = jWTHandler.GenerateJSONWebToken(user, _settings);
            response.access_token = JWTResponse.access_token;
            response.expires_in = JWTResponse.expires_in;

            return response;
        }

        public void Register(RegisterRequest model)
        {
            // validate
            if (_context.Users.Any(x => x.Username == model.Username))
                throw new AppException("Username '" + model.Username + "' is already taken");

            // map model to new user object
            var user = _mapper.Map<User>(model);

            // hash password
            user.PasswordHash = BCryptNet.HashPassword(model.Password);

            // save user
            _context.Users.Add(user);
            _context.SaveChanges();
            //await base.Add(user);
        }

        public int TokenIsValid(string token)
        {
            var userId = 0;// _jwtUtils.ValidateToken(token);
            return userId != null ? (int)userId : 0;
        }

    }
}
