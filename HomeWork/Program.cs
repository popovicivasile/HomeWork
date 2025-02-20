using HomeWork.Data;
using HomeWork.Data.Domain;
using HomeWork.Data.Repository.Abstract;
using HomeWork.Data.Repository.Real;
using HomeWork.Data.ServicesCall;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();



var builder = WebApplication.CreateBuilder(args);

DatabaseInitializer.ApplyMigrations<DentalDbContext>(configuration.GetConnectionString("DbConnection"));
builder.Services.AddControllers();
builder.Services.AddDbContext<DentalDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("DbConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ISecurityRepository, SecurityRepository>();
builder.Services.AddScoped<MailService>();
builder.Services.AddIdentity<UserRegistration, IdentityRole>(
                options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                }).AddEntityFrameworkStores<DentalDbContext>();
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
