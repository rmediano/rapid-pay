using RapidPay2.DTOs;
using RapidPay2.Infrastructure;

namespace RapidPay2.Services;

public class CardManagementService(ICardGenerator cardGenerator, IPaymentFeesModule paymentFeesModule, ICardsRepository cardsRepository) : ICardManagementService
{
    public async Task<CardBalanceResponse?> GetCardBalanceAsync(string user, string cardNumber)
    {
        var card = await cardsRepository.GetCardAsync(user, cardNumber);

        return card is null ? null : new CardBalanceResponse
        {
            CardNumber = card.CardNumber,
            Balance = card.Balance
        };
    }

    public async Task<CardBalanceResponse> CreateCardAsync(string user)
    {
        var card = cardGenerator.CreateNewCard();
        await cardsRepository.StoreCardAsync(user, card);

        return new CardBalanceResponse
        {
            CardNumber = card.CardNumber,
            Balance = card.Balance
        };
    }

    public async Task<PaymentReceiptResponse?> ProcessPaymentAsync(string user, string cardNumber, decimal paymentAmount)
    {
        var currentCard = await cardsRepository.GetCardAsync(user, cardNumber);

        if (currentCard is null) return null;

        var paymentFee = paymentFeesModule.GetPaymentFee();
        var total = paymentAmount + paymentFee;
        var newBalanceValue = currentCard.Balance - total;

        if (newBalanceValue < 0)
        {
            throw new InvalidOperationException("Insufficient funds");
        }

        await cardsRepository.UpdateCardAsync(user, currentCard with
        {
            Balance = newBalanceValue
        });

        return new PaymentReceiptResponse
        {
            SubTotal = paymentAmount,
            Fee = paymentFee,
            Total = total
        };
    }
}
