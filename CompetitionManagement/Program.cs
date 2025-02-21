using System.Reflection;
using CompetitionManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CompetitionManagement;

public class Program
{
        public static void Main(string[] args)
        {
                var builder = WebApplication.CreateBuilder(args);

                // Add services to the container.
                builder.Services.AddAuthorization();

                builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
                
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                
                builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
                {
                        options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

                        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
                });
                
                builder.Services.AddEndpointsApiExplorer();
                
                builder.Services.AddControllers();
                
                builder.Services.AddAuthorization();
                builder.Services.AddAuthentication();
                
                
                builder.Services.AddSwaggerGen();

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                        app.UseSwagger();
                        app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                app.UseAuthentication();
                app.UseAuthorization();

                app.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                
                app.Run();
        }
}