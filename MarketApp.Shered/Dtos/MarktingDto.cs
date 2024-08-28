using Microsoft.AspNetCore.Http;

namespace MarketApp.Shered.Dtos
{
    public class MarktingDto : MarketingAd
    {
        public IFormFile file { get; set; } = null!; //ملف الصورة الذي نحتاجه لرفعه لخادم البيانات
    }
}
