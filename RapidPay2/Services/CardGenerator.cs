using RapidPay2.Domain;

namespace RapidPay2.Services;

public class CardGenerator : ICardGenerator
{
    private const string Bin = "426580";
    private const int CardNumberLength = 15;
    private const decimal StartingBalance = 1000;

    public Card CreateNewCard()
    {
        var cardNumber = GenerateCreditCardNumber();
        return new Card
        {
            CardNumber = cardNumber,
            Balance = StartingBalance
        };
    }

    private static string GenerateCreditCardNumber()
    {
        Random random = new Random();
        string cardNumber = Bin;

        while (cardNumber.Length < CardNumberLength - 1)
        {
            cardNumber += random.Next(0, 10).ToString();
        }

        int checkDigit = GetLuhnCheckDigit(cardNumber);
        cardNumber += checkDigit.ToString();

        return cardNumber;
    }

    private static int GetLuhnCheckDigit(string number)
    {
        int sum = number.Reverse()
            .Select((c, i) => (c - '0') * (i % 2 == 0 ? 2 : 1))
            .Sum(d => d > 9 ? d - 9 : d);

        return (10 - (sum % 10)) % 10;
    }
}