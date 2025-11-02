using HelseFlow_Backend.Application.DTOs;
using HelseFlow_Backend.Application.Interfaces;
using HelseFlow_Backend.Domain.Entities;

namespace HelseFlow_Backend.Application.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<LoginResult?> LoginAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);

        if (user == null || !_passwordHasher.VerifyPassword(user.PasswordHash, password))
        {
            return null; // Invalid credentials
        }

        var token = _jwtService.GenerateToken(user);
        var userDto = new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            Age = user.Age,
            Region = user.Region,
            RiskFactors = user.RiskFactors
        };

        return new LoginResult(token, userDto);
    }

    public async Task<RegisterResult?> RegisterAsync(string name, string email, string password, int? age, string? region, List<string>? riskFactors)
    {
        // Check if a user with this email already exists
        var existingUser = await _userRepository.GetByEmailAsync(email);
        if (existingUser != null)
        {
            return null; // Email already registered
        }

        var passwordHash = _passwordHasher.HashPassword(password);
        var newUser = new User(
            Guid.NewGuid(),
            name,
            email,
            passwordHash,
            "patient", // Default to patient role
            age,
            region,
            riskFactors
        );

        await _userRepository.AddAsync(newUser);

        var userDto = new UserDto
        {
            Id = newUser.Id,
            Name = newUser.Name,
            Email = newUser.Email,
            Role = newUser.Role,
            Age = newUser.Age,
            Region = newUser.Region,
            RiskFactors = newUser.RiskFactors
        };

        return new RegisterResult(userDto);
    }

    public async Task<UserDto?> UpdateProfileAsync(Guid userId, string name, int? age, string? region, List<string>? riskFactors)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return null; // User not found
        }

        user.UpdateProfile(name, age, region, riskFactors);
        await _userRepository.UpdateAsync(user);

        var userDto = new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            Age = user.Age,
            Region = user.Region,
            RiskFactors = user.RiskFactors
        };

        return userDto;
    }
}

public record LoginResult(string Token, UserDto User);
public record RegisterResult(UserDto User);
