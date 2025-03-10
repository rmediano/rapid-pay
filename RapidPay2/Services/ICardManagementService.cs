using RapidPay2.DTOs;

namespace RapidPay2.Services;

public interface ICardManagementService
{
    Task<CardBalanceResponse?> GetCardBalanceAsync(string user, string cardNumber);
    Task<CardBalanceResponse> CreateCardAsync(string user);
    Task<PaymentReceiptResponse?> ProcessPaymentAsync(string user, string cardNumber, decimal paymentAmount);
}