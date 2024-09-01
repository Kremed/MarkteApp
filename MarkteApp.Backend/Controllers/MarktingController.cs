using MarketApp.Backend;
using MarketApp.Shered;
using MarketApp.Shered.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;

namespace MarkteApp.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarktingController(CurrenciyDbContext db, IWebHostEnvironment _environment) : ControllerBase
    {

        [HttpPost("admin/createAd")]
        public async Task<IActionResult> createAd([FromForm] MarketingAd marktingDto, [FromForm] IFormFile file)
        {
            try
            {

                //==========================================================================
                // (01) Save The image File To wwwroot/uploads <=> حفظ ملف الصورة في الملفات التابتة بالمشروع
                //==========================================================================

                var imageFile = file;

                if (imageFile is null || imageFile.Length == 0)
                {
                    return BadRequest("لايوجد ملف لتحميله, الرجاء ادراج ملف صورة الأعلان");
                }

                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

                if (!Directory.Exists(uploadsFolder)) //لتــأكد من ان الملف موجود فعليـا في ملفات المشروع uploads
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                //أنشاء أسم عشوائي للملف ليتم حفظه بــأسم جديد

                string GuidRandomFileName = Guid.NewGuid().ToString();
                var fileName = GuidRandomFileName.Replace("-", "") + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                // Save The File To The Path  =>
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                var imageUrl = $"https://maui.ly/uploads/{fileName}";

                //==========================================================================
                // (02) Adding The Markting To Database <=> حفظ الاعلان في جدول الاعلانات في قواعد البيانات
                //==========================================================================

                MarketingAd ad = new()
                {
                    Title = marktingDto.Title,
                    Description = marktingDto.Description,
                    FromDate = marktingDto.FromDate,
                    ToDate = marktingDto.ToDate,
                    IsActive = marktingDto.IsActive,
                    Url = marktingDto.Url,
                    CoverImage = imageUrl,
                };

                await db.MarketingAds.AddAsync(ad);

                await db.SaveChangesAsync();

                var ads = await db.MarketingAds.ToListAsync();

                return Ok(ads);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //=============================================================
        //Admin Endpoints
        //=============================================================
        [HttpGet("admin/getAds")]
        public async Task<IActionResult> getAds()
        {




            try
            {
                var ads = await db.MarketingAds.ToListAsync();
                return Ok(ads);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message + Environment.NewLine + ex.InnerException);
            }
        }

        [HttpPost("admin/deactivateAd")]
        public async Task<IActionResult> deactivateAd(int id)
        {
            var databaseAd = await db.MarketingAds.FirstOrDefaultAsync(ad => ad.Id == id);
            if (databaseAd is null)
            {
                return BadRequest("الاعلان غير موجود في قواعد البيانات, الرجاء التـأكد من معرف الاعلان واعادة المحاولة");
            }


            databaseAd.IsActive = !databaseAd.IsActive;

            await db.SaveChangesAsync();

            return Ok("تم تعديل حالة تنشيط الاعلان بنجاح");
        }


        //=============================================================
        //Clinte Endpoints
        //=============================================================
        [HttpGet("getAds")]
        public async Task<IActionResult> getClinteAds()
        {
            var ads = await db.MarketingAds
                              .Where(ad => ad.IsActive == true && DateTime.Now.Date <= ad.ToDate.Date)
                              .ToListAsync();
            return Ok(ads);
        }

    }
}
