# Kantar Otomasyon

Kantara giren ve çıkan araçların tartımlarını kaydeden masaüstü uygulaması.
.NET 8 (Windows Forms) ve SQL Server ile yazılmıştır.

## Özellikler

- Kullanıcı kaydı ve girişi (şifreler PBKDF2 ile özetlenerek saklanır)
- Araç girişi: plaka, şoför, giriş ağırlığı
- Araç çıkışı: çıkış ağırlığı ve ürün ağırlığı (çıkış − giriş) hesabı
- Ana ekranda kantardaki araçlar, araç sayısı ve toplam ağırlık
- Çıkış yapan araçların kayıtları silinmez, geçmiş olarak saklanır

## Proje yapısı

| Proje | Açıklama |
|---|---|
| `src/KantarOtomasyon.Core` | Modeller ve iş kuralları (UI ve veritabanından bağımsız) |
| `src/KantarOtomasyon.Data` | SQL Server erişimi (Microsoft.Data.SqlClient, parametreli sorgular) |
| `src/KantarOtomasyon.WinForms` | Windows Forms arayüzü |
| `tests/KantarOtomasyon.Tests` | xUnit birim ve SQL Server entegrasyon testleri |

## Çalıştırma

Gerekenler: .NET 8 SDK ve SQL Server (Express / Developer / LocalDB).

```
dotnet run --project src/KantarOtomasyon.WinForms
```

Uygulama ilk açılışta veritabanını ve tabloları kendisi oluşturur. Bağlantı dizesi
`src/KantarOtomasyon.WinForms/appsettings.json` dosyasından okunur; `KANTAR_BAGLANTI`
ortam değişkeni ayarlıysa o kullanılır. Varsayılan:

```
Server=.;Database=KantarOtomasyon;Integrated Security=true;TrustServerCertificate=true
```

## Testler

```
dotnet test
```

SQL Server entegrasyon testleri yalnızca `KANTAR_TEST_BAGLANTI` ortam değişkeni
ayarlıysa çalışır (değilse atlanır). Testler her çalıştırmada geçici bir veritabanı
oluşturup sonunda siler:

```powershell
$env:KANTAR_TEST_BAGLANTI = "Server=.;Integrated Security=true;TrustServerCertificate=true"
dotnet test
```

GitHub Actions her push ve pull request'te testleri SQL Server 2022 konteynerine
karşı çalıştırır ve Windows derlemesini `KantarOtomasyon-win-x64` artifact'i olarak yükler.

## Java sürümünden farklar

Proje önceden Java Swing + MySQL ile yazılmıştı (git geçmişinde duruyor). Dönüşümde:

- SQL enjeksiyonuna açık string birleştirmeli sorgular parametreli sorgulara çevrildi
- Şifreler düz metin yerine tuzlu PBKDF2 özetiyle saklanıyor
- Kayıtta "şifreler eşleşmiyor" kontrolü artık kayıttan önce yapılıyor
- Ağırlıklar `VARCHAR` yerine `INT`, tarihler `DATETIME2` olarak tutuluyor
- Araç çıkışında kayıt silinmiyor; çıkış ağırlığı ve tarihi işleniyor
- Aynı plakanın aynı anda iki kez içeride olması veritabanı düzeyinde engelleniyor
