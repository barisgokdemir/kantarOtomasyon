using KantarOtomasyon.Core;

namespace KantarOtomasyon.WinForms;

public sealed class AnaForm : Form
{
    private readonly KantarServisi _servis;
    private readonly string _kullaniciAdi;
    private readonly DataGridView _tablo = Ui.Tablo();
    private readonly Label _saat = new() { AutoSize = true, Font = Ui.BaslikFont };
    private readonly Label _toplamAgirlik = new() { AutoSize = true, Font = Ui.BaslikFont };
    private readonly Label _aracSayisi = new() { AutoSize = true, Font = Ui.BaslikFont };
    private readonly System.Windows.Forms.Timer _zamanlayici = new() { Interval = 1000 };

    public AnaForm(KantarServisi servis, string kullaniciAdi)
    {
        _servis = servis;
        _kullaniciAdi = kullaniciAdi;
        Ui.FormAyarla(this, "Kantar Otomasyon", 900, 560);
        FormBorderStyle = FormBorderStyle.Sizable;
        MaximizeBox = true;

        var ust = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(12), WrapContents = true };
        ust.Controls.Add(Bilgi("Kullanıcı", new Label { Text = kullaniciAdi, AutoSize = true, Font = Ui.BaslikFont }));
        ust.Controls.Add(Bilgi("Tarih / Saat", _saat));
        ust.Controls.Add(Bilgi("Kantardaki araç", _aracSayisi));
        ust.Controls.Add(Bilgi("Toplam ağırlık", _toplamAgirlik));

        Ui.AracKolonlari(_tablo);
        Controls.Add(_tablo);
        Controls.Add(ust);
        Controls.Add(Ui.Butonlar(
            Ui.Buton("Çıkış", (_, _) => Close()),
            Ui.Buton("Araç Çıkışı", (_, _) => Ac(new CikisForm(_servis))),
            Ui.Buton("Araç Girişi", (_, _) => Ac(new GirisForm(_servis, _kullaniciAdi)))));

        _zamanlayici.Tick += (_, _) => _saat.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
        _zamanlayici.Start();
        _saat.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
        Yenile();
    }

    private static Control Bilgi(string baslik, Label deger)
    {
        var panel = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, AutoSize = true, Margin = new Padding(0, 0, 32, 0) };
        panel.Controls.Add(new Label { Text = baslik, AutoSize = true, ForeColor = SystemColors.GrayText });
        panel.Controls.Add(deger);
        return panel;
    }

    private void Ac(Form form)
    {
        using (form)
            form.ShowDialog(this);
        Yenile();
    }

    private void Yenile()
    {
        var araclar = _servis.IcerdekiAraclar();
        _tablo.DataSource = araclar.ToList();
        _aracSayisi.Text = araclar.Count.ToString();
        _toplamAgirlik.Text = $"{araclar.Sum(a => (long)a.GirisAgirligi):N0} Kg";
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            _zamanlayici.Dispose();
        base.Dispose(disposing);
    }
}
