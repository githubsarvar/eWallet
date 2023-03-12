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
public class AccountsController : ControllerBase
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public AccountsController(IConfiguration configuration, UserManager<ApplicationUser> userManager)
    {
        this._userManager = userManager;
        this._configuration = configuration;
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest  loginRequest)
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
        IEnumerable<string> roles = _userManager.GetRolesAsync(user).GetAwaiter().GetResult();
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var creds = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Vars.TheSecretKey)), SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["AccessConfiguration:ExpireDays"]));
        var token = new JwtSecurityToken(
                        issuer: _configuration["AccessConfiguration:Issuer"],
                        audience: _configuration["AccessConfiguration:Audience"],
                        expires: expires,
                        claims: claims,
                        signingCredentials: creds
                    );
        

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest createUser)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {           

            var user = new ApplicationUser
            {
                UserName = createUser.UserName,
                Email = createUser.Email,
                FirstName = createUser.FirstName,
                LastName = createUser.LastName,
                PhoneNumber = createUser.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, createUser.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok();
        }
        catch (Exception)
        {
            return BadRequest();
        }
        
    }

}
