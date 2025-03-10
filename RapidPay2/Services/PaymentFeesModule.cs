namespace RapidPay2.Services;

public class PaymentFeesModule : IPaymentFeesModule
{
    public decimal GetPaymentFee()
    {
        return UniversalFeesExchange.Instance.GetCurrentFeePrice();
    }
}