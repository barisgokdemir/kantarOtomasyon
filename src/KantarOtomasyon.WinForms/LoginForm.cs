using KantarOtomasyon.Core;

namespace KantarOtomasyon.WinForms;

public sealed class LoginForm : Form
{
    private readonly KullaniciServisi _servis;
    private readonly TextBox _ad = new();
    private readonly TextBox _sifre = new() { UseSystemPasswordChar = true };
    private readonly Label _mesaj = Ui.Mesaj();

    public string KullaniciAdi { get; private set; } = "";

    public LoginForm(KullaniciServisi servis)
    {
        _servis = servis;
        Ui.FormAyarla(this, "Kantar Otomasyon - Giriş", 380, 205);

        var girisButonu = Ui.Buton("Giriş", (_, _) => GirisYap());
        Controls.Add(Ui.Izgara(("Kullanıcı adı", _ad), ("Şifre", _sifre)));
        Controls.Add(new Label { Text = "Kantar Otomasyon", Font = Ui.BaslikFont, Dock = DockStyle.Top, Height = 44, TextAlign = ContentAlignment.MiddleCenter });
        Controls.Add(_mesaj);
        Controls.Add(Ui.Butonlar(
            girisButonu,
            Ui.Buton("Kayıt Ol", (_, _) => KayitAc()),
            Ui.Buton("Çıkış", (_, _) => Close())));
        AcceptButton = girisButonu;
    }

    private void GirisYap()
    {
        var sonuc = _servis.GirisYap(_ad.Text, _sifre.Text);
        if (!sonuc.Basarili)
        {
            Ui.Goster(_mesaj, sonuc);
            return;
        }
        KullaniciAdi = _ad.Text.Trim();
        DialogResult = DialogResult.OK;
    }

    private void KayitAc()
    {
        using var kayit = new KayitForm(_servis);
        if (kayit.ShowDialog(this) == DialogResult.OK)
        {
            _ad.Text = kayit.KullaniciAdi;
            _sifre.Clear();
            _sifre.Focus();
            Ui.Goster(_mesaj, Sonuc.Tamam("Kayıt başarılı, giriş yapabilirsiniz"));
        }
    }
}
