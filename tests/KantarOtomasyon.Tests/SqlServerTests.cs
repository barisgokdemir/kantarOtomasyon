using KantarOtomasyon.Core;
using KantarOtomasyon.Data;
using Microsoft.Data.SqlClient;

namespace KantarOtomasyon.Tests;

/// <summary>
/// KANTAR_TEST_BAGLANTI ortam değişkeni ayarlıysa gerçek SQL Server'a karşı çalışır,
/// değilse atlanır. Veritabanı adı her çalıştırmada rastgele üretilir ve sonunda silinir.
/// </summary>
public sealed class SqlServerFactAttribute : FactAttribute
{
    public SqlServerFactAttribute()
    {
        if (string.IsNullOrWhiteSpace(SqlServerFixture.SunucuBaglantisi))
            Skip = "KANTAR_TEST_BAGLANTI ayarlı değil";
    }
}

public sealed class SqlServerFixture : IDisposable
{
    public static string? SunucuBaglantisi => Environment.GetEnvironmentVariable("KANTAR_TEST_BAGLANTI");

    public string BaglantiDizesi { get; } = "";

    public SqlServerFixture()
    {
        if (string.IsNullOrWhiteSpace(SunucuBaglantisi))
            return;

        BaglantiDizesi = new SqlConnectionStringBuilder(SunucuBaglantisi)
        {
            InitialCatalog = $"KantarTest_{Guid.NewGuid():N}",
        }.ConnectionString;
        Veritabani.Olustur(BaglantiDizesi);
    }

    public void Dispose()
    {
        if (BaglantiDizesi.Length == 0)
            return;

        var builder = new SqlConnectionStringBuilder(BaglantiDizesi);
        var ad = builder.InitialCatalog;
        SqlConnection.ClearAllPools();
        builder.InitialCatalog = "master";
        using var conn = new SqlConnection(builder.ConnectionString);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = $"ALTER DATABASE [{ad}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [{ad}];";
        cmd.ExecuteNonQuery();
    }
}

public class SqlServerTests(SqlServerFixture db) : IClassFixture<SqlServerFixture>
{
    private static string Benzersiz(string onek) => onek + Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();

    [SqlServerFact]
    public void Sema_tekrar_olusturulunca_hata_vermez()
    {
        Veritabani.Olustur(db.BaglantiDizesi);
    }

    [SqlServerFact]
    public void Kullanici_kaydi_ve_girisi_sql_server_uzerinde_calisir()
    {
        var servis = new KullaniciServisi(new SqlKullaniciRepository(db.BaglantiDizesi));
        var ad = Benzersiz("kullanici");

        Assert.True(servis.KayitOl(ad, "1470", "1470").Basarili);
        Assert.False(servis.KayitOl(ad, "1470", "1470").Basarili);
        Assert.True(servis.GirisYap(ad, "1470").Basarili);
        Assert.False(servis.GirisYap(ad, "' OR '1'='1").Basarili);
    }

    [SqlServerFact]
    public void Sql_enjeksiyonu_iceren_kullanici_adi_veri_olarak_saklanir()
    {
        var repo = new SqlKullaniciRepository(db.BaglantiDizesi);
        var ad = "x'); DROP TABLE dbo.Kullanicilar; --";

        Assert.True(repo.Ekle(new Kullanici(ad, "hash")));
        Assert.Equal(ad, repo.Bul(ad)!.KullaniciAdi);
    }

    [SqlServerFact]
    public void Arac_giris_cikis_akisi_sql_server_uzerinde_calisir()
    {
        var repo = new SqlAracRepository(db.BaglantiDizesi);
        var servis = new KantarServisi(repo);
        var plaka = Benzersiz("34T");

        Assert.True(servis.AracGirisi(plaka, "Şükrü Öztürk", "15000", "baris").Basarili);
        Assert.False(servis.AracGirisi(plaka, "Ali", "9000", "baris").Basarili);

        var iceride = repo.IcerideBul(plaka);
        Assert.NotNull(iceride);
        Assert.True(iceride.Id > 0);
        Assert.Equal("Şükrü Öztürk", iceride.Sofor);
        Assert.Contains(servis.IcerdekiAraclar(), a => a.Plaka == plaka);

        Assert.True(servis.AracCikisi(plaka, "42000").Basarili);
        Assert.Null(repo.IcerideBul(plaka));
        Assert.DoesNotContain(servis.IcerdekiAraclar(), a => a.Plaka == plaka);

        // Çıkış yapan araç geçmişte kalır ve tekrar giriş yapabilir.
        Assert.True(servis.AracGirisi(plaka, "Ali", "15100", "baris").Basarili);
    }

    [SqlServerFact]
    public void Ayni_anda_ayni_plaka_icin_yalnizca_bir_giris_kabul_edilir()
    {
        var repo = new SqlAracRepository(db.BaglantiDizesi);
        var plaka = Benzersiz("06P");

        var sonuclar = Enumerable.Range(0, 8).AsParallel().Select(_ => repo.GirisEkle(new AracKaydi
        {
            Plaka = plaka,
            Sofor = "Ali",
            GirisTarihi = DateTime.Now,
            KayitEden = "baris",
            GirisAgirligi = 1000,
        })).ToList();

        Assert.Equal(1, sonuclar.Count(s => s));
    }

    [SqlServerFact]
    public void Veritabani_gecersiz_agirligi_reddeder()
    {
        var repo = new SqlAracRepository(db.BaglantiDizesi);
        var ex = Assert.Throws<SqlException>(() => repo.GirisEkle(new AracKaydi
        {
            Plaka = Benzersiz("35N"),
            Sofor = "Ali",
            GirisTarihi = DateTime.Now,
            KayitEden = "baris",
            GirisAgirligi = -1,
        }));
        Assert.Equal(547, ex.Number); // CHECK kısıtı ihlali
    }

    [SqlServerFact]
    public void Iceride_olmayan_aracin_cikisi_guncellenmez()
    {
        var repo = new SqlAracRepository(db.BaglantiDizesi);
        Assert.False(repo.CikisKaydet(Benzersiz("01Y"), 1000, DateTime.Now));
    }
}
