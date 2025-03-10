using RapidPay2.Domain;

namespace RapidPay2.Services;

public interface ICardGenerator
{
    public Card CreateNewCard();
}