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

            // –егистрируем репозиторий и сервис (Scoped Ч часто оптимальный вариант)
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();

            var app = builder.Build();

            app.UseCors("AllowAll");

            // ѕри необходимости выполним миграции и/или создадим базу данных
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();
                // или db.Database.Migrate(); Ч если используем миграции
            }

            // Configure the HTTP request pipeline.

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
