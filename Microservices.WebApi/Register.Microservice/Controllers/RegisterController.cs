using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Register.Microservice.Data.EFCore;
using Register.Microservice.Helpers;
using Register.Microservice.Models;

namespace Register.Microservice.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        private IOptions<Audience> _settings;
        private EfCoreUserRepository _repository;
        public RegisterController(EfCoreUserRepository repository, IOptions<Audience> settings)
        {
            _repository = repository;
            this._settings = settings;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _repository.Authenticate(model, _settings);
            return Ok(response);
        }

        [HttpPost]
        public IActionResult Post(RegisterRequest model)
        {
            _repository.Register(model);
            return Ok(new { message = "Registration successful" });
        }

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
