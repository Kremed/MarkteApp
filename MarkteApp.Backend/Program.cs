using MarkteApp.Backend.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
                //.ConfigureApiBehaviorOptions(options =>
                // {
                //     options.SuppressModelStateInvalidFilter = true;
                // });

//جلب جملة الاتصال من ملف الاعدادات في مشروعنا [appsettings.json]
//ووضعها في متغير من نوع ستريج [String]
string? DefaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");

//تهئية المشروع لخدمة قواعد البيانات واضافة الخدمة في حاوية حقن التبعية

builder.Services.AddDbContext<CurrenciyDbContext>(opt => opt.UseSqlServer(DefaultConnection));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
