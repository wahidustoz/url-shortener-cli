
# Ilmhub URL Shortener - Loyiha shartlari
---

## Asosiy Funktsional Talablar

1. **URL qisqartirish:**  
- Foydalanuvchi uzun URL manzilini kiritsa, ilova o'ziga xos va iloji boricha qisqa kodni yaratadi. 
- Qisqartirilgan URL kodining uzunligi faqat zarur bo'lsa oshadi, ya'ni avvalgi qisqartirilgan URL lar soniga qarab.
- Agar bir xil URL bir necha marta qisqartirilsa, har safar bir xil kod qaytariladi.

2. **Asl URL ni olish:**  
- Foydalanuvchi qisqartirilgan kodni kiritganda, ilova asl URL manzilini qaytarishi kerak.
- Agar kod mavjud bo'lmasa, xato xabari ko'rsatiladi.

3. **Barcha qisqartirilgan URL'larni ko'rsatish (sahifalab ko'rsatish):**  
- Barcha qisqartirilgan URL'larni ko'rsatishda, har safar 10 URL'dan ko'rsatilishi va sahifalar o'rtasida o'tish imkoniyati bo'lishi kerak.
- O'ng yoki chap strelkali tugmalar bilan navigatsiya qilish

4. **Spectre.Console bilan yaxshilangan interfeys:**  
- `Spectre.Console` paketidan foydalanib, foydalanuvchi interfeysini yanada chiroyli qilish.
- Qisqartirish, URL olish va ko'rsatish funksiyalari uchun aniq va tushunarli interfeys taqdim etilishi kerak.

5. **QR Kod Yaratish:**  
- Har bir yangi qisqartirilgan URL uchun foydalanuvchi QR kodini yaratish imkoniyatiga ega bo'lishi kerak.
- QR kodni imkoni bo'lsa terminalda ko'rsating (Spectra.Console orqali iloji bor)
- Agar konsoleda ko'rsatishni eplolmasangiz, faylga saqlab fayl manzilini ko'rsating
- QR kod yaratish uchun `Net.Codecrete.QrCodeGenerator` paketidan foydalaning.

6. **Qisqartirilgan URL uchun maxsus Hostname:**  
   Qisqartirilgan URL'larning asosiy _hostname_ ini quyidagilar orqali sozlash mumkin bo'lishi kerak:
   - `HOSTNAME` env. variable o'zgaruvchisidan o'qish. *[👉 Batafsil](https://learn.microsoft.com/en-us/dotnet/api/system.environment.getenvironmentvariable?view=net-8.0)*
   - CommandLine argumentidan o'qish. *[👉 Batafsil](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/program-structure/main-command-line)*
   - Agar hostname `env. variable` yoki `CommandLine argument`dan topilmasa, default qiymat sifatida `https://url.ilmhub.uz` ishlatiladi.

7. **Ma'lumotlarni xotirada saqlash:**  
- URL'larni saqlash va olish uchun samarali ma'lumotlar strukturasidan foydalanish kerak.
- Aql bilan istalgan tip tanlang, masalan Dictionary.
---

## Qo'shimcha Talablar

- **Console Interfeysi**: Interfeys foydalanuvchi uchun qulay va tushunarli bo'lishi kerak.
- **Exception Handling**: Noto'g'ri kirishlar bo'lsa, xatoliklarni samarali usulda qayta ishlash va foydalanuvchiga ma'lumot berish kerak.
---

## Interface'lar
Quyidagi interfacelar (kontraktlar) asosida implementatsiya qiling.

#### 1. `IUrlShortener`

URL qisqartirish va olish uchun asosiy interfeys.

```csharp
public interface IUrlShortener
{
    string ShortenUrl(string longUrl);
    string GetOriginalUrl(string shortCode);
    IEnumerable<Url> GetPaginatedUrls(int pageNumber, int pageSize = 10); // Paginasiyalashni qo'llab-quvvatlash
}
```

#### 2. `IShortCodeGenerator`

Qisqa kodni yaratish uchun interfeys.

```csharp
public interface IShortCodeGenerator
{
    string GenerateShortCode(string longUrl);
}
```

#### 3. `IUrlDisplayService`

Paginasiyalangan URL'larni ko'rsatish va konsolda chiqarish uchun interfeys.

```csharp
public interface IUrlDisplayService
{
    void DisplayUrls(IEnumerable<Url> urls);
    void DisplayPaginatedUrls(int pageNumber);
}
```

#### 4. `IQrCodeService`

QR kodlarni yaratish uchun interfeys.

```csharp
public interface IQrCodeService
{
    void GenerateQrCode(string shortUrl);
}
```

#### 5. `IConfigService`

Konfiguratsiya (hostname olish) bilan ishlash uchun interfeys.

```csharp
public interface IConfigService
{
    string GetBaseHostname();
}
```
---

## Xotiradagi Ma'lumotlarni Saqlash Uchun Taklif Qilingan Ma'lumotlar Strukturalari
> Quyidgai ma'lumotlar faqatgina yordam sifatida taqdim etilyapti. Bu qismiga to'liq rioya qilmasdan boshqacha implementatsiya qilish mumkin. 

1. **URL Mapping uchun Dictionary:**  
   `Dictionary<string, Url>` dan foydalaning, bu yerda `shortCode` kalit bo'ladi va `Url` obyekti qiymat bo'ladi. Bu URL'larni O(1) vaqt bilan olish imkonini beradi.

2. **Izlash uchun Dictionary:**  
   Ikkinchi `Dictionary<string, string>` yaratish kerak, bu `longUrl` ni `shortCode` ga moslashtiradi. Bu bir xil URL'lar uchun bir xil qisqartirilgan kodni ta'minlashga yordam beradi.

3. **Malumotni saqlash uchun model:**

   ```csharp
   public class Url
   {
       public string OriginalUrl { get; set; }
       public string ShortCode { get; set; }
       public int AccessCount { get; set; } = 0; // Qisqartirilgan URL har safar murojaat qilganda oshadi
       public DateTime DateCreated { get; set; } = DateTime.UtcNow;
   }
   ```

4. **Paginasiyalash uchun List:**  
   URL'larni ko'rsatishda `Dictionary` qiymatlaridan `List<Url>` ga o'tib, paginasiyalashni amalga oshirish mumkin.
---

## Rubrikalar (baholash mezonlari)

### Asosiy talablar (70%)
- **URL qisqartirish va olish:** Talaba URL qisqartirish va qisqartirilgan URL'ni olish funksiyalarini to'g'ri ishlatgan bo'lishi kerak. (30%)
- **Paginasiyalash:** URL'larni sahifalar bo'lib ko'rsatish funksiyasi to'g'ri amalga oshirilgan bo'lishi kerak. Har safar faqat 10 URL ko'rsatilishi kerak. (10%)
- **QR kod yaratish:** Har bir yangi URL qisqartirilganda, foydalanuvchi QR kodini yaratish imkoniyatiga ega bo'lishi kerak. (10%)
- **Spectre.Console bilan UI:** Ilovadagi foydalanuvchi interfeysi aniq va tushunarli bo'lishi kerak. `Spectre.Console` paketidan to'g'ri foydalanish. (10%)
- **URL Mapping:** URL'larni saqlash va olish uchun samarali ma'lumotlar strukturasidan foydalanish. (10%)

### Yordamchi talablar (20%)
- **Ma'lumotlarni saqlash:** URL'larni saqlash uchun `Dictionary` yoki boshqa samarali struktura ishlatish. (10%)
- **Kodning tozaligi va tushunarliligi:** Kod aniq, modulga ajratilgan va yaxshilangan. Har bir funksiyaning vazifasi aniq. (10%)

### Qo'shimcha Talablar (10%)
- **GitHub foydalanish:** Githudagi `https://github.com/wahidustoz/url-shortener-cli` repozitoriyasini forklagan va o'zining yakuniy ishini PR (pull request) qilib yuborgan bo'lishi kerak. (5%)
- **ReadMe va video:** Loyiha _root papkasida_ `readme.md` faylini yaratgan va unda quyidagilarni yoritgan bo'lishi kerak:
  - boshqa dasturchilar loyihani qanday qilib local ishga tushirishini va qanday ishlatishini tushintirivuchi tekst yo'riqnoma. (2%)
  - loyihaning qanday ishlashi, unda avtor qanday muammolarni hal qilgani haqidagi  2~5 daqiqalik youtubedagi video havolasini qo'shgan bo'lishi kerak. (3%)

### Qo'shimcha nuqtalar:
- **Paketlarni o'rnatish va sozlash:** Ilova uchun kerakli barcha paketlar to'g'ri o'rnatilgan va ishlashiga ishonch hosil qilingan. 

