var builder = WebApplication.CreateBuilder(args);

// CORS Ayarları
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

// Controller'ları ekle
builder.Services.AddControllers();

var app = builder.Build();

// CORS'u aktif et
app.UseCors("AllowAll");

// HTTPS yönlendirmesi
app.UseHttpsRedirection();

// Controller route'larını aktif et
app.MapControllers();

app.Run();