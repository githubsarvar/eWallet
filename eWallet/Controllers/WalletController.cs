using AutoMapper;
using eWallet.DTOs;
using eWallet.Events;
using eWallet.Infrastructure;
using eWallet.Models;
using eWallet.Rules;
using eWallet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eWallet.Controllers;

[Authorize(Roles = Role.User)]
[Route("api/[controller]")]
[ApiController]
public class WalletController : Controller
{
    private readonly IUserProvider _userProvider;
    private readonly WalletDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IWalletCharge _walletCharge;

    public WalletController(IUserProvider userProvider, WalletDbContext dbContext, IMapper mapper, IWalletCharge walletCharge)
    {
        _userProvider = userProvider;
        _dbContext = dbContext;
        _mapper = mapper;
        _walletCharge = walletCharge;
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

    [HttpGet]
    [Route("CreateWallet")]
    public async Task<IActionResult> CreateWallet()
    {
        try
        {

            Wallet wallet = new Wallet();
            wallet.OwnerId = _userProvider.CurrentUser.Id;  
            wallet.WalletNo = WalletService.GenerateWalletNumber(_userProvider.CurrentUser.Id);
            wallet.Balance = new Money(0m, "TJS");

            _dbContext.Wallets.Add(wallet); 
            _dbContext.SaveChanges();   


            return Ok(_mapper.Map<WalletDTO>(wallet));
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }
    [HttpPost]
    [Route("Recharge")]
    public async Task<IActionResult> WalletRecharge([FromBody] WalletRechargeRequest walletRecharge)
    {
        try
        {

            var walletBalance = _dbContext.Wallets.Where(x=> x.WalletNo == walletRecharge.WalletNo).Select(x=>x.Balance).AsNoTracking().FirstOrDefault();

            if(!_walletCharge.IsChargeable(_userProvider.CurrentUser, walletBalance, new Money(walletRecharge.Amount, walletRecharge.Currency)))
                return BadRequest("Out of limit.");


            Transaction transaction = new Transaction();  
            transaction.WalletNo = walletRecharge.WalletNo;
            transaction.TransactionType = TransactionType.Recharg;
            transaction.TransactionDate = DateTime.Now;
            transaction.Amount = new Money(walletRecharge.Amount, walletRecharge.Currency);

            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();

            transaction.QueueDomainEvent(new RechargeTransactionCreatedEvent(transaction));

            await _dbContext.DispatchDomainEventsAsync();
            return Ok();   
        }
        catch(ArgumentException ex) 
        { 
            return BadRequest(ex.Message); 
        }
        catch (Exception)
        {
            return BadRequest();    
        }
    }


    [HttpPost]
    [Route("RechargeStatistics")]    
    public async Task<IActionResult> RechargeStatistics([FromBody] RechargeStatisticsRequest statistic) 
    {
        try
        {
            var wallet = _dbContext.Wallets.Where(x => x.WalletNo == statistic.WalletNo && x.OwnerId == _userProvider.CurrentUser.Id).FirstOrDefault();
            if (wallet == null)
                return NotFound("Wallet No:"+statistic.WalletNo+" is not found.");


            var rechargeStatistic = _dbContext.Transactions
                .Where(x => x.WalletNo == wallet.WalletNo 
                    && x.TransactionType == TransactionType.Recharg
                    && x.TransactionDate.Month == DateTime.Now.Month 
                    && x.TransactionDate.Year == DateTime.Now.Year
                    )
                .GroupBy(l => l.TransactionType)
                .Select(s=> new RechargeStatisticsDTO
                {
                    WalletNo = s.First().WalletNo,
                    Currency = s.First().Amount.Currency,
                    TrasactionQty = s.Count(),
                    TotatAmount = s.Sum(c=>c.Amount.Amount)
                }).FirstOrDefault();

            return Ok(rechargeStatistic);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    [HttpGet]
    [Route("Balance/{walletNo}")]
    public async Task<IActionResult> WalletBanace(string walletNo) 
    {
        try
        {
            var wallet = _dbContext.Wallets
                .Where(x => x.WalletNo == walletNo && x.OwnerId == _userProvider.CurrentUser.Id)
                .FirstOrDefault();

            //if (wallet == null)
              //  return NotFound("Wallet No:" + walletNo + " is not found.");


            return Ok(_mapper.Map<WalletDTO>(wallet));
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

}
