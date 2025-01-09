using _09_01_25_homework.Model;
using CSharpFunctionalExtensions;
using System.Linq.Expressions;

namespace _09_01_25_homework.Interfaces
{
    public interface IRepository<T> where T : class
    {

        public Task<Result<List<T>>> GetListAsync(
              Expression<Func<T, bool>>? filter = null,
              Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
              int? pageNumber = null,
              int? pageSize = null);
        

        public Task<Result<T>> AddAsync(T item);

        public Task<Result<T?>> GetByIdAsync(int id);
        public Task<Result> DeleteAsync(T item);
    }
}
