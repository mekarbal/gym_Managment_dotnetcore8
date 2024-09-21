using System.Text;
using GymManagment.Connection;
using GymManagment.Services.Interfaces;
using GymManagment.Services;
using Microsoft.EntityFrameworkCore;
using GymManagement.Services;
using GymManagment.Configs;
using GymManagment.Models.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using GymManagment.Seeds;

var builder = WebApplication.CreateBuilder(args);
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();



builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
        };

    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("ADMIN"));
    options.AddPolicy("UserOrAdmin", policy =>
        policy.RequireRole("ADMIN", "USER"));
});
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 23))));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddScoped<DataSeeder>(); // Register DataSeeder in DI

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    builder.Logging.AddConsole();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var seeder = services.GetRequiredService<DataSeeder>();
    seeder.SeedAdminUser();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();