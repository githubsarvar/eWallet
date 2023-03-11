using AutoMapper;
using eWallet.DTOs;
using eWallet.Infrastructure;
using eWallet.Models;

namespace eWallet
{
    public class AutoMapping: Profile
    {
        public AutoMapping() 
        {
            CreateMap<WalletDTO, Wallet>()
                .ForMember(d => d.Balance, conf => conf.MapFrom(src => new Money(src.Balance, src.Currency)));
            CreateMap<Wallet, WalletDTO>()
                    .ForMember(d => d.Balance, conf => conf.MapFrom(s => s.Balance.Amount))
                    .ForMember(d => d.Currency, conf => conf.MapFrom(s => s.Balance.Currency));
        }

    }
}
