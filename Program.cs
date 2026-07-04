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
builder.Services.Configure<BankInfo>(builder.Configuration.GetSection("BankInfo"));

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IBankingService, BankingService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod());
});


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("Frontend");

app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

app.Run();