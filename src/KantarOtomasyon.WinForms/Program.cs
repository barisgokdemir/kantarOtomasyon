using System.Text.Json;
using KantarOtomasyon.Core;
using KantarOtomasyon.Data;
using Microsoft.Data.SqlClient;

namespace KantarOtomasyon.WinForms;

static class Program
{
    private const string VarsayilanBaglanti =
        "Server=.;Database=KantarOtomasyon;Integrated Security=true;TrustServerCertificate=true";

    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        var baglanti = BaglantiDizesiOku();
        try
        {
            Veritabani.Olustur(baglanti);
        }
        catch (SqlException ex)
        {
            MessageBox.Show($"SQL Server'a bağlanılamadı:\n{ex.Message}\n\nappsettings.json içindeki bağlantı dizesini kontrol edin.",
                "Kantar Otomasyon", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var kullaniciServisi = new KullaniciServisi(new SqlKullaniciRepository(baglanti));
        var kantarServisi = new KantarServisi(new SqlAracRepository(baglanti));

        using var login = new LoginForm(kullaniciServisi);
        if (login.ShowDialog() != DialogResult.OK)
            return;

        Application.Run(new AnaForm(kantarServisi, login.KullaniciAdi));
    }

    /// <summary>Önce KANTAR_BAGLANTI ortam değişkenine, sonra appsettings.json'a bakar.</summary>
    private static string BaglantiDizesiOku()
    {
        var ortam = Environment.GetEnvironmentVariable("KANTAR_BAGLANTI");
        if (!string.IsNullOrWhiteSpace(ortam))
            return ortam;

        var yol = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        if (File.Exists(yol))
        {
            using var doc = JsonDocument.Parse(File.ReadAllText(yol));
            if (doc.RootElement.TryGetProperty("ConnectionStrings", out var cs)
                && cs.TryGetProperty("Kantar", out var deger)
                && deger.GetString() is { Length: > 0 } dize)
                return dize;
        }
        return VarsayilanBaglanti;
    }
}
