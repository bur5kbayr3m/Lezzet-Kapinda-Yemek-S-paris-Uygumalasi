# YemekSipariş — Online Yemek Siparişi Projesi

Bu projede, Yemeksepeti ve benzeri yemek sipariş platformlarının temel işlevleri örnek alınarak, .NET MVC mimarisi ve C# dili ile modern bir online yemek sipariş sistemi geliştirilmiştir.

-----------------------------------------------------
TEMEL ÖZELLİKLER
-----------------------------------------------------

• 3 Kullanıcı Tipi:
  - Admin: API seviyesinde tüm kullanıcıları ve siparişleri yönetebilir (arayüz paneli tamamlanmadı).
  - Mağaza (Restoran): Ürün ekler, siparişlerini ve yorumlarını görür, kampanya oluşturabilir.
  - Müşteri: Kayıt olup restoranlardan ürün seçer, sepetine ekleyip sipariş verir.

• Kimlik Doğrulama:
  - JWT ile güvenli kullanıcı giriş/çıkış sistemi.

• Restoran ve Ürün Yönetimi:
  - Restoranlar detaylı profil ve menüye sahip.
  - Her mağaza kendi ürün ve menüsünü yönetebilir.

• Sepet ve Sipariş Modülü:
  - Kullanıcılar ürünleri sepete ekleyip, sipariş verebilir.
  - Sepette toplam tutar ve ödeme yöntemi seçimi bulunur.
  - Sipariş geçmişi takibi (müşteri ve mağaza için).

• Arama ve Filtreleme:
  - Restoranları ve ürünleri isim veya kategoriye göre arayabilme.

• Yorum ve Puanlama:
  - Kullanıcılar sipariş verdikleri restoranları puanlayıp yorum yapabilir.

• Kampanya & Promosyon Sekmesi:
  - Her restoran için özel kampanya ve promosyon alanı (yakında gelecek).

• Modern Arayüz:
  - Responsive (mobil uyumlu) ve sade bir tasarım.
  - Modallar ile kullanıcı dostu giriş/kayıt ekranları.
  - Dinamik menü ve sekmeli detay ekranları.

-----------------------------------------------------
KURULUM & KULLANIM
-----------------------------------------------------

1. Proje dosyalarını bilgisayarınıza kopyalayın veya klonlayın.
2. `appsettings.json` içinde veritabanı bağlantısını düzenleyin.
3. Bağımlılıkları yükleyin (NuGet üzerinden: AutoMapper, EntityFramework, JWT vb.).
4. Veritabanı için migration uygulayın:
     - `dotnet ef database update`
5. Uygulamayı çalıştırın ve tarayıcıda ana sayfayı açın.

-----------------------------------------------------
TAMAMLANAN MODÜLLER
-----------------------------------------------------

- Kullanıcı/Mağaza kayıt ve giriş sistemi
- Restoran yönetimi ve menü detayları
- Sepet ve sipariş oluşturma işlevi
- JWT ile güvenli kimlik doğrulama
- Admin için temel API uçları

-----------------------------------------------------
EKSİK/KISMEN YAPILAN MODÜLLER
-----------------------------------------------------

- **Admin Paneli:** Sadece API katmanında mevcut, arayüzü oluşturulmadı.
- Sipariş geçmişi ve detaylı rapor ekranları
- Yorum ve puanlama arayüzü
- Kampanya/promosyon yönetimi

-----------------------------------------------------
GELECEKTE EKLENECEK MODÜLLER
-----------------------------------------------------

- Tam özellikli admin paneli ve yönetici işlemleri (arayüz)
- Sipariş durumu takibi ve raporlama
- Canlı kurye takibi & teslimat modülü
- Ürün stok yönetimi ve otomatik stok düşme
- Mobil uygulama desteği
- Anlık bildirim ve e-posta bilgilendirme sistemi
- Kampanya/Promosyon detaylı yönetim ekranı

-----------------------------------------------------
PROJE HAKKINDA KISA ÖZET
-----------------------------------------------------

Proje, Yemeksepeti ve benzeri platformların temel işlevlerini içerecek şekilde tasarlanmıştır. Kullanıcılar restoranlara göz atabilir, menülerden ürün ekleyebilir ve güvenli şekilde sipariş oluşturabilirler. Her rolün kendine özel panel ve yetkileri bulunur.

-----------------------------------------------------
HAZIRLAYAN
-----------------------------------------------------

Burak Bayram  
İstanbul Aydın Üniversitesi — 2025  
Yazılım Geliştirme

-----------------------------------------------------
