using Microsoft.AspNetCore.Identity;
using NullGuard;

namespace eWallet.Models;

public class ApplicationUser : IdentityUser
{    
    [AllowNull]
    public string? FirstName { get; set; }
    [AllowNull]
    public string? LastName { get; set; }
    public bool IsEmailConfirmed { get; set; } = false;
    [AllowNull]
    public string? PhoneNumber { get; set; }
    public ApplicationUserStatus Status { get; set; }

    [AllowNull]
    public ICollection<Wallet> Wallets { get; set; }    
}


public enum ApplicationUserStatus
{
    UNIDENTIFIED,
    IDENTIFIED    
}