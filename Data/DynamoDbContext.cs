using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace DynamoDB.Data
{
     public class DynamoDbContext
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private readonly DynamoDBContext _context;

        public DynamoDbContext(IAmazonDynamoDB dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
            _context = new DynamoDBContext(_dynamoDbClient);
        }

        public DynamoDBContext Context => _context;
    }
}