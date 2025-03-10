using System.ComponentModel.DataAnnotations;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RapidPay2.DTOs;
using RapidPay2.Extensions;
using RapidPay2.Services;

namespace RapidPay2.Controllers;

[ApiController]
[Authorize]
[ApiVersion("1.0")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CardsController(ICardManagementService cardManagementService) : ControllerBase
{
    // GET
    [HttpGet("{cardNumber}")]
    [ProducesResponseType(typeof(CardBalanceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBalance(string cardNumber)
    {
        var cardBalance = await cardManagementService.GetCardBalanceAsync(User.GetUsername(), cardNumber);

        return cardBalance is null ? NotFound() : Ok(cardBalance);
    }

    // POST
    [HttpPost]
    [ProducesResponseType(typeof(CardBalanceResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateCard()
    {
        var cardBalanceResponse = await cardManagementService.CreateCardAsync(User.GetUsername());
        return Ok(cardBalanceResponse);
    }

    [HttpPatch("{cardNumber}")]
    [ProducesResponseType(typeof(PaymentReceiptResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Pay(string cardNumber, [FromBody] PaymentRequest paymentRequest)
    {
        var paymentReceipt = await cardManagementService.ProcessPaymentAsync(User.GetUsername(), cardNumber, paymentRequest.Amount);
        return paymentReceipt is null ? NotFound() : Ok(paymentReceipt);
    }
}