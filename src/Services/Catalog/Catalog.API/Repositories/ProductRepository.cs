using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext context;

        public ProductRepository(ICatalogContext context)
        {
            this.context = context;
        }

        public async Task CreateProduct(Product product)
        {
            await context.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProduct(string id)
        {
            //todo: check how it works
            var result = await context.Products.DeleteOneAsync(p => p.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<Product> GetProduct(string productId)
        {
            return await context.Products.Find(p => p.Id == productId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            FilterDefinition<Product> filterDefinition = Builders<Product>.Filter.Eq(p => p.Category, categoryName);

            return await context.Products.Find(filterDefinition).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string productName)
        {
            FilterDefinition<Product> filterDefinition = Builders<Product>.Filter.ElemMatch(p => p.Name, productName);

            return await context.Products.Find(filterDefinition).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return (await context.Products.FindAsync(p => true)).ToEnumerable();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var result = await context.Products.ReplaceOneAsync(p => p.Id == product.Id, product);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}
