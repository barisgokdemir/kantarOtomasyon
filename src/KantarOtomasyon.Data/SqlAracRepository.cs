using System.Data;
using KantarOtomasyon.Core;
using Microsoft.Data.SqlClient;

namespace KantarOtomasyon.Data;

public sealed class SqlAracRepository(string baglantiDizesi) : IAracRepository
{
    private const string Kolonlar =
        "Id, Plaka, Sofor, GirisTarihi, KayitEden, GirisAgirligi, CikisAgirligi, CikisTarihi";

    public bool GirisEkle(AracKaydi kayit)
    {
        using var conn = new SqlConnection(baglantiDizesi);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = """
            INSERT INTO dbo.AracKayitlari (Plaka, Sofor, GirisTarihi, KayitEden, GirisAgirligi)
            OUTPUT INSERTED.Id
            VALUES (@plaka, @sofor, @tarih, @kayitEden, @agirlik)
            """;
        cmd.Parameters.Add("@plaka", SqlDbType.NVarChar, 15).Value = kayit.Plaka;
        cmd.Parameters.Add("@sofor", SqlDbType.NVarChar, 45).Value = kayit.Sofor;
        cmd.Parameters.Add("@tarih", SqlDbType.DateTime2).Value = kayit.GirisTarihi;
        cmd.Parameters.Add("@kayitEden", SqlDbType.NVarChar, 45).Value = kayit.KayitEden;
        cmd.Parameters.Add("@agirlik", SqlDbType.Int).Value = kayit.GirisAgirligi;
        try
        {
            kayit.Id = (int)cmd.ExecuteScalar()!;
            return true;
        }
        catch (SqlException ex) when (Veritabani.BenzersizlikIhlali(ex))
        {
            return false;
        }
    }

    public AracKaydi? IcerideBul(string plaka)
    {
        using var conn = new SqlConnection(baglantiDizesi);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = $"SELECT {Kolonlar} FROM dbo.AracKayitlari WHERE Plaka = @plaka AND CikisTarihi IS NULL";
        cmd.Parameters.Add("@plaka", SqlDbType.NVarChar, 15).Value = plaka;
        using var r = cmd.ExecuteReader();
        return r.Read() ? Oku(r) : null;
    }

    public IReadOnlyList<AracKaydi> IcerdekileriListele()
    {
        using var conn = new SqlConnection(baglantiDizesi);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = $"SELECT {Kolonlar} FROM dbo.AracKayitlari WHERE CikisTarihi IS NULL ORDER BY GirisTarihi, Id";
        using var r = cmd.ExecuteReader();
        var liste = new List<AracKaydi>();
        while (r.Read())
            liste.Add(Oku(r));
        return liste;
    }

    public bool CikisKaydet(string plaka, int cikisAgirligi, DateTime cikisTarihi)
    {
        using var conn = new SqlConnection(baglantiDizesi);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = """
            UPDATE dbo.AracKayitlari
            SET CikisAgirligi = @agirlik, CikisTarihi = @tarih
            WHERE Plaka = @plaka AND CikisTarihi IS NULL
            """;
        cmd.Parameters.Add("@plaka", SqlDbType.NVarChar, 15).Value = plaka;
        cmd.Parameters.Add("@agirlik", SqlDbType.Int).Value = cikisAgirligi;
        cmd.Parameters.Add("@tarih", SqlDbType.DateTime2).Value = cikisTarihi;
        return cmd.ExecuteNonQuery() == 1;
    }

    private static AracKaydi Oku(SqlDataReader r) => new()
    {
        Id = r.GetInt32(0),
        Plaka = r.GetString(1),
        Sofor = r.GetString(2),
        GirisTarihi = r.GetDateTime(3),
        KayitEden = r.GetString(4),
        GirisAgirligi = r.GetInt32(5),
        CikisAgirligi = r.IsDBNull(6) ? null : r.GetInt32(6),
        CikisTarihi = r.IsDBNull(7) ? null : r.GetDateTime(7),
    };
}
