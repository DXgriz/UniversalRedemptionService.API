using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UniversalRedemptionService.API.Data;
using UniversalRedemptionService.API.DTOs;
using UniversalRedemptionService.API.Models;

namespace UniversalRedemptionService.API.Services
{
    public class AuthService(SLUCRSDbContext context, IConfiguration config)
    {
        public async Task<string> RegisterAsync(RegisterRequestDto dto)
        {
            if (await context.Users.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("User already exists");

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            var wallet = new Wallet { User = user };

            context.Users.Add(user);
            context.Wallets.Add(wallet);

            await context.SaveChangesAsync();

            return GenerateJwtToken(user);
        }

        public async Task<string> LoginAsync(LoginRequestDto dto)
        {
            var user = await context.Users.Include(u => u.Wallet).FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Invalid credentials");

            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(User user)
        {
            var jwt = config.GetSection("Jwt");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), //for rate limiting & identity
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role ?? "User") //RBAC
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["Key"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(jwt["ExpiresInMinutes"]!)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
