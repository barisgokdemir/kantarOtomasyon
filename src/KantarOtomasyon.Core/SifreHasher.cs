using System.Security.Cryptography;

namespace KantarOtomasyon.Core;

/// <summary>PBKDF2 (SHA-256) ile şifre özeti. Biçim: iterasyon.tuz.özet (Base64).</summary>
public static class SifreHasher
{
    private const int TuzBoyutu = 16;
    private const int OzetBoyutu = 32;
    private const int Iterasyon = 100_000;

    public static string Hashle(string sifre)
    {
        var tuz = RandomNumberGenerator.GetBytes(TuzBoyutu);
        var ozet = Rfc2898DeriveBytes.Pbkdf2(sifre, tuz, Iterasyon, HashAlgorithmName.SHA256, OzetBoyutu);
        return $"{Iterasyon}.{Convert.ToBase64String(tuz)}.{Convert.ToBase64String(ozet)}";
    }

    public static bool Dogrula(string sifre, string hash)
    {
        var parcalar = hash.Split('.');
        if (parcalar.Length != 3 || !int.TryParse(parcalar[0], out var iterasyon) || iterasyon <= 0)
            return false;

        byte[] tuz, beklenen;
        try
        {
            tuz = Convert.FromBase64String(parcalar[1]);
            beklenen = Convert.FromBase64String(parcalar[2]);
        }
        catch (FormatException)
        {
            return false;
        }
        if (beklenen.Length == 0)
            return false;

        var ozet = Rfc2898DeriveBytes.Pbkdf2(sifre, tuz, iterasyon, HashAlgorithmName.SHA256, beklenen.Length);
        return CryptographicOperations.FixedTimeEquals(ozet, beklenen);
    }
}
