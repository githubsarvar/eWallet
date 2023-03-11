using System.Security.Cryptography;
using System.Text;

namespace eWallet.Services;

public class WalletService
{

    public static string GenerateWalletNumber(string userId)
    {
        // Generate a random 6-digit number
        int randomNumber = new Random().Next(100000, 999999);

        // Generate the control key by concatenating the user ID and the random number
        string controlKey = userId + randomNumber.ToString();

        // Generate a hash of the control key using SHA256
        byte[] controlKeyBytes = Encoding.UTF8.GetBytes(controlKey);
        byte[] hashBytes;
        using (SHA256 sha256 = SHA256.Create())
        {
            hashBytes = sha256.ComputeHash(controlKeyBytes);
        }
        string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

        // Format the e-wallet number using the first 4 characters of the hash and the random number
        string eWalletNumber = hash.Substring(0, 4) + "-" + randomNumber.ToString();

        return eWalletNumber;
    }
}

