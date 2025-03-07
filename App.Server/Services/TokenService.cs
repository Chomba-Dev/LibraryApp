using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class TokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(string username, List<string> roles)
    {
        // Get JWT settings from configuration
        var jwtSettings = _configuration.GetSection("Jwt");

        // Validate JWT settings
        var key = jwtSettings["Key"];
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var expireMinutes = jwtSettings["ExpireMinutes"];

        if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(expireMinutes))
        {
            throw new ArgumentNullException("JWT configuration is missing or invalid in appsettings.json.");
        }

        // Ensure the key is at least 32 bytes (256 bits)
        if (Encoding.ASCII.GetByteCount(key) < 32)
        {
            throw new ArgumentException("JWT key must be at least 32 bytes (256 bits) long.");
        }

        // Parse expiration time
        if (!double.TryParse(expireMinutes, out var expireMinutesValue))
        {
            throw new ArgumentException("ExpireMinutes must be a valid number.");
        }

        // Create security key and signing credentials
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        // Create claims for the token
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username) // Add username to the token
        };

        // Add roles to the token
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // Create token descriptor
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expireMinutesValue),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = credentials
        };

        // Generate and return the token
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}