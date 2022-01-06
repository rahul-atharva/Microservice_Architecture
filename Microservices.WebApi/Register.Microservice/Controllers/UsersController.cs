using Core.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Register.Microservice.Data.EFCore;
using Register.Microservice.Entities;
using Register.Microservice.Models;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Register.Microservice.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : MyMDBController<User, EfCoreUserRepository>
    {
        private EfCoreUserRepository _repository;
        public UsersController(EfCoreUserRepository repository
            ) : base(repository)
        {
            _repository = repository;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _repository.Authenticate(model);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(RegisterRequest model)
        {
            _repository.Register(model);
            return Ok(new { message = "Registration successful" });
        }

        [HttpPut("{id}")]
        public async override Task<IActionResult> Put(int id, User model)
        {
            var user = await _repository.Get(id);

            var users = await _repository.GetAll();

            // validate
            if (model.Username != user.Username && users.Any(x => x.Username == model.Username))
                return Ok(new { message = "Username '" + model.Username + "' is already taken" });

            // hash password if it was entered
            if (!string.IsNullOrEmpty(model.Password))
                user.PasswordHash = BCryptNet.HashPassword(model.Password);

            user.Username = model.Username;
            user.Name = model.Name;
            user.EmailAddress = model.EmailAddress;
            user.Status = model.Status;

            await _repository.Update(user);

            return Ok(new { message = "Update successful" });
        }

        [AllowAnonymous]
        [HttpGet("tokenIsValid")]
        public IActionResult TokenIsValid()
        {
#pragma warning disable CS8604 // Possible null reference argument.
            var userId = _repository.TokenIsValid(token: Request.Headers["x-auth-token"].FirstOrDefault());
#pragma warning restore CS8604 // Possible null reference argument.
            return Ok(new { message = userId });
        }
       
    }
}
