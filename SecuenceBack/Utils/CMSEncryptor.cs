using System.Text;
using System.Security.Cryptography;

namespace SecuenceBack.Utils;
public class CMSEncryptor
{
    private readonly IConfiguration appsettings;
    private readonly RNGCryptoServiceProvider randomNumberGenerator;

    public CMSEncryptor(IConfiguration _appsettings)
    {
        appsettings = _appsettings;
        randomNumberGenerator = new RNGCryptoServiceProvider();
    }

    // Encrypts password using SHA256 algorithm.
    public string Encrypt(string password)
    {
        var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + appsettings["Encryption:Key"]));
        var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        return hash;
    }

    public bool Compare(string passwordAttempt, string hashedPassword)
    {
        var watch = Encrypt(passwordAttempt + appsettings["Encryption:Key"]);
        return (Encrypt(passwordAttempt) == hashedPassword);
    }

}
