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

        [HttpPost("CreateCurrency")]
        public async Task<IActionResult> CreateCurrency([FromBody] Currency NewCurrency)
        {

            var SameData = await db.Currencies
                       .Where(x => x.Name == NewCurrency.Name || x.Code == NewCurrency.Code)
                       .ToListAsync();

            if (SameData.Count() != 0)
            {
                return BadRequest("العنصر الذي تريد ادراجه موجود مسبقــا");
            }

            await db.Currencies.AddAsync(NewCurrency);
            await db.SaveChangesAsync();
            return Ok("تم أضافة العملية الجديدة بنجاح");

        }
    }
}
