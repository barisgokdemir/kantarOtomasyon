using KantarOtomasyon.Core;

namespace KantarOtomasyon.WinForms;

public sealed class CikisForm : Form
{
    private readonly KantarServisi _servis;
    private readonly DataGridView _tablo = Ui.Tablo();
    private readonly TextBox _plaka = new() { ReadOnly = true };
    private readonly TextBox _sofor = new() { ReadOnly = true };
    private readonly TextBox _girisAgirligi = new() { ReadOnly = true };
    private readonly TextBox _cikisAgirligi = new() { MaxLength = 9 };
    private readonly TextBox _urunAgirligi = new() { ReadOnly = true };
    private readonly Label _gosterge = new() { Text = "0 Kg", Font = Ui.BuyukFont, Dock = DockStyle.Top, Height = 70, TextAlign = ContentAlignment.MiddleCenter };
    private readonly Label _mesaj = Ui.Mesaj();

    public CikisForm(KantarServisi servis)
    {
        _servis = servis;
        Ui.FormAyarla(this, "Araç Çıkışı", 820, 520);

        Ui.AracKolonlari(_tablo);
        _tablo.SelectionChanged += (_, _) => Sec();
        _cikisAgirligi.TextChanged += (_, _) => Hesapla();

        var sag = new Panel { Dock = DockStyle.Right, Width = 340 };
        sag.Controls.Add(Ui.Izgara(
            ("Plaka", _plaka), ("Şoför", _sofor), ("Giriş ağırlığı", _girisAgirligi),
            ("Çıkış ağırlığı", _cikisAgirligi), ("Ürün ağırlığı", _urunAgirligi)));
        sag.Controls.Add(_gosterge);

        var kaydet = Ui.Buton("Çıkışı Kaydet", (_, _) => Kaydet());
        Controls.Add(_tablo);
        Controls.Add(sag);
        Controls.Add(_mesaj);
        Controls.Add(Ui.Butonlar(Ui.Buton("Kapat", (_, _) => Close()), kaydet));
        AcceptButton = kaydet;
        Yenile();
    }

    private void Yenile()
    {
        _tablo.DataSource = _servis.IcerdekiAraclar().ToList();
        Sec();
    }

    private void Sec()
    {
        if (_tablo.CurrentRow?.DataBoundItem is not AracKaydi arac)
        {
            _plaka.Clear();
            _sofor.Clear();
            _girisAgirligi.Clear();
            return;
        }
        _plaka.Text = arac.Plaka;
        _sofor.Text = arac.Sofor;
        _girisAgirligi.Text = arac.GirisAgirligi.ToString();
        Hesapla();
    }

    private void Hesapla()
    {
        _gosterge.Text = $"{_cikisAgirligi.Text} Kg";
        _urunAgirligi.Text =
            KantarServisi.AgirlikCozumle(_cikisAgirligi.Text, out var cikis) && int.TryParse(_girisAgirligi.Text, out var giris)
                ? KantarServisi.UrunAgirligi(giris, cikis).ToString()
                : "";
    }

    private void Kaydet()
    {
        if (_plaka.Text.Length == 0)
        {
            Ui.Goster(_mesaj, Sonuc.Hata("Listeden bir araç seçiniz"));
            return;
        }
        var sonuc = _servis.AracCikisi(_plaka.Text, _cikisAgirligi.Text);
        Ui.Goster(_mesaj, sonuc);
        if (sonuc.Basarili)
        {
            _cikisAgirligi.Clear();
            Yenile();
        }
    }
}
