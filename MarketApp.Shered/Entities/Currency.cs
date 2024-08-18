using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MarketApp.Shered;


public partial class Currency
{
    //[Required(ErrorMessage = "حقل ID حقل مطلوب, الرجاء التحقق من البيانات واعادة المحاولة.")]
    //[Range(1, 15, ErrorMessage = "سعر العملة مبالغ فيه, الرجاء تحديد السعر بين 1 و 15.")]
    public int Id { get; set; }


    [Required(ErrorMessage = "Name is required.")]
    [StringLength(50, ErrorMessage = "لايمكن ان يكون طول الاسم اطول من 50 حرف, الرجاء تغير الاسم الخاص بالعملة")]
    //[EmailAddress(ErrorMessage = "هذا الحقل ليس بريد الكتروني, الرجاء اعادة المحاولة ببيانات صحيحة.")]
    public string Name { get; set; } = null!;


    [Required(ErrorMessage = "الوصف مطلوب الرجاء كتابة وصف لتحسين تجربة المستخدم في التطبيق.")]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = "كود العملة اجباري.")]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "كود العملة يجب ان يتكون من 6 حروف فقط")]
    [RegularExpression(@"^[A-Z]{6}$", ErrorMessage = "يجب ان يتكون الكود من 6 حروف انجليزية كبيرة, الارقام والحروف الصغيرة لاتقبل")]
    public string Code { get; set; } = null!;

    [Required(ErrorMessage = "الرجاء أضافة رابط الصورة ,هذا الحقل مطلوب لايمكن تركه فارغأ.")]
    [Url(ErrorMessage = "الصورة يجب ان تكون برابط صالح.")]
    [RegularExpression(@"^https?:\/\/.*\.(jpg|jpeg|png|gif|bmp|webp)$", ErrorMessage = "الرابط يجب ان يكون لصورة مثلا: (jpg, jpeg, png, gif, bmp, webp).")]
    public string ImageUrl { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTime insertedDate { get; set; }

    public List<CurrencyPrice> currencyPrices = new();

    public static string Validate(Currency currency)
    {
        if (string.IsNullOrEmpty(currency.Name))
        {
            return "لايمكن اضافة عملة بدون أسم, الرجاء كتابة اسم العملية";
        }
        else if (string.IsNullOrEmpty(currency.Description))
        {
            return "الرجاء تزود النظام بوصف العملة, أضافة وصف يساعد في تقديم تجربة مستخدم جيدة";
        }
        else if (string.IsNullOrEmpty(currency.Code) ||
                                     currency.Code.Length != 6)
        {
            return $"كود العملة غير مطابق, الرجاء تحديد كود عملة بالحروف اللاتينية ويجب ان يتكون من 6 خانات مثلا: (TULYD)";
        }
        else if (string.IsNullOrEmpty(currency.ImageUrl) ||
                 !currency.ImageUrl.StartsWith("https://") ||
                 (!currency.ImageUrl.EndsWith(".png") && !currency.ImageUrl.EndsWith(".jpeg") && !currency.ImageUrl.EndsWith(".jpg")))
        {
            return "صورة العملة يجب ان تكون رابط صالح, الرجاء التأكد من صورة العملة.";
        }

        return "OK";
    }

}
