using System.Data;
using KantarOtomasyon.Core;
using Microsoft.Data.SqlClient;

namespace KantarOtomasyon.Data;

public sealed class SqlKullaniciRepository(string baglantiDizesi) : IKullaniciRepository
{
    public Kullanici? Bul(string kullaniciAdi)
    {
        using var conn = new SqlConnection(baglantiDizesi);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT KullaniciAdi, SifreHash FROM dbo.Kullanicilar WHERE KullaniciAdi = @ad";
        cmd.Parameters.Add("@ad", SqlDbType.NVarChar, 45).Value = kullaniciAdi;
        using var r = cmd.ExecuteReader();
        return r.Read() ? new Kullanici(r.GetString(0), r.GetString(1)) : null;
    }

    public bool Ekle(Kullanici kullanici)
    {
        using var conn = new SqlConnection(baglantiDizesi);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO dbo.Kullanicilar (KullaniciAdi, SifreHash) VALUES (@ad, @hash)";
        cmd.Parameters.Add("@ad", SqlDbType.NVarChar, 45).Value = kullanici.KullaniciAdi;
        cmd.Parameters.Add("@hash", SqlDbType.NVarChar, 200).Value = kullanici.SifreHash;
        try
        {
            cmd.ExecuteNonQuery();
            return true;
        }
        catch (SqlException ex) when (Veritabani.BenzersizlikIhlali(ex))
        {
            return false;
        }
    }
}
