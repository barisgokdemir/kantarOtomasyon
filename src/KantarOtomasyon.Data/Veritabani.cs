using Microsoft.Data.SqlClient;

namespace KantarOtomasyon.Data;

/// <summary>Veritabanını ve tabloları yoksa oluşturur.</summary>
public static class Veritabani
{
    private const string Sema = """
        IF OBJECT_ID(N'dbo.Kullanicilar', N'U') IS NULL
        CREATE TABLE dbo.Kullanicilar (
            KullaniciAdi NVARCHAR(45)  NOT NULL CONSTRAINT PK_Kullanicilar PRIMARY KEY,
            SifreHash    NVARCHAR(200) NOT NULL
        );

        IF OBJECT_ID(N'dbo.AracKayitlari', N'U') IS NULL
        BEGIN
            CREATE TABLE dbo.AracKayitlari (
                Id            INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_AracKayitlari PRIMARY KEY,
                Plaka         NVARCHAR(15) NOT NULL,
                Sofor         NVARCHAR(45) NOT NULL,
                GirisTarihi   DATETIME2(0) NOT NULL,
                KayitEden     NVARCHAR(45) NOT NULL,
                GirisAgirligi INT NOT NULL CONSTRAINT CK_AracKayitlari_GirisAgirligi CHECK (GirisAgirligi > 0),
                CikisAgirligi INT NULL CONSTRAINT CK_AracKayitlari_CikisAgirligi CHECK (CikisAgirligi > 0),
                CikisTarihi   DATETIME2(0) NULL
            );

            -- Aynı plaka aynı anda yalnızca bir kez kantarda (içeride) olabilir.
            CREATE UNIQUE INDEX UX_AracKayitlari_IcerdekiPlaka
                ON dbo.AracKayitlari (Plaka) WHERE CikisTarihi IS NULL;
        END
        """;

    public static void Olustur(string baglantiDizesi)
    {
        var builder = new SqlConnectionStringBuilder(baglantiDizesi);
        var veritabaniAdi = builder.InitialCatalog;
        if (string.IsNullOrWhiteSpace(veritabaniAdi))
            throw new ArgumentException("Bağlantı dizesinde veritabanı adı (Initial Catalog) olmalı.", nameof(baglantiDizesi));

        builder.InitialCatalog = "master";
        using (var master = new SqlConnection(builder.ConnectionString))
        {
            master.Open();
            using var cmd = master.CreateCommand();
            cmd.CommandText = $"IF DB_ID(@ad) IS NULL CREATE DATABASE {QuoteName(veritabaniAdi)};";
            cmd.Parameters.AddWithValue("@ad", veritabaniAdi);
            cmd.ExecuteNonQuery();
        }

        using var conn = new SqlConnection(baglantiDizesi);
        conn.Open();
        using var sema = conn.CreateCommand();
        sema.CommandText = Sema;
        sema.ExecuteNonQuery();
    }

    internal static string QuoteName(string ad) => "[" + ad.Replace("]", "]]") + "]";

    /// <summary>Benzersizlik (PK / unique index) ihlali mi?</summary>
    internal static bool BenzersizlikIhlali(SqlException ex) => ex.Number is 2601 or 2627;
}
