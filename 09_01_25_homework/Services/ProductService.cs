using _09_01_25_homework.Data;
using _09_01_25_homework.Interfaces.Services;
using _09_01_25_homework.Model;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Linq;
using System.Xml;
using _09_01_25_homework.Interfaces.Repository;

namespace _09_01_25_homework.Services
{
    public class ProductService : IProductService
    {
        private IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository) {
            _productRepository = productRepository;
        }

        public async Task<Result<Product>> AddAsync(Product item)
        {
            var result = await _productRepository.ExistsByNameAsync(item.Name);

            if (result.IsFailure) return Result.Failure<Product>(result.Error);

            if (result.Value) return Result.Failure<Product>("Имя занято!");

            return await _productRepository.AddAsync(item);
        }

        public Task<Result<List<Product>>> GetListAsync()
        {
            return _productRepository.GetListAsync();
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var findResult = await _productRepository.GetByIdAsync(id);

            if(findResult.IsFailure) return findResult;

            if (findResult.Value == null) return findResult;

            var delResult = await _productRepository.DeleteAsync(findResult.Value);

            if (delResult.IsFailure) return delResult; 

            return Result.Success();
        }
    }
}
