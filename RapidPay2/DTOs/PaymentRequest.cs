using System.ComponentModel.DataAnnotations;

namespace RapidPay2.DTOs;

public record PaymentRequest
{
    [Required]
    public decimal Amount { get; init; }
}