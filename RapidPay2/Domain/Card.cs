namespace RapidPay2.Domain;

public record Card
{
    public int Id { get; init; }
    public string? CardNumber { get; init; }
    public decimal Balance { get; init; }
}