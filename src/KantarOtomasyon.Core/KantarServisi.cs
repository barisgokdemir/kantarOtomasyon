namespace KantarOtomasyon.Core;

public sealed class KantarServisi(IAracRepository repository, TimeProvider? zaman = null)
{
    private readonly TimeProvider _zaman = zaman ?? TimeProvider.System;

    public Sonuc AracGirisi(string plaka, string sofor, string agirlikMetni, string kayitEden)
    {
        plaka = PlakaNormalize(plaka);
        sofor = sofor.Trim();

        if (plaka.Length == 0 || plaka.Length > 15)
            return Sonuc.Hata("Geçerli bir plaka giriniz");
        if (sofor.Length == 0)
            return Sonuc.Hata("Şoför adı giriniz");
        if (!AgirlikCozumle(agirlikMetni, out var agirlik))
            return Sonuc.Hata("Ağırlık pozitif bir tam sayı olmalı");

        var kayit = new AracKaydi
        {
            Plaka = plaka,
            Sofor = sofor,
            GirisTarihi = _zaman.GetLocalNow().DateTime,
            KayitEden = kayitEden,
            GirisAgirligi = agirlik,
        };

        return repository.GirisEkle(kayit)
            ? Sonuc.Tamam("Kayıt başarılı")
            : Sonuc.Hata("Bu plaka zaten kantarda, farklı bir plaka giriniz");
    }

    public Sonuc AracCikisi(string plaka, string cikisAgirligiMetni)
    {
        plaka = PlakaNormalize(plaka);
        if (!AgirlikCozumle(cikisAgirligiMetni, out var cikisAgirligi))
            return Sonuc.Hata("Çıkış ağırlığı pozitif bir tam sayı olmalı");

        var kayit = repository.IcerideBul(plaka);
        if (kayit is null || !repository.CikisKaydet(plaka, cikisAgirligi, _zaman.GetLocalNow().DateTime))
            return Sonuc.Hata("Bu plakada içeride araç yok");

        return Sonuc.Tamam($"Çıkış kaydedildi. Ürün ağırlığı: {UrunAgirligi(kayit.GirisAgirligi, cikisAgirligi)} Kg");
    }

    public IReadOnlyList<AracKaydi> IcerdekiAraclar() => repository.IcerdekileriListele();

    public long ToplamAgirlik() => repository.IcerdekileriListele().Sum(a => (long)a.GirisAgirligi);

    public int AracSayisi() => repository.IcerdekileriListele().Count;

    public static int UrunAgirligi(int girisAgirligi, int cikisAgirligi) => cikisAgirligi - girisAgirligi;

    public static string PlakaNormalize(string plaka) =>
        string.Concat(plaka.Where(c => !char.IsWhiteSpace(c))).ToUpperInvariant();

    public static bool AgirlikCozumle(string metin, out int agirlik) =>
        int.TryParse(metin.Trim(), System.Globalization.NumberStyles.None, null, out agirlik) && agirlik > 0;
}
