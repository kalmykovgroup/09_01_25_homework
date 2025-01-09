using _09_01_25_homework.Model;
using CSharpFunctionalExtensions;

namespace _09_01_25_homework.Interfaces.Repository
{
    public interface IProductRepository : IRepository<Product>
    {
        public Task<Result<bool>> ExistsByNameAsync(string name);
    }
}
