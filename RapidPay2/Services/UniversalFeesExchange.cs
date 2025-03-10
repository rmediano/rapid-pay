namespace RapidPay2.Services;

public class UniversalFeesExchange
{
    private readonly Timer _timer;
    private decimal _currentFeeValue = 1.0m;

    private UniversalFeesExchange()
    {
        Console.WriteLine("UniversalFeesExchange started");
        _timer = new Timer(UpdateFeePrice, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        // _timer = new Timer(UpdateFeeValue, null, TimeSpan.Zero, TimeSpan.FromHours(1));
    }

    private static readonly UniversalFeesExchange _instance = new ();
    public static UniversalFeesExchange Instance => _instance;

    public decimal GetCurrentFeePrice()
    {
        return _currentFeeValue;
    }

    private void UpdateFeePrice(object? state)
    {
        var random = new Random();
        decimal newDecimal;
        do
        {
            newDecimal = (decimal) random.NextDouble() * 2;
        } while (newDecimal == 0);
        _currentFeeValue *= newDecimal;
        Console.WriteLine("New fee value: " + _currentFeeValue);
    }
}

// public class UniversalFeesExchange : IUniversalFeesExchange
// {
//     private readonly Timer _timer;
//     private decimal _currentFeeValue;
//
//     public UniversalFeesExchange()
//     {
//         _currentFeeValue = 1.0m;
//         _timer = new Timer(UpdateFeePrice, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
//         // _timer = new Timer(UpdateFeeValue, null, TimeSpan.Zero, TimeSpan.FromHours(1));
//     }
//
//     public decimal GetCurrentFeePrice()
//     {
//         return _currentFeeValue;
//     }
//
//     private void UpdateFeePrice(object? state)
//     {
//         var random = new Random();
//         decimal newDecimal;
//         do
//         {
//             newDecimal = (decimal) random.NextDouble() * 2;
//         } while (newDecimal == 0);
//         _currentFeeValue *= newDecimal;
//         Console.WriteLine("New fee value: " + _currentFeeValue);
//     }
// }