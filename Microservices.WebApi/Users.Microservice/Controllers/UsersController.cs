using Core.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Users.Microservice.Data.EFCore;
using Users.Microservice.Entities;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Users.Microservice.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : MyMDBController<User, EfCoreUserRepository>
    {
        private EfCoreUserRepository _repository;


        public UsersController(EfCoreUserRepository repository) : base(repository)
        {
            _repository = repository;

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

    }
}
