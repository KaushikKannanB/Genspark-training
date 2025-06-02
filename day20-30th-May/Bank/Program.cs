using Bank.Contexts;
using Bank.Interfaces;
using Bank.Repositories;
using Bank.Services;
using Microsoft.EntityFrameworkCore;
using Bank.Misc;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext with connection string (update with your actual connection string)
builder.Services.AddDbContext<BankContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IFAQRepository, FAQRepository>();

// Register services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITransactionServices, TransactionService>();
builder.Services.AddScoped<IFAQServices, FAQServices>();
builder.Services.AddHttpClient<IFAQServices, FAQServices>();


// builder.Services.AddScoped<IUserServices, UserService>();  // if you have IUserServices and UserService
builder.Services.AddSingleton<UserInteractionMapper>(); // ✅ Required

// Add controllers (if you use MVC controllers)
builder.Services.AddControllers();

// Add swagger if you want API docs/testing UI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
