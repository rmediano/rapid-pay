using RapidPay2.DTOs;

namespace RapidPay2.Services;

public interface IAuthService
{
    string GenerateToken(string username);
}