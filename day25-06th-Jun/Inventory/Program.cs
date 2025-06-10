using System.Text;

using Inventory.Interfaces;
using Inventory.Misc;
using Inventory.Models;
// using Inventory.Hubs;

using Inventory.Repositories;
using Inventory.MiddleWare;
// using FirstAPI.Authorization;
using Inventory.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;

using Npgsql.Replication.PgOutput.Messages;
using Inventory.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();

// Add services to the container
builder.Services.AddControllers();

// Register DbContext
builder.Services.AddDbContext<InventoryContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
#region  Repositories
builder.Services.AddTransient<IRepository<string, Manager>, ManagerRepository>();
builder.Services.AddTransient<IRepository<string, Admin>, AdminRepository>();
builder.Services.AddTransient<IRepository<string, User>, UserRepository>();
builder.Services.AddTransient<IRepository<string, Category>, CategoryRepository>();
builder.Services.AddTransient<IRepository<string, CategoryAddRequest>, CategoryAddRequestRepository>();

builder.Services.AddTransient<IRepository<string, Inventories>, InventoryRepository>();

builder.Services.AddTransient<IRepository<string, Product>, ProductRepository>();
builder.Services.AddTransient<IRepository<string, StockLogging>, StockUpdateRepository>();
builder.Services.AddTransient<IRepository<string, ProductUpdateLog>, ProductUpdateRepository>();
builder.Services.AddTransient<IBlacklistedTokenRepository, BlacklistedTokenRepository>();




#endregion

#region Services
builder.Services.AddTransient<IEncryptService, EncryptService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IAutheticationService, AuthenticationService>();
builder.Services.AddTransient<IAdminService, AdminService>();
builder.Services.AddTransient<ICurrentUserService, CurrentUserService>();
builder.Services.AddTransient<IManagerService, ManagerService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IProductService, ProductService>();







#endregion

// Register Swagger (optional but helpful during development)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Notify API", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    // opt.OperationFilter<AddFileUploadParamsOperationFilter>();
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // This disables reference preservation
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;

        // Optional: Makes JSON pretty but not required
        options.JsonSerializerOptions.WriteIndented = true;
    });


#region AuthenticationFilter
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Keys:JwtTokenKey"]))
                    };
                });
#endregion
var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<TokenBlacklistMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

