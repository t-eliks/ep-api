using System.Text;
using DataAccess;
using DataAccess.Entities.User;
using DataAccess.Models;
using FluentValidation.AspNetCore;
using Logic;
using Logic.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Security;
using Services;
using Services.Common;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetEnvironmentVariable("ConnectionString");
            string origin = Configuration.GetEnvironmentVariable("Origin");
            string secret = Configuration.GetEnvironmentVariable("Secret");

            services.AddCors(o =>
            {
                o.AddPolicy("AllowOrigins", builder =>
                {
                    builder.WithOrigins(origin)
                        .AllowCredentials()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            services.AddDbContext<Repository>(o => o.UseNpgsql(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole<int>>(o =>
            {
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = true;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequireUppercase = true;
                o.Password.RequiredLength = 8;
            }).AddEntityFrameworkStores<Repository>().AddDefaultTokenProviders();

            services.RegisterCommands();
            services.RegisterServices();
            services.ConfigureJwtAuthentication(Encoding.ASCII.GetBytes(secret));

            services.AddSignalR();

            services.AddControllers()
                .AddFluentValidation(o => o.RegisterValidatorsFromAssemblyContaining<LoginViewModel>())
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors("AllowOrigins");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<DiscussionHub>("/hubs/discussion");
            });

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var repository = serviceScope.ServiceProvider.GetService<Repository>();
                
                repository.SeedDatabase();
            }
        }
    }
}
