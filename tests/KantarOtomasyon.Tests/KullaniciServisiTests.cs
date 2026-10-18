using KantarOtomasyon.Core;

namespace KantarOtomasyon.Tests;

public class KullaniciServisiTests
{
    private readonly SahteKullaniciRepository _repo = new();
    private readonly KullaniciServisi _servis;

    public KullaniciServisiTests() => _servis = new KullaniciServisi(_repo);

    [Fact]
    public void Kayit_olan_kullanici_giris_yapabilir()
    {
        Assert.True(_servis.KayitOl("baris", "1470", "1470").Basarili);
        Assert.True(_servis.GirisYap("baris", "1470").Basarili);
    }

    [Fact]
    public void Sifre_veritabanina_duz_metin_yazilmaz()
    {
        _servis.KayitOl("baris", "1470", "1470");
        Assert.NotEqual("1470", _repo.Kayitlar["baris"].SifreHash);
    }

    [Fact]
    public void Yanlis_sifreyle_giris_basarisiz()
    {
        _servis.KayitOl("baris", "1470", "1470");
        var sonuc = _servis.GirisYap("baris", "9999");
        Assert.False(sonuc.Basarili);
        Assert.Equal("Kullanıcı adı veya şifre yanlış", sonuc.Mesaj);
    }

    [Fact]
    public void Olmayan_kullanici_giris_yapamaz()
    {
        Assert.False(_servis.GirisYap("yok", "1234").Basarili);
    }

    [Fact]
    public void Sql_enjeksiyonu_ile_giris_yapilamaz()
    {
        _servis.KayitOl("baris", "1470", "1470");
        Assert.False(_servis.GirisYap("baris' --", "x").Basarili);
        Assert.False(_servis.GirisYap("baris", "' OR '1'='1").Basarili);
    }

    [Fact]
    public void Sifreler_eslesmezse_kayit_yapilmaz()
    {
        // Java sürümünde bu kontrol kayıttan SONRA yapılıyordu; kullanıcı yine de ekleniyordu.
        var sonuc = _servis.KayitOl("baris", "1470", "1471");
        Assert.False(sonuc.Basarili);
        Assert.Empty(_repo.Kayitlar);
    }

    [Fact]
    public void Ayni_kullanici_adi_iki_kez_kaydedilemez()
    {
        _servis.KayitOl("baris", "1470", "1470");
        var sonuc = _servis.KayitOl("baris", "abcd", "abcd");
        Assert.False(sonuc.Basarili);
        Assert.Equal("Bu kullanıcı adı zaten alınmış", sonuc.Mesaj);
    }

    [Theory]
    [InlineData("", "1234")]
    [InlineData("   ", "1234")]
    [InlineData("baris", "123")]
    public void Gecersiz_kayit_reddedilir(string ad, string sifre)
    {
        Assert.False(_servis.KayitOl(ad, sifre, sifre).Basarili);
        Assert.Empty(_repo.Kayitlar);
    }

    [Fact]
    public void Kullanici_adi_bosluklari_kirpilir()
    {
        _servis.KayitOl("  baris ", "1470", "1470");
        Assert.True(_servis.GirisYap("baris", "1470").Basarili);
    }
}
