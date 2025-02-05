using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Student_Hall_Management.Helpers;
using Student_Hall_Management.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<EmailService>();
builder.Services.AddSingleton<ITokenBlacklistRepository, TokenBlacklistRepository>();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("HallAdminPolicy", policy => policy.RequireRole("HallAdmin"));
    options.AddPolicy("StudentPolicy", policy => policy.RequireRole("Student"));
    options.AddPolicy("DSWPolicy", policy => policy.RequireRole("DSW"));
});

builder.Services.AddCors((options) =>
{
    options.AddPolicy("DevCors", (corsBuilder) =>
    {
        corsBuilder.WithOrigins("http://localhost:3000", "http://localhost:4200", "https://localhost:5174", "http://localhost:5174", "http://localhost:5173","https://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });

    options.AddPolicy("ProdCors", (corsBuilder) =>
    {
        corsBuilder.WithOrigins("http://myProductionSite.com")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });

});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        var tokenKey = builder.Configuration.GetSection("AppSettings:TokenKey").Value;
        if (string.IsNullOrEmpty(tokenKey))
        {
            throw new InvalidOperationException("TokenKey is not configured in the app settings.");
        }

        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });


builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IProfileRepository,ProfileRepository>();
builder.Services.AddScoped<IHomePageRepository, HomePageRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IComplaintRepository, ComplaintRepository>();
builder.Services.AddScoped<INoticeRepository, NoticeRepository>();
builder.Services.AddScoped<IPresentDateTime,PresentDateTime>();
builder.Services.AddScoped<IHallDetailsManagementRepository, HallDetailsManagementRepository>();
builder.Services.AddScoped<IOverviewRepository,OverviewRepository>();
builder.Services.AddScoped<IAdminRoomRepository, AdminRoomRepository>();
builder.Services.AddScoped<IStudentManagementRepository, StudentManagementRepository>();
builder.Services.AddScoped<INoticeManagementRepository,NoticeManagementRepository>();
builder.Services.AddScoped<IComplaintManagementRepository, ComplaintManagementRepository>();
builder.Services.AddScoped<IAdminPaymentRepository, AdminPaymentRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("DevCors");
}
else
{
    app.UseCors("DevCors");
}

app.UseHttpsRedirection();

app.UseMiddleware<TokenBlacklistMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
