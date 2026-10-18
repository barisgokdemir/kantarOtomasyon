namespace KantarOtomasyon.Core;

/// <summary>Kantara giren bir aracın tartım kaydı.</summary>
public sealed class AracKaydi
{
    public int Id { get; set; }
    public required string Plaka { get; set; }
    public required string Sofor { get; set; }
    public DateTime GirisTarihi { get; set; }
    public required string KayitEden { get; set; }
    public int GirisAgirligi { get; set; }
    public int? CikisAgirligi { get; set; }
    public DateTime? CikisTarihi { get; set; }

    public bool Iceride => CikisTarihi is null;

    /// <summary>Çıkış yapılmışsa yüklenen/boşaltılan ürün ağırlığı (çıkış - giriş).</summary>
    public int? UrunAgirligi => CikisAgirligi - GirisAgirligi;
}

public sealed record Kullanici(string KullaniciAdi, string SifreHash);
