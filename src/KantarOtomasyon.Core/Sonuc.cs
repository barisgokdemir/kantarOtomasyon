namespace KantarOtomasyon.Core;

public sealed record Sonuc(bool Basarili, string Mesaj)
{
    public static Sonuc Tamam(string mesaj) => new(true, mesaj);
    public static Sonuc Hata(string mesaj) => new(false, mesaj);
}
