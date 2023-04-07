using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Profiles;
using ECommerce.Api.Products.Providers;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Products.Tests
{
    public class ProductsServiceTest : IDisposable
    {
        ProductsDbContext dbContext = new ProductsDbContext(new DbContextOptionsBuilder<ProductsDbContext>().UseInMemoryDatabase(nameof(GetProductReturnsAllProductsTest)).Options);
        public ProductsServiceTest()
        {
            if (dbContext.Products == null)
                CreateProducts(dbContext);
        }


        [Fact]
        public async void GetProductReturnsAllProductsTest()
        {

            var productsprofile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productsprofile));
            var mapper = new Mapper(configuration);
            var productsProvider = new ProductsProvider(dbContext, null, mapper);

            var product = await productsProvider.GetProductsAsync();
            Assert.True(product.IsSuccess);
            Assert.True(product.Products.Any());
            Assert.Null(product.ErrorMessage);
        }

        [Fact]
        public async void GetProductReturnsProductUsingValidIdTest()
        {
            //var options = new DbContextOptionsBuilder<ProductsDbContext>().UseInMemoryDatabase(nameof(GetProductReturnsAllProductsTest)).Options;
            //var dbContext = new ProductsDbContext(options);
            //CreateProducts(dbContext);
            var productsprofile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productsprofile));
            var mapper = new Mapper(configuration);
            var productsProvider = new ProductsProvider(dbContext, null, mapper);

            var product = await productsProvider.GetProductAsync(1);
            Assert.True(product.IsSuccess);
            Assert.NotNull(product.Product);
            Assert.True(product.Product.Id == 1);
            Assert.Null(product.ErrorMessage);
        }

        [Fact]
        public async void GetProductReturnsProductUsingInvalidValidIdTest()
        {
            //var options = new DbContextOptionsBuilder<ProductsDbContext>().UseInMemoryDatabase(nameof(GetProductReturnsAllProductsTest)).Options;
            //var dbContext = new ProductsDbContext(options);
            //CreateProducts(dbContext);
            var productsprofile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productsprofile));
            var mapper = new Mapper(configuration);
            var productsProvider = new ProductsProvider(dbContext, null, mapper);

            var product = await productsProvider.GetProductAsync(-1);
            Assert.False(product.IsSuccess);
            Assert.Null(product.Product);
            Assert.NotNull(product.ErrorMessage);
        }

        private void CreateProducts(ProductsDbContext productsDbContext)
        {
            for (int i = 1; i < 10; i++)
            {
                productsDbContext.Products.Add(new Product()
                {
                    Id = i,
                    Name = Guid.NewGuid().ToString(),
                    Inventory = i + 10,
                    Price = (decimal)(i * 3.14)
                });
            }
            productsDbContext.SaveChanges();
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}