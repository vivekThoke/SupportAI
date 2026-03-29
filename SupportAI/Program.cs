using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SupportAI.Application.Auth.Commands;
using SupportAI.Infrastructure;
using Microsoft.IdentityModel.Tokens;

var key = Encoding.UTF8.GetBytes("SUPER_SECRET_KEY_123_AROUND_THE_WORLD_TRAVEL_WITH_ONE_PURPOSE_TO_MAKE_NEW_THINGS_THAT_MAKE_THIS_THINGS_COME_TRUE");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(RegisterTenantCommand).Assembly));


builder.Services.AddInfrastructure(
    builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,

        IssuerSigningKey = new SymmetricSecurityKey(key),
    };
});

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("TenantHeader", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "X-Tenant-Name",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Description = "Tenant Name Header"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "TenantHeader"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
