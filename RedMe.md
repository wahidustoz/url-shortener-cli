# URL Shortener Console App

Bu loyiha URL manzillarni qisqartirish uchun konsol interfeysiga ega dasturdir. `Spectre.Console` yordamida interfeys chiroyli va qulay qilingan, `MD5` hash algoritmi esa har bir URL uchun noyob qisqartirilgan kod yaratadi.

## Loyihani Local Ishga Tushirish

### Talablar

- [.NET SDK 6.0](https://dotnet.microsoft.com/download/dotnet/6.0) yoki undan yuqori versiya o‘rnatilgan bo‘lishi kerak.

### O'rnatish va Ishga Tushirish

1. **Loyihani yuklab olish va papkaga kirish:**

   ```bash
   git clone https://github.com/username/UrlShortenerApp.git
   cd UrlShortenerApp
```
Kerakli paketlarni o‘rnatish:

2. **Terminal yoki konsolda quyidagi buyruqlarni kiriting:**

```bash
Copy code
dotnet add package Spectre.Console --version 0.44.0
dotnet add package Net.Codecrete.QrCodeGenerator --version 1.1.0
dotnet restore
```
shu bilan dastur sizning local qurilmangizda ishlashga tayyor . Xizmatizmizdan foydanganingdan hursantmiz