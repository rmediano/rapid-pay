namespace RapidPay2.DTOs;

public record PaymentReceiptResponse
{
    public decimal SubTotal { get; init; }
    public decimal Fee { get; init; }
    public decimal Total { get; init; }
}
