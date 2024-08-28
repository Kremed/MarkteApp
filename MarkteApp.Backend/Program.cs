using MarketApp.Backend;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder =>
        {
            builder.WithOrigins("http://localhost:26484", "http://localhost:5298", "https://localhost:7065")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

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

app.UseStaticFiles(); // Enables serving static files

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();
