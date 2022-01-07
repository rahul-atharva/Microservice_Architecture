using Core.Api.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Users.Microservice.Data;
using Users.Microservice.Data.EFCore;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Users.Microservice
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private IHostingEnvironment HostingEnvironment { get; set; }

        public Startup(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            HostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        // add services to the DI container
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddCors();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Users API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                }, new List<string>()
            }
        });
            });

            // File should be ENVironment Specific
            IConfigurationBuilder builder = new ConfigurationBuilder()
                                    .AddJsonFile("appsettings.json") // This file will be overridden by below next line 
                                    .AddJsonFile($"appsettings.{HostingEnvironment.EnvironmentName}.json", optional: true); // Read ENV value for appsetting

            services.AddJwtAuthentication(HostingEnvironment, builder); // Extension for JWT

            services.AddDbContext<UserAPIContext>(options =>
                   options.UseSqlServer(_configuration.GetConnectionString("UsersApiDatabase")));

            // configure DI for application services
            services.AddScoped<EfCoreUserRepository>();
            //services.AddScoped<IUserService, UserService>();
        }

        // configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            // generated swagger json and swagger ui middleware
            app.UseSwagger();
            app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "Users API"));


            app.UseRouting();

            app.UseAuthorization();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            // global error handler
            //app.UseMiddleware<ErrorHandlerMiddleware>();

            // custom jwt auth middleware
            //app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(x => x.MapControllers());
        }
    }
}
