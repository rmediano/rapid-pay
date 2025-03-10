using System.ComponentModel.DataAnnotations.Schema;

namespace RapidPay2.Infrastructure.SQLServer.Entities;

public class Card
{
    public int CardId { get; set; }
    public string User { get; set; }
    public string CardNumber { get; set; }

    [Column(TypeName = "decimal(28,10)")]
    public decimal Balance { get; set; }
}