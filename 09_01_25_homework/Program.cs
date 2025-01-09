using _09_01_25_homework.Data;
using _09_01_25_homework.Interfaces.Repository;
using _09_01_25_homework.Interfaces.Services;
using _09_01_25_homework.Repository;
using _09_01_25_homework.Services;
using Microsoft.EntityFrameworkCore;

namespace _09_01_25_homework
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            builder.Services.AddControllers(); 

            builder.Services.AddDbContext<AppDbContext>(options =>
               options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
           );

            // ������������ ����������� � ������ (Scoped � ����� ����������� �������)
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();

            var app = builder.Build();

            app.UseCors("AllowAll");

            // ��� ������������� �������� �������� �/��� �������� ���� ������
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();
                // ��� db.Database.Migrate(); � ���� ���������� ��������
            }
 
            // �������� ��������� ����������� ������
            app.UseStaticFiles();

            app.MapGet("/", async context =>
            {
                context.Response.ContentType = "text/html";
                await context.Response.SendFileAsync("wwwroot/index.html");
            });

            app.MapControllers();

            app.Run();
        }
    }
}
