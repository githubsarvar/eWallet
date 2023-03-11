using eWallet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eWallet.Controllers;

[Authorize(Roles =Role.User)]
[Route("api/[controller]")]
[ApiController]
public class WalletController : Controller
{
    
}
