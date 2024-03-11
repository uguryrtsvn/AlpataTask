using AlpataAPI.Extentions;
using AlpataAPI.Extentions.SeriLog;
using AlpataAPI.OpenApiOptions;
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
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Data;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
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

#region API VERSIONING  
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                    new HeaderApiVersionReader("x-api-version"),
                                                    new MediaTypeApiVersionReader("x-api-version"));
});

builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});
#endregion
#region Swagger 
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(options =>
{

    //JWT
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



//#region SeriLog
//var columnOptions = new ColumnOptions
//{
//    AdditionalColumns = new[]
//    {
//        new SqlColumn
//        {
//            ColumnName = "UserId",
//            DataType = SqlDbType.UniqueIdentifier,
//            AllowNull = true
//        }
//    }
//};
//columnOptions.Store.Remove(StandardColumn.MessageTemplate);
//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Override("AlpataDAL.AppDbContext", LogEventLevel.Information)
//    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Error)
//    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
//    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Error)
//    .MinimumLevel.Override("System.Net.Http", LogEventLevel.Error)
//    .Enrich.FromLogContext()
//    .WriteTo.MSSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
//        new MSSqlServerSinkOptions()
//        {
//            AutoCreateSqlTable = true,
//            TableName = "ApiLogs",
//            BatchPeriod = TimeSpan.FromSeconds(10),
//            BatchPostingLimit = 1000
//        },
//        columnOptions: columnOptions).CreateLogger();
//#endregion



//
builder.Services.AddHealthChecks();

var app = builder.Build();
app.MapHealthChecks("/Healthy");
// DB Strategy  //Db yoksa runtimede oluþup daha sonra projeyi ayaða kaldýrýlýr.
using (var scope = app.Services.CreateScope())
{ 
    scope.ServiceProvider.GetRequiredService<DbInitializer>().Run();
}


// Configure the HTTP request pipeline.

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
    {
        var url = $"/swagger/{description.GroupName}/swagger.json";
        var name = description.GroupName.ToUpperInvariant();
        options.SwaggerEndpoint(url, name.ToString());
    }
});
app.UseCors();


#region  Exception Logger Middleware
app.UseExceptionHandlingMiddleware();
#endregion
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
