using KantarOtomasyon.Core;

namespace KantarOtomasyon.Tests;

public class KantarServisiTests
{
    private static readonly DateTimeOffset Simdi = new(2026, 10, 18, 14, 30, 0, TimeSpan.Zero);
    private readonly SahteAracRepository _repo = new();
    private readonly KantarServisi _servis;

    public KantarServisiTests() => _servis = new KantarServisi(_repo, new SabitZaman(Simdi));

    [Fact]
    public void Arac_girisi_kaydedilir()
    {
        var sonuc = _servis.AracGirisi("34 abc 123", "Ali", "15000", "baris");

        Assert.True(sonuc.Basarili);
        var kayit = Assert.Single(_repo.Kayitlar);
        Assert.Equal("34ABC123", kayit.Plaka);
        Assert.Equal("Ali", kayit.Sofor);
        Assert.Equal(15000, kayit.GirisAgirligi);
        Assert.Equal("baris", kayit.KayitEden);
        Assert.Equal(Simdi.DateTime, kayit.GirisTarihi);
        Assert.True(kayit.Iceride);
    }

    [Fact]
    public void Ayni_plaka_iceride_iken_tekrar_giremez()
    {
        _servis.AracGirisi("34ABC123", "Ali", "15000", "baris");
        var sonuc = _servis.AracGirisi("34 abc 123", "Veli", "9000", "baris");

        Assert.False(sonuc.Basarili);
        Assert.Single(_repo.Kayitlar);
    }

    [Fact]
    public void Cikis_yapan_arac_tekrar_giris_yapabilir()
    {
        _servis.AracGirisi("34ABC123", "Ali", "15000", "baris");
        _servis.AracCikisi("34ABC123", "40000");

        Assert.True(_servis.AracGirisi("34ABC123", "Ali", "15100", "baris").Basarili);
        Assert.Equal(2, _repo.Kayitlar.Count);
    }

    [Theory]
    [InlineData("", "Ali", "1000")]
    [InlineData("34ABC123", "", "1000")]
    [InlineData("34ABC123", "Ali", "")]
    [InlineData("34ABC123", "Ali", "abc")]
    [InlineData("34ABC123", "Ali", "0")]
    [InlineData("34ABC123", "Ali", "-500")]
    [InlineData("34ABC123", "Ali", "12.5")]
    [InlineData("34ABC123", "Ali", "99999999999")]
    [InlineData("34ABCDEFGHIJKLMN", "Ali", "1000")]
    public void Gecersiz_giris_reddedilir(string plaka, string sofor, string agirlik)
    {
        Assert.False(_servis.AracGirisi(plaka, sofor, agirlik, "baris").Basarili);
        Assert.Empty(_repo.Kayitlar);
    }

    [Fact]
    public void Cikis_agirligi_ve_urun_agirligi_hesaplanir()
    {
        _servis.AracGirisi("34ABC123", "Ali", "15000", "baris");

        var sonuc = _servis.AracCikisi("34abc123", "40000");

        Assert.True(sonuc.Basarili);
        Assert.Contains("25000", sonuc.Mesaj);
        var kayit = Assert.Single(_repo.Kayitlar);
        Assert.Equal(40000, kayit.CikisAgirligi);
        Assert.Equal(25000, kayit.UrunAgirligi);
        Assert.Equal(Simdi.DateTime, kayit.CikisTarihi);
        Assert.False(kayit.Iceride);
    }

    [Fact]
    public void Bosaltma_yapan_aracin_urun_agirligi_negatif_olur()
    {
        Assert.Equal(-12000, KantarServisi.UrunAgirligi(girisAgirligi: 30000, cikisAgirligi: 18000));
    }

    [Fact]
    public void Iceride_olmayan_arac_cikis_yapamaz()
    {
        Assert.False(_servis.AracCikisi("06XYZ99", "1000").Basarili);
    }

    [Fact]
    public void Gecersiz_cikis_agirligi_reddedilir()
    {
        _servis.AracGirisi("34ABC123", "Ali", "15000", "baris");
        Assert.False(_servis.AracCikisi("34ABC123", "abc").Basarili);
        Assert.True(Assert.Single(_repo.Kayitlar).Iceride);
    }

    [Fact]
    public void Toplam_agirlik_ve_arac_sayisi_yalnizca_icerdekileri_sayar()
    {
        _servis.AracGirisi("34A1", "Ali", "1500", "baris");
        _servis.AracGirisi("34A2", "Veli", "2500", "baris");
        _servis.AracGirisi("34A3", "Can", "4000", "baris");
        _servis.AracCikisi("34A2", "9000");

        Assert.Equal(2, _servis.AracSayisi());
        Assert.Equal(5500, _servis.ToplamAgirlik());
        Assert.Equal(["34A1", "34A3"], _servis.IcerdekiAraclar().Select(a => a.Plaka));
    }

    [Fact]
    public void Toplam_agirlik_int_sinirini_tasabilir()
    {
        _servis.AracGirisi("34A1", "Ali", "999999999", "baris");
        _servis.AracGirisi("34A2", "Veli", "999999999", "baris");
        _servis.AracGirisi("34A3", "Can", "999999999", "baris");

        Assert.Equal(2_999_999_997L, _servis.ToplamAgirlik());
    }

    [Theory]
    [InlineData("34 abc 123", "34ABC123")]
    [InlineData(" 06xyz99 ", "06XYZ99")]
    [InlineData("35\tdef\t1", "35DEF1")]
    public void Plaka_normalize_edilir(string girdi, string beklenen)
    {
        Assert.Equal(beklenen, KantarServisi.PlakaNormalize(girdi));
    }
}
