using Microsoft.EntityFrameworkCore;
using SimpleBankingAPI.Data;
using SimpleBankingAPI.Model;
using SimpleBankingAPI.Repository.Implementation;
using SimpleBankingAPI.Repository.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Register our DB service 
builder.Services.AddDbContext<BankingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlDatabaseConnection"))
);

builder.Services.Configure<SmtpMail>(builder.Configuration.GetSection("SmtpMail"));

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IBankingService, BankingService>();

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