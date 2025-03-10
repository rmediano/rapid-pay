using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace RapidPay2.Infrastructure.SQLServer;

public class SqlServerInit(IConfiguration config, IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var connectionString = config["SQLSERVER_CONNECTION_STRING"]!;

        await using var connection = new SqlConnection(connectionString);
        var checkDbQuery = "IF DB_ID('RapidPay') IS NULL CREATE DATABASE RapidPay";

        var command = new SqlCommand(checkDbQuery, connection);

        try
        {
            await connection.OpenAsync(cancellationToken);
            await command.ExecuteNonQueryAsync(cancellationToken);
            Console.WriteLine("Database checked and created if it did not exist.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred creating the RapidPay db: {ex.Message}");
        }

        using var scope = serviceProvider.CreateScope();
        var cardsContext = scope.ServiceProvider.GetRequiredService<CardsContext>();
        await cardsContext.Database.MigrateAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}