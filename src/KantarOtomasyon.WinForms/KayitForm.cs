using KantarOtomasyon.Core;

namespace KantarOtomasyon.WinForms;

public sealed class KayitForm : Form
{
    private readonly KullaniciServisi _servis;
    private readonly TextBox _ad = new();
    private readonly TextBox _sifre = new() { UseSystemPasswordChar = true };
    private readonly TextBox _sifreTekrar = new() { UseSystemPasswordChar = true };
    private readonly Label _mesaj = Ui.Mesaj();

    public string KullaniciAdi { get; private set; } = "";

    public KayitForm(KullaniciServisi servis)
    {
        _servis = servis;
        Ui.FormAyarla(this, "Kullanıcı Kaydı", 400, 220);

        var kaydet = Ui.Buton("Kaydet", (_, _) => Kaydet());
        Controls.Add(Ui.Izgara(("Kullanıcı adı", _ad), ("Şifre", _sifre), ("Şifre tekrar", _sifreTekrar)));
        Controls.Add(_mesaj);
        Controls.Add(Ui.Butonlar(kaydet, Ui.Buton("Geri", (_, _) => Close())));
        AcceptButton = kaydet;
    }

    private void Kaydet()
    {
        var sonuc = _servis.KayitOl(_ad.Text, _sifre.Text, _sifreTekrar.Text);
        if (!sonuc.Basarili)
        {
            Ui.Goster(_mesaj, sonuc);
            return;
        }
        KullaniciAdi = _ad.Text.Trim();
        DialogResult = DialogResult.OK;
    }
}
