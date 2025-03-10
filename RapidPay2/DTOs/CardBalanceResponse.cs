namespace RapidPay2.DTOs;

public record CardBalanceResponse
{
    public string? CardNumber { get; init; }
    public decimal Balance { get; init; }
}
