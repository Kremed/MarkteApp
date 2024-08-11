using MarkteApp.Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace MarkteApp.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController(CurrenciyDbContext db) : ControllerBase
    {

        //نقطة النهاية الخاصة بأستجلاب جميع العملات من جدول العملات
        // This Endpoint Is only for Admins but we can use it for all apps
        [HttpGet("admin/getCurrencies")]
        public async Task<IActionResult> getCurrencies()
        {
                                          
            var currenciesList = await db.Currencies
                                .Where(c => c.IsActive == true)
                                .ToListAsync();

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


        [HttpDelete("admin/deleteCurrency")]
        public async Task<IActionResult> deleteCurrency(int currencyID)
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

    }
}

