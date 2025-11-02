using HelseFlow_Backend.Domain.Entities;

namespace HelseFlow_Backend.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}
