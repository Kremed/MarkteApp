using System;
using System.Collections.Generic;

namespace MarkteApp.Backend.Models;

public partial class Currency
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public bool IsActive { get; set; }


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
