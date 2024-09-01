using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace MarketApp.Shered.Dtos
{
    public class MarktingDto : MarketingAd
    {
        //[JsonIgnore]
        //[NotMapped]
        //public string? StringActiveSatus { get; set; }

        //[JsonIgnore]
        //[NotMapped]
        //public string? colorCode { get; set; }
        //public IFormFile file { get; set; } = null!; //ملف الصورة الذي نحتاجه لرفعه لخادم البيانات
    }

   
}


