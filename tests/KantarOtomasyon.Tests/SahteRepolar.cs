using KantarOtomasyon.Core;

namespace KantarOtomasyon.Tests;

internal sealed class SahteKullaniciRepository : IKullaniciRepository
{
    public Dictionary<string, Kullanici> Kayitlar { get; } = [];

    public Kullanici? Bul(string kullaniciAdi) => Kayitlar.GetValueOrDefault(kullaniciAdi);

    public bool Ekle(Kullanici kullanici) => Kayitlar.TryAdd(kullanici.KullaniciAdi, kullanici);
}

internal sealed class SahteAracRepository : IAracRepository
{
    public List<AracKaydi> Kayitlar { get; } = [];

    public bool GirisEkle(AracKaydi kayit)
    {
        if (IcerideBul(kayit.Plaka) is not null)
            return false;
        kayit.Id = Kayitlar.Count + 1;
        Kayitlar.Add(kayit);
        return true;
    }

    public AracKaydi? IcerideBul(string plaka) => Kayitlar.SingleOrDefault(a => a.Plaka == plaka && a.Iceride);

    public IReadOnlyList<AracKaydi> IcerdekileriListele() => Kayitlar.Where(a => a.Iceride).ToList();

    public bool CikisKaydet(string plaka, int cikisAgirligi, DateTime cikisTarihi)
    {
        var kayit = IcerideBul(plaka);
        if (kayit is null)
            return false;
        kayit.CikisAgirligi = cikisAgirligi;
        kayit.CikisTarihi = cikisTarihi;
        return true;
    }
}

internal sealed class SabitZaman(DateTimeOffset an) : TimeProvider
{
    public override DateTimeOffset GetUtcNow() => an.ToUniversalTime();
    public override TimeZoneInfo LocalTimeZone => TimeZoneInfo.Utc;
}
