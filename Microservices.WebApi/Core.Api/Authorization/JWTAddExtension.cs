using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace Core.Api.Authorization
{
    /// <summary>
    /// Extension class for JWT
    /// </summary>
    public static class JWTAddExtension
    {
        /// <summary>
        /// Create JWT Token Validation Mechanism
        /// </summary>
        /// <param name="services"></param>
        /// <param name="builder"></param>
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration _configuration)
        {
            // Hard Coded file should be ENVironment Specific
            var audienceConfig = _configuration.GetSection("Audience");

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(audienceConfig["Secret"]));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = audienceConfig["Iss"],
                ValidateAudience = true,
                ValidAudience = audienceConfig["Aud"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
            };

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = "WebAPIKey";
            })
            .AddJwtBearer("WebAPIKey", x =>
            {
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = tokenValidationParameters;
            });

            //services
            //    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options =>
            //    {
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuer = true, // validate the server
            //            ValidateAudience = true, // Validate the recipient of token is authorized to receive
            //            ValidateLifetime = true, // Check if token is not expired and the signing key of the issuer is valid 
            //            ValidateIssuerSigningKey = true, // Validate signature of the token 

            //            //Issuer and audience values are same as defined in generating Token
            //            ValidIssuer = _config.GetSection("Jwt")["Issuer"].ToString(), // stored in appsetting file
            //            ValidAudience = _config["Jwt:Issuer"], // stored in appsetting file
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])) // stored in appsetting file
            //        };
            //    });
        }
    }
}
