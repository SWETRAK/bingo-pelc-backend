using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using BingoPelc.Configs;
using BingoPelc.Entities;
using BingoPelc.Exceptions;
using BingoPelc.Models;
using BingoPelc.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BingoPelc.Services;

public class AuthService: IAuthService
{
    private readonly ILogger<AuthService> _logger;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly AuthenticationSettings _authenticationSettings;
    private readonly DomainContextDb _dbContext;
    private readonly IMapper _mapper;

    public AuthService(
        ILogger<AuthService> logger, 
        IPasswordHasher<User> passwordHasher,
        AuthenticationSettings authenticationSettings,
        DomainContextDb dbContext,
        IMapper mapper
        )
    {
        _logger = logger;
        _passwordHasher = passwordHasher;
        _authenticationSettings = authenticationSettings;
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<(UserInfoDto, string)> GetUserInfo(string guideId)
    {
        if (!Guid.TryParse(guideId, out var guideIdGuid)) throw new IncorrectGuidException();

        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == guideIdGuid);
        
        if (user is null) throw new UserNotFoundException(guideIdGuid.ToString());
        
        var token = GenerateJwtToken(user);
        var userInfoDto = _mapper.Map<UserInfoDto>(user);

        return (userInfoDto, token);
    }
    
    public async Task<(UserInfoDto, string)> LoginUser(LoginUserWithPasswordDto userDto)
    {
        var user = await _dbContext
            .Users
            .FirstOrDefaultAsync(u => u.Email == userDto.Email);

        if (user is null) throw new InvalidLoginDataException(userDto.Email);

        var result = _passwordHasher.VerifyHashedPassword(user, user.HashedPassword, userDto.Password);
        if (result == PasswordVerificationResult.Failed) throw new InvalidLoginDataException(userDto.Email);
        
        var userInfoDto = _mapper.Map<UserInfoDto>(user);
        var token = GenerateJwtToken(user);
        _logger.LogInformation("User created with {UserEmail} at {S}", user.Email, DateTime.Now.ToString("yyyy-MMMM-dd h:mm:ss tt zz"));
        return (userInfoDto, token);
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, $"{user.Nickname}"),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

        var token = new JwtSecurityToken(
            _authenticationSettings.JwtIssuer,
            _authenticationSettings.JwtIssuer,
            claims,
            expires: expires,
            signingCredentials: cred
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }
}