using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

public class DynamoDBInit(IAmazonDynamoDB dynamoDbClient) : IHostedService
{
    public const string TableName = "datastore";

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            var table = await dynamoDbClient.DescribeTableAsync(TableName, cancellationToken);
            Console.WriteLine($"Table status: {table.Table.TableStatus}");
        }
        catch (ResourceNotFoundException)
        {
            Console.WriteLine("Table not found.");
            var createTableRequest = new CreateTableRequest
            {
                TableName = TableName,
                KeySchema = new List<KeySchemaElement>
                {
                    new ()
                    {
                        AttributeName = "PK",
                        KeyType = "HASH" // Partition key
                    },
                    new ()
                    {
                        AttributeName = "SK",
                        KeyType = "RANGE" // String type
                    }
                },
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new ()
                    {
                        AttributeName = "PK",
                        AttributeType = "S" // String type
                    },
                    new ()
                    {
                        AttributeName = "SK",
                        AttributeType = "S" // String type
                    }
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                }
            };

            await dynamoDbClient.CreateTableAsync(createTableRequest, cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}