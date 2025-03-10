using RapidPay2.Domain;

namespace RapidPay2.Infrastructure;

public interface ICardsRepository
{
    Task<Card?> GetCardAsync(string user, string cardNumber);
    Task StoreCardAsync(string user, Card card);
    Task UpdateCardAsync(string user, Card card);
}