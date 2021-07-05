using System;
using api.Data;
using api.Helper;
using api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace api
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

            services.AddControllers();
            services.AddScoped<OpenTokService>();
            services.Configure<TokboxSettings>(Configuration.GetSection("TokboxSettings"));
            services.AddDbContext<DataContext>(options =>
           {
               // options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            //    options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
               var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

               string connStr;

               // Depending on if in development or production, use either Heroku-provided
               // connection string, or development connection string from env var.
               if (env == "Development")
               {
                   // Use connection string from file.
                   connStr = Configuration.GetConnectionString("DefaultConnection");
               }
               else
               {
                   // Use connection string provided at runtime by Heroku.
                   var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
                
                   // Parse connection URL to connection string for Npgsql
                   connUrl = connUrl.Replace("postgres://", string.Empty);
                   var pgUserPass = connUrl.Split("@")[0];
                   var pgHostPortDb = connUrl.Split("@")[1];
                   var pgHostPort = pgHostPortDb.Split("/")[0];
                   var pgDb = pgHostPortDb.Split("/")[1];
                   var pgUser = pgUserPass.Split(":")[0];
                   var pgPass = pgUserPass.Split(":")[1];
                   var pgHost = pgHostPort.Split(":")[0];
                   var pgPort = pgHostPort.Split(":")[1];

                   connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};SSL Mode=Require;TrustServerCertificate=True";
               }
               // Whether the connection string came from the local development configuration file
               // or from the environment variable from Heroku, use it to set up your DbContext.
               options.UseNpgsql(connStr);
           });
            services.AddCors();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(x=>x.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithOrigins("http://localhost:4200"));

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
