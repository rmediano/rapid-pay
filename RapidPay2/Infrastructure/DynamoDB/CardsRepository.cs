using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace RapidPay2.Infrastructure.DynamoDB;

public class CardsRepository(IDynamoDBContext dbContext) : ICardsRepository
{
    private DynamoDBOperationConfig DynamoConfig { get; } = new()
    {
        OverrideTableName = DynamoDBInit.TableName
    };

    private async Task<IEnumerable<Entities.Card>> QueryByPartitionAndSortKeyAsync(string user, string cardNumber)
    {
        var items = await dbContext
            .QueryAsync<Entities.Card>(user, QueryOperator.Equal, [cardNumber], DynamoConfig)
            .GetRemainingAsync();

        return items ?? [];
    }

    public async Task<Domain.Card?> GetCardAsync(string user, string cardNumber)
    {
        var entityCards = await QueryByPartitionAndSortKeyAsync(user, cardNumber);
        var domainCard = entityCards.Select(entityCard => new Domain.Card { CardNumber = entityCard.CardNumber, Balance = entityCard.Balance }).FirstOrDefault();

        return domainCard;
    }

    private Task StoreItemAsync(string user, Domain.Card card)
    {
        var entity = new Entities.Card
        {
            Pk = user,
            Sk = card.CardNumber,
            CardNumber = card.CardNumber,
            Balance = card.Balance
        };
        return dbContext.SaveAsync(entity, DynamoConfig);
    }

    public Task StoreCardAsync(string user, Domain.Card card)
    {
        return StoreItemAsync(user, card);
    }

    public Task UpdateCardAsync(string user, Domain.Card card)
    {
        return StoreItemAsync(user, card);
    }
}