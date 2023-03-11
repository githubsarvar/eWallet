using AutoMapper;
using eWallet.DTOs;
using eWallet.Infrastructure;
using eWallet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eWallet.Controllers;

[Authorize(Roles =Role.User)]
[Route("api/[controller]")]
[ApiController]
public class WalletController : Controller
{
    private readonly IUserProvider _userProvider;
    private readonly WalletDbContext _dbContext;
    private readonly IMapper _mapper;

    public WalletController(IUserProvider userProvider, WalletDbContext dbContext, IMapper mapper)
    {
        _userProvider = userProvider;
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("UserWaltets")]
    public async Task<IActionResult> GetUserWaltets()
    {
        try
        {
            var wallets = _dbContext.Wallets
            .Where(x => x.OwnerId == _userProvider.CurrentUser.Id)
            .AsNoTracking()
            .ToList();

            return Ok(_mapper.Map<List<WalletDTO>>(wallets));
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    
}
