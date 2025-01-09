using _09_01_25_homework.Data;
using _09_01_25_homework.Interfaces.Repository;
using _09_01_25_homework.Model;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace _09_01_25_homework.Repository
{
    public class ProductRepository : IProductRepository
    {

        private AppDbContext _appDbContext;

        public ProductRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        public async Task<Result<bool>> ExistsByNameAsync(string name)
        {
            try
            {
                return await _appDbContext.Products.AnyAsync(p => p.Name == name);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>($"Произошла ошибка: {ex.Message}");
            }
        }

        public async Task<Result<Product>> AddAsync(Product product)
        {
            try
            {
                _appDbContext.Products.Add(product);
                var result = await _appDbContext.SaveChangesAsync();

                // Проверяем, были ли фактические изменения в БД
                if (result > 0)
                    return Result.Success(product);
                else
                    return Result.Failure<Product>("Запись не была добавлена. Возможно, не было изменений или произошла ошибка.");
            }
            catch (DbUpdateException ex)
            {
                return Result.Failure<Product>($"Произошла ошибка при обновлении БД: {ex.Message}");
            }
            catch (Exception ex)
            {
                return Result.Failure<Product>($"Произошла ошибка: {ex.Message}");
            }
        }

        public async Task<Result> DeleteAsync(Product product)
        {
            try
            {
                _appDbContext.Products.Remove(product);
                await _appDbContext.SaveChangesAsync();
                return Result.Success();
            }
            catch (InvalidOperationException ex)
            {
                // Логирование ошибок связанных с LINQ или конфигурацией модели
                return Result.Failure($"Ошибка выполнения запроса: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Логирование всех остальных ошибок 
                return Result.Failure($"Произошла неожиданная ошибка: {ex.Message}");
            }
        }

        public async Task<Result<Product?>> GetByIdAsync(int id)
        {
            try
            {
                return await _appDbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
                 
            }
            catch (InvalidOperationException ex)
            {
                // Логирование ошибок связанных с LINQ или конфигурацией модели
                return Result.Failure<Product?>($"Ошибка выполнения запроса: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Логирование всех остальных ошибок 
                return Result.Failure<Product?>($"Произошла неожиданная ошибка: {ex.Message}");
            }
        }

        public async Task<Result<List<Product>>> GetListAsync(
                Expression<Func<Product, bool>>? filter = null,
                Func<IQueryable<Product>, IOrderedQueryable<Product>>? orderBy = null,
                int? pageNumber = null,
                int? pageSize = null)
        {
            try
            {
                IQueryable<Product> query = _appDbContext.Set<Product>();

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                if (orderBy != null)
                {
                    query = orderBy(query);
                }

                if (pageNumber.HasValue && pageSize.HasValue)
                {
                    if (pageNumber <= 0 || pageSize <= 0)
                    {
                        Result.Failure<List<Product>>("pageNumber и pageSize должны быть больше 0");
                    }

                    int skip = (pageNumber.Value - 1) * pageSize.Value;
                    query = query.Skip(skip).Take(pageSize.Value);
                }

                return await query.ToListAsync();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                // Логирование ошибки пагинации
                return Result.Failure<List<Product>>($"Ошибка пагинации: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                // Логирование ошибок связанных с LINQ или конфигурацией модели
                return Result.Failure<List<Product>>($"Ошибка выполнения запроса: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Логирование всех остальных ошибок 
                return Result.Failure<List<Product>>($"Произошла неожиданная ошибка: {ex.Message}");
            }
        }
    }
}
