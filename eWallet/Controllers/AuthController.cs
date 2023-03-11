using eWallet.DTOs;
using eWallet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace eWallet.Controllers;


[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration, UserManager<ApplicationUser> userManager)
    {
        this._userManager = userManager;
        this._configuration = configuration;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO  loginRequest)
    {
        var user = await _userManager.FindByNameAsync(loginRequest.UserName);
        if (user == null)        
            return Unauthorized("Invalid email or password");
                
        if (!_userManager.CheckPasswordAsync(user, loginRequest.Password).GetAwaiter().GetResult())            
            return Unauthorized("Invalid email or password");

        var token = GenerateJwtToken(user);
        return Ok(new { Token = token });
    }
    private string GenerateJwtToken(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var creds = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Vars.TheSecretKey)), SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["AccessConfiguration:ExpireDays"]));
        var token = new JwtSecurityToken(
            _configuration["AccessConfiguration:Issuer"],
            _configuration["AccessConfiguration:Audience"],
            claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
