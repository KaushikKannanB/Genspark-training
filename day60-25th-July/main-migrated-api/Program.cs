using System.Text;
using MainMigration.Context;
using MainMigration.Interfaces;
using MainMigration.Models;
using MainMigration.Repositories;
using MainMigration.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http, // ← changed from ApiKey
        Scheme = "bearer",               // ← lowercase 'bearer' required
        BearerFormat = "JWT",
        Description = "Enter JWT token only (no need to type 'Bearer')"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});



#region DBConnection
builder.Services.AddDbContext<MainMigrationContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);
#endregion


#region Repositories

builder.Services.AddTransient<IRepository<int, User>, UserRepository>();
builder.Services.AddTransient<IRepository<int, Product>, ProductRepository>();
builder.Services.AddTransient<IRepository<int, Order>, OrderRepository>();
builder.Services.AddTransient<IRepository<int, OrderDetail>, OrderDetailRepository>();
builder.Services.AddTransient<IRepository<int, Model>, ModelRepository>();
builder.Services.AddTransient<IRepository<int, Color>, ColorRepository>();
builder.Services.AddTransient<IRepository<int, Category>, CategoryRepository>();
builder.Services.AddTransient<IRepository<int, Cart>, CartRepository>();


#endregion

#region Services

builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ICurrentUserService, CurrentUserService>();
builder.Services.AddTransient<IAuthService, AuthenticationService>();
builder.Services.AddTransient<IOtherServices, OtherServices>();
builder.Services.AddTransient<IProductService, ProductService>();
#endregion

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();