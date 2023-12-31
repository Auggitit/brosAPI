using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AuggitAPIServer.Data;
using BoldReports.Web;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AuggitAPIServerContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("con") ?? throw new InvalidOperationException("Connection string 'con' not found.")));

// Add services to the container.
//CROS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
           policy =>
           {
               policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();               
               policy.WithOrigins("https://auggit.brositecom.com").AllowAnyMethod().AllowAnyHeader();
               policy.WithOrigins("https://auggitdev.brositecom.com").AllowAnyMethod().AllowAnyHeader();
           });
});

Bold.Licensing.BoldLicenseProvider.RegisterLicense("uhKE/WWLnan0+1dw/f7DswNc0l55GojH19qJQ8C30xw=");

ReportConfig.DefaultSettings = new ReportSettings().RegisterExtensions(new List<string> { "BoldReports.Data.PostgreSQL" });

builder.Services.AddMemoryCache();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
