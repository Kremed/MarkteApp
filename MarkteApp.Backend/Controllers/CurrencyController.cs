using MarkteApp.Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace MarkteApp.Backend.Controllers
{
    //كلاس يدعي كنترولر يتم فيه انشاء نقاط النهاية المراد الوصول اليها عبره
    //Route :api/Currency
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController(CurrenciyDbContext db) : ControllerBase
    {

        //نقطة النهاية الخاصة بأستجلاب جميع العملات من جدول العملات
        // This Endpoint Is only for Admins but we can use it for all apps
        [HttpGet("admin/getCurrencies")]
        public async Task<IActionResult> getCurrencies()
        {
            //LINQ Method syntax:
            var currenciesList = await db.Currencies
                                .Where(c => c.IsActive == true)
                                .ToListAsync();

            //LINQ Query syntax:
            //var currenciesListQ = await (from Currency in db.Currencies
            //                             where Currency.IsActive == true
            //                             select Currency)
            //                             .ToListAsync();

            return Ok(currenciesList);
        }




        //نقطة النهاية الخاصة بأضافة عملة جديدة لجدول العملات
        // This Endpoint Is only for Admins
        [HttpPost("admin/createCurrency")]
        public async Task<IActionResult> createCurrency([FromBody] Currency newCurrency)
        {

            var CurrencyFromDataBase = await db.Currencies
                                        .Where(x => x.Name == newCurrency.Name || x.Code == newCurrency.Code)
                                        .FirstOrDefaultAsync();

            if (CurrencyFromDataBase is not null)
            {
                return BadRequest("العنصر الذي تريد ادراجه موجود مسبقــا, الرجاء التأكد من الاسم والكود.");
            }


            //01) - make validation on currency =>
            var result = Currency.Validate(newCurrency);
            if (result != "OK")
            {
                return BadRequest(result);
            }

            await db.Currencies.AddAsync(newCurrency);

            await db.SaveChangesAsync();

            return Ok("تم أضافة العملية الجديدة بنجاح");
        }




        //نقطة النهاية الخاصة بحذف عملة من قاعدة البيانات
        [HttpDelete("admin/deleteCurrency")]
        public async Task<IActionResult> deleteCurrency([FromQuery] int currencyID)
        {
            var CurrencyFromDataBase = await db.Currencies
                                               .Where(x => x.Id == currencyID)
                                               .FirstOrDefaultAsync();
            if (CurrencyFromDataBase is null)
            {
                return BadRequest("العنصر الذي تريد حدفه غير موجود في قواعد البيانات !!");
            }

            db.Currencies.Remove(CurrencyFromDataBase);

            await db.SaveChangesAsync();

            return Ok("تم حذف العملة من جدول العملات بنجاح");
        }




        //نقطة نهاية خاصة بتعديل بيانات عملة في قاعدة البيانات
        [HttpPost("admin/updateCurrency")]
        public async Task<IActionResult> updateCurrency([FromBody] Currency currency)
        {
            var CurrencyFromDataBase = await db.Currencies
                                               .Where(x => x.Id == currency.Id)
                                               .FirstOrDefaultAsync();

            //01) - make validation on currency =>
            if (CurrencyFromDataBase is null)
            {
                return NotFound("العملة الذي تريد التعديل عليها غير موجودة, الرجاء ادراج العملة للتمكن من التعديل");
            }
            else if (string.IsNullOrEmpty(currency.Name))
            {
                return BadRequest("لايمكن اضافة عملة بدون أسم, الرجاء كتابة اسم العملية");
            }
            else if (string.IsNullOrEmpty(currency.Description))
            {
                return BadRequest("الرجاء تزود النظام بوصف العملة, أضافة وصف يساعد في تقديم تجربة مستخدم جيدة");
            }
            //Code نقوم بأجراء تحققات علي حقل 
            //من الضروري ان يكون الكود غير فارغا
            //من الضروري ان يكون الكود يحتوي علي 6 خانات تدل علي العملة والعملة المقابلة
            //USD X | LYD x | (USDLYD) OK | (TUNLYD) OK
            else if (string.IsNullOrEmpty(currency.Code) ||
                                         currency.Code.Length != 6)
            {

                //API REQUEST Curreny ISO Codes Provider 
                //Get All Currencis startWhit currency.Code

                return BadRequest($"كود العملة غير مطابق, الرجاء تحديد كود عملة بالحروف اللاتينية ويجب ان يتكون من 6 خانات مثلا: (TULYD)");
            }
            else if (string.IsNullOrEmpty(currency.ImageUrl) ||
                     !currency.ImageUrl.StartsWith("https://") ||
                     (!currency.ImageUrl.EndsWith(".png") && !currency.ImageUrl.EndsWith(".jpeg") && !currency.ImageUrl.EndsWith(".jpg")))
            {
                return BadRequest("صورة العملة يجب ان تكون رابط صالح, الرجاء التأكد من صورة العملة.");
            }

            //02) - Update the existing currency in the database
            CurrencyFromDataBase.Name = currency.Name;
            CurrencyFromDataBase.Description = currency.Description;
            CurrencyFromDataBase.Code = currency.Code;
            CurrencyFromDataBase.IsActive = currency.IsActive;
            CurrencyFromDataBase.ImageUrl = currency.ImageUrl;

            //CurrencyFromDataBase = currency;

            await db.SaveChangesAsync();

            return Ok($"تمت عملية تعديل البيانات الخاصة بعملة: {CurrencyFromDataBase.Name} بنجاح.");
        }

        //نقطة النهاية الخاصة بحذف سجل سعر عملة من قاعدة البيانات
        [HttpDelete("admin/deleteCurrencyPrice")]
        public async Task<IActionResult> deleteCurrencyPrice(int priceID)
        {
            var RecordFromDataBase = await db.CurrencyPrices
                                               .Where(x => x.Id == priceID)
                                               .FirstOrDefaultAsync();
            if (RecordFromDataBase is null)
            {
                return BadRequest("سجل السعر الذي تريد حذفه غير موجود الرجاء التأكد من البيانات واعادة المحاولة !!");
            }

            db.CurrencyPrices.Remove(RecordFromDataBase);

            await db.SaveChangesAsync();

            return Ok("تم حذف العملة من جدول العملات بنجاح");
        }

        //نقطة نهاية خاصة بأضافة سعر عملة في جدول الاسعار بدلالة معرف العملة من جدول العملات
        [HttpPost("admin/createCurrencyPrice/{currencyID}")]
        public async Task<IActionResult> createCurrencyPrice(
            [FromRoute] int currencyID,
            [FromBody] CurrencyPrice newPrice)
        {
            //LINQ Method syntax:
            var CurrencyFromDataBase = await db.Currencies
                                             .Where(x => x.Id == currencyID)
                                             .FirstOrDefaultAsync();

            //LINQ Query syntax:
            //var CurrencyFromDataBaseQ = await (from Currency in db.Currencies
            //                                   where Currency.Id == currencyID
            //                                   select Currency)
            //                                   .FirstOrDefaultAsync();


            if (CurrencyFromDataBase is null)
            {
                return NotFound("لايمكن أضافة سجل سعر لعملة غير موجودة, الرجاء أضافة العملة المراد اضافة السعر لها.");
            }

            newPrice.CurrencyId = CurrencyFromDataBase.Id;
            newPrice.RecordTime = DateTime.Now;   //12/08/2024 06:33:22 م
            newPrice.Price = Math.Round(newPrice.Price, 2); //bug


            //var dateTime = DateTime.Now;   //{12/08/2024 06:33:22 م}
            //var Date = DateTime.Now.Date;  //{12/08/2024 12:00:00 ص}
            //var day = DateTime.Now.Day;    //12
            //var Month = DateTime.Now.Month;//08
            //var Year = DateTime.Now.Year;  //24


            //Year Currency Records <=> سجلات العملة السنوية
            var YearCurrencyRecords = await db.CurrencyPrices
                                      .Where(x => x.CurrencyId == CurrencyFromDataBase.Id)
                                      .ToListAsync();
            //x.RecordTime.Year == DateTime.Now.Year
            //&& x.RecordTime.Month == DateTime.Now.Month &&
            //    x.RecordTime.Day == DateTime.Now.Day


            var lastPrice = YearCurrencyRecords.LastOrDefault();

            if (YearCurrencyRecords.Count() > 0 && lastPrice is not null) // lastPrice == null
            {


                var MaxPriceInYear = YearCurrencyRecords.Max(p => p.Price); // الاستعلام عن اعلي سعر في السنة

                var MinPriceInYear = YearCurrencyRecords.Min(p => p.Price); // الاستعلام عن اقل سعر في السنة

                var AveragePriceInYear = YearCurrencyRecords.Average(p => p.Price); // الاستعلام عن متوسط سعر العملة في السنة


                double currentPrice = newPrice.Price;
                double previousPrice = lastPrice.Price;

                // Calculate the Price Change Ratio
                double priceChangeRatio = (currentPrice - previousPrice);
                double percentageChange = (currentPrice - previousPrice) / previousPrice * 100;

                newPrice.Ratio = percentageChange;
            }
            else
            {
                newPrice.Ratio = 0.00;
            }

            await db.CurrencyPrices.AddAsync(newPrice);
            await db.SaveChangesAsync();


            return Ok($"تمت عملية تعديل البيانات الخاصة بعملة: {CurrencyFromDataBase.Name} بنجاح.");
        }

    }
}

