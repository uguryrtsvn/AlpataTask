using AlpataAPI.Extentions;
using AlpataBLL;
using AlpataBLL.DependencyResolvers;
using AlpataBLL.Profiles;
using AlpataBLL.Services.Abstracts;
using AlpataBLL.Services.Concretes;
using AlpataDAL;
using AlpataDAL.IRepositories;
using AlpataDAL.Repositories;
using AlpataDAL.SeedData;
using AlpataEntities.Entities.Concretes;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AlpataDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("AlpataDAL")));

#region Auto Mapper
builder.Services.AddAutoMapper(typeof(IProfile));
#endregion



#region Cors
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(builder =>
        builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
#endregion

#region Jwt Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Token:Issuer"],
        ValidAudience = builder.Configuration["Token:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
        ClockSkew = TimeSpan.Zero,
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = async (context) =>
        {
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";

            var errorResponse = new
            {
                data = "",
                StatusCode = context.Response.StatusCode,
                Message = "401 - You are not authorized to take this action",
                Success = false,
            };
            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
        }
    };
});
#endregion

#region Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});
#endregion
#region Fluent Validation
builder.Services.AddValidatorsFromAssemblyContaining<IFluentValidator>().AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
#endregion
#region Dependencies 
builder.Services.AddDependencyResolvers(new BusinessModule());
#endregion
var app = builder.Build();

// DB Strategy  //Db yoksa runtimede oluþup daha sonra proje ayaða kaldýrýlýr.
using var scope = app.Services.CreateScope();
scope.ServiceProvider.GetRequiredService<DbInitializer>().Run();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
