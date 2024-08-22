using DealmateApi.Domain.Aggregates;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DealmateApi.Service.Authentication;

public class JWTToken
{
    public static string GenerateJWTToken(Employee employee, IConfiguration configuration)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var employeeClaims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier,employee.Id.ToString()),
            new Claim(ClaimTypes.Email,employee.Email),
            new Claim(ClaimTypes.Name,employee.Name),
            new Claim(ClaimTypes.Role,employee.RoleId.ToString()!)
        };
        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: employeeClaims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
