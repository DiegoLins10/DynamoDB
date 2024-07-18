using Amazon.DynamoDBv2.DataModel;
using DynamoDB.Data;
using DynamoDB.Entities;


namespace DynamoDB.Repositories
{
    public class ProductRepository
    {
        private readonly DynamoDBContext _context;

        public ProductRepository(DynamoDbContext dbContext)
        {
            _context = dbContext.Context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var conditions = new List<ScanCondition>();
            return await _context.ScanAsync<Product>(conditions).GetRemainingAsync();
        }

        public async Task<Product> GetByIdAsync(string id)
        {
            return await _context.LoadAsync<Product>(id);
        }

        public async Task SaveAsync(Product product)
        {
            await _context.SaveAsync(product);
        }

        public async Task DeleteAsync(string id)
        {
            await _context.DeleteAsync<Product>(id);
        }
    }
}