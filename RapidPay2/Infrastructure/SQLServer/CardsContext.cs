using Microsoft.EntityFrameworkCore;

namespace RapidPay2.Infrastructure.SQLServer;

public class CardsContext(DbContextOptions<CardsContext> options) : DbContext(options)
{
    public DbSet<Entities.Card> Cards { get; set; }
}