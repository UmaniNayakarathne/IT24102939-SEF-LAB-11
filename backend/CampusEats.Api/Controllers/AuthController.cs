using CampusEats.Api.Data;
using CampusEats.Api.Dtos;
using CampusEats.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
// --- New Usings for Token Generation ---
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CampusEats.Api.Controllers;

[ApiController]
[Route("api/[controller]")] 
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _cfg;
    
    public AuthController(AppDbContext db, IConfiguration cfg)
    { 
        _db = db; 
        _cfg = cfg; 
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var taken = await _db.Users.AnyAsync(u => u.Email == dto.Email);
        if (taken) return Conflict("Email already used");

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password), 
            Role = "Customer"
        };
        
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        
        return StatusCode(201, new { user.Id, user.Email, user.Role }); 
    }

    // --- NEW: Login Endpoint ---
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        // Find the user by email
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        
        // Verify credentials: check if user is null OR password does not match
        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return Unauthorized("Invalid credentials");
            
        // If valid, return a newly generated JWT token
        return Ok(new { token = CreateToken(user) });
    }

    // --- NEW: Token Builder Method ---
    private string CreateToken(User user)
    {
        var jwt = _cfg.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        // Define claims (payload data inside the token)
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role) // For Role-Based Access Control (RBAC)
        };
        
        // Construct the JWT
        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2), // Token valid for 2 hours
            signingCredentials: creds
        );
        
        // Serialize the token to a string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}