using KantarOtomasyon.Core;

namespace KantarOtomasyon.WinForms;

public sealed class GirisForm : Form
{
    private readonly KantarServisi _servis;
    private readonly string _kayitEden;
    private readonly TextBox _plaka = new() { CharacterCasing = CharacterCasing.Upper, MaxLength = 15 };
    private readonly TextBox _sofor = new() { MaxLength = 45 };
    private readonly TextBox _agirlik = new() { MaxLength = 9 };
    private readonly Label _gosterge = new() { Text = "0 Kg", Font = Ui.BuyukFont, Dock = DockStyle.Top, Height = 70, TextAlign = ContentAlignment.MiddleCenter };
    private readonly Label _mesaj = Ui.Mesaj();

    public GirisForm(KantarServisi servis, string kayitEden)
    {
        _servis = servis;
        _kayitEden = kayitEden;
        Ui.FormAyarla(this, "Araç Girişi", 420, 320);

        _agirlik.TextChanged += (_, _) => _gosterge.Text = $"{_agirlik.Text} Kg";
        var kaydet = Ui.Buton("Kaydet", (_, _) => Kaydet());

        Controls.Add(Ui.Izgara(("Plaka", _plaka), ("Şoför", _sofor), ("Giriş ağırlığı (Kg)", _agirlik)));
        Controls.Add(_gosterge);
        Controls.Add(_mesaj);
        Controls.Add(Ui.Butonlar(Ui.Buton("Kapat", (_, _) => Close()), kaydet));
        AcceptButton = kaydet;
    }

    private void Kaydet()
    {
        var sonuc = _servis.AracGirisi(_plaka.Text, _sofor.Text, _agirlik.Text, _kayitEden);
        Ui.Goster(_mesaj, sonuc);
        if (sonuc.Basarili)
        {
            _plaka.Clear();
            _sofor.Clear();
            _agirlik.Clear();
            _plaka.Focus();
        }
    }
}
