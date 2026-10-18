namespace KantarOtomasyon.Core;

public sealed class KullaniciServisi(IKullaniciRepository repository)
{
    public Sonuc GirisYap(string kullaniciAdi, string sifre)
    {
        var kullanici = repository.Bul(kullaniciAdi.Trim());
        return kullanici is not null && SifreHasher.Dogrula(sifre, kullanici.SifreHash)
            ? Sonuc.Tamam("Giriş başarılı")
            : Sonuc.Hata("Kullanıcı adı veya şifre yanlış");
    }

    public Sonuc KayitOl(string kullaniciAdi, string sifre, string sifreTekrar)
    {
        kullaniciAdi = kullaniciAdi.Trim();
        if (kullaniciAdi.Length == 0 || kullaniciAdi.Length > 45)
            return Sonuc.Hata("Kullanıcı adı 1-45 karakter olmalı");
        if (sifre.Length < 4)
            return Sonuc.Hata("Şifre en az 4 karakter olmalı");
        if (sifre != sifreTekrar)
            return Sonuc.Hata("Şifreler eşleşmiyor");

        return repository.Ekle(new Kullanici(kullaniciAdi, SifreHasher.Hashle(sifre)))
            ? Sonuc.Tamam("Kayıt başarılı")
            : Sonuc.Hata("Bu kullanıcı adı zaten alınmış");
    }
}
