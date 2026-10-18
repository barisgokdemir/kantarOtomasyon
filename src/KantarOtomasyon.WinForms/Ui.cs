namespace KantarOtomasyon.WinForms;

/// <summary>Formları kod ile kurmak için küçük yardımcılar.</summary>
internal static class Ui
{
    public static readonly Font BaslikFont = new("Segoe UI", 16F, FontStyle.Bold);
    public static readonly Font BuyukFont = new("Segoe UI", 28F, FontStyle.Bold);

    public static void FormAyarla(Form form, string baslik, int genislik, int yukseklik)
    {
        form.Text = baslik;
        form.ClientSize = new Size(genislik, yukseklik);
        form.StartPosition = FormStartPosition.CenterScreen;
        form.Font = new Font("Segoe UI", 10F);
        form.FormBorderStyle = FormBorderStyle.FixedDialog;
        form.MaximizeBox = false;
    }

    /// <summary>Etiket + kontrol satırlarından oluşan iki kolonlu tablo.</summary>
    public static TableLayoutPanel Izgara(params (string Etiket, Control Kontrol)[] satirlar)
    {
        var tablo = new TableLayoutPanel
        {
            ColumnCount = 2,
            AutoSize = true,
            Dock = DockStyle.Top,
            Padding = new Padding(12),
        };
        tablo.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        tablo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        foreach (var (etiket, kontrol) in satirlar)
        {
            kontrol.Dock = DockStyle.Fill;
            tablo.Controls.Add(new Label { Text = etiket, AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(3, 8, 3, 3) });
            tablo.Controls.Add(kontrol);
        }
        return tablo;
    }

    public static FlowLayoutPanel Butonlar(params Button[] butonlar)
    {
        var panel = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            FlowDirection = FlowDirection.RightToLeft,
            AutoSize = true,
            Padding = new Padding(8),
        };
        panel.Controls.AddRange(butonlar);
        return panel;
    }

    public static Button Buton(string metin, EventHandler tikla)
    {
        var b = new Button { Text = metin, AutoSize = true, MinimumSize = new Size(110, 34) };
        b.Click += tikla;
        return b;
    }

    public static Label Mesaj() => new() { Dock = DockStyle.Bottom, AutoSize = false, Height = 28, TextAlign = ContentAlignment.MiddleCenter };

    public static void Goster(Label etiket, Core.Sonuc sonuc)
    {
        etiket.Text = sonuc.Mesaj;
        etiket.ForeColor = sonuc.Basarili ? Color.ForestGreen : Color.Firebrick;
    }

    public static DataGridView Tablo() => new()
    {
        Dock = DockStyle.Fill,
        ReadOnly = true,
        AllowUserToAddRows = false,
        AllowUserToDeleteRows = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        MultiSelect = false,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        RowHeadersVisible = false,
        AutoGenerateColumns = false,
        BackgroundColor = SystemColors.Window,
        ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
    };

    public static void AracKolonlari(DataGridView tablo)
    {
        tablo.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Plaka", DataPropertyName = nameof(Core.AracKaydi.Plaka) });
        tablo.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Şoför", DataPropertyName = nameof(Core.AracKaydi.Sofor) });
        tablo.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Giriş Tarihi",
            DataPropertyName = nameof(Core.AracKaydi.GirisTarihi),
            DefaultCellStyle = { Format = "dd.MM.yyyy HH:mm" },
            MinimumWidth = 135,
        });
        tablo.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Kayıt Eden", DataPropertyName = nameof(Core.AracKaydi.KayitEden) });
        tablo.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Ağırlık (Kg)",
            DataPropertyName = nameof(Core.AracKaydi.GirisAgirligi),
            DefaultCellStyle = { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight },
        });
    }
}
