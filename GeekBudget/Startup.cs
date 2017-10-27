using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography.X509Certificates;
using System.IdentityModel.Tokens.Jwt;
using GeekBudget.Middleware;
using Microsoft.EntityFrameworkCore;
using GeekBudget.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Text;
using Newtonsoft.Json;

namespace GeekBudget
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //register cors
            services.AddCors();

            // Add framework services.
            services.AddMvc().AddJsonOptions(options => {
                //This option enables loop-reference-serialization problems. For example if we Parent-Child relationship with navigation probperties on both, serializing it to JSON won't throw an exception
                //options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            //Dependency injection

            //register dbcontext as a singleton
            services.AddDbContext<GeekBudgetContext>(
                options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            //register geekcontext
            services.AddTransient<IGeekBudgetContext, GeekBudgetContext>();

            //register contact repo
            services.AddTransient<IUserRepository, UserRepository>();

            //register swagger generator
            //services.AddSwaggerGen()

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            InitializeDatabase(serviceProvider);

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //Authentication
            app.UseMiddleware<UserKeyValidator>();

            //Exception handling
            app.UseMiddleware<ErrorWrapper>();

            //Enable CORS
            app.UseCors(builder => builder
                .AllowAnyOrigin()           //TODO: setup allowed urls
                .AllowAnyMethod()           
                .WithHeaders("user-key", "Content-Type"));  //Allow only authorized cors requests

            app.UseMvc();
        }

        private void InitializeDatabase(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<GeekBudgetContext>().Database.Migrate();
            }
        }
    }
}
