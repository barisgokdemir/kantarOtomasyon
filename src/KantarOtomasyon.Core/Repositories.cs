namespace KantarOtomasyon.Core;

public interface IKullaniciRepository
{
    Kullanici? Bul(string kullaniciAdi);

    /// <summary>Kullanıcı adı zaten varsa false döner.</summary>
    bool Ekle(Kullanici kullanici);
}

public interface IAracRepository
{
    /// <summary>Plaka zaten içerideyse false döner.</summary>
    bool GirisEkle(AracKaydi kayit);

    AracKaydi? IcerideBul(string plaka);

    IReadOnlyList<AracKaydi> IcerdekileriListele();

    /// <summary>İçerideki aracın çıkışını kaydeder; araç içeride değilse false döner.</summary>
    bool CikisKaydet(string plaka, int cikisAgirligi, DateTime cikisTarihi);
}
