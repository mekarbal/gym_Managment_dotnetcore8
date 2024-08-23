using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using GymManagment.Exceptions;
using GymManagment.Models;
using GymManagment.Models.DTOs;
using GymManagment.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;


namespace GymManagment.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController:ControllerBase
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;
    
    public AuthController(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }
    
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDTO loginDto)
    {
        try
        {
            var user = _userService.Authenticate(loginDto);
            if (user == null)
                return Unauthorized();

            var token = GenerateJwtToken(user);
            var validUserObject = new
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Image = user.Image
            };
            return Ok(new { Message = "Logged In Successfully", Token = token, User = validUserObject });
        }
        catch (Exception e)
        {
            throw new ApiException((int)HttpStatusCode.InternalServerError, e.Message);
        }
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

}