using Amazon.DynamoDBv2.DataModel;

namespace RapidPay2.Infrastructure.Entities;

public class Card
{
    [DynamoDBHashKey("PK")]
    public string? Pk { get; set; }

    [DynamoDBRangeKey("SK")]
    public string? Sk { get; set; }

    public string? CardNumber  { get; set; }
    public decimal Balance  { get; set; }
}