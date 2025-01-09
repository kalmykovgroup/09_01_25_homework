using _09_01_25_homework.Model;
using CSharpFunctionalExtensions;
using System.Linq.Expressions;

namespace _09_01_25_homework.Interfaces.Services
{
    public interface IProductService 
    {
        public Task<Result<List<Product>>> GetListAsync();


        public Task<Result<Product>> AddAsync(Product item);

        public Task<Result> DeleteAsync(int id);
    }
}
