using Microsoft.EntityFrameworkCore;
using Card = RapidPay2.Domain.Card;

namespace RapidPay2.Infrastructure.SQLServer;

public class CardsRepository(CardsContext cardsContext) : ICardsRepository
{
    public async Task<Card?> GetCardAsync(string user, string cardNumber)
    {
        var card = await cardsContext.Cards.AsNoTracking().FirstOrDefaultAsync(card => card.User == user && card.CardNumber == cardNumber);

        return card is null
            ? null
            : new Card
            {
                Id = card.CardId,
                CardNumber = card.CardNumber,
                Balance = card.Balance,
            };
    }

    public async Task StoreCardAsync(string user, Card card)
    {
        var cardEntity = new Entities.Card
        {
            User = user,
            CardNumber = card.CardNumber!,
            Balance = card.Balance
        };
        cardsContext.Add(cardEntity);
        await cardsContext.SaveChangesAsync();
    }

    public async Task UpdateCardAsync(string user, Card card)
    {
        var cardEntity = new Entities.Card
        {
            CardId = card.Id,
            User = user,
            CardNumber = card.CardNumber!,
            Balance = card.Balance
        };
        cardsContext.Update(cardEntity);
        await cardsContext.SaveChangesAsync();
    }
}
