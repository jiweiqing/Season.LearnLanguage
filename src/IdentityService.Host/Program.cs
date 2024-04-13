using IdentityService.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using System.Text.Json.Serialization;
using MediatR;
using IdentityService.Host;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.HttpOverrides;
using IdentityService.Domain;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Reflection;
using Learning.AspNetCore;
using Learning.Domain;
using Learning.Infrastructure;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// 注册各个模块下的服务
builder.Services.RunModuleInitializers();

// Add services to the container.
builder.Services.AutomaticRegisterService();

// log
builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{
    loggerConfiguration
    .ReadFrom.Configuration(hostingContext.Configuration)
    .Enrich.FromLogContext();
});

// dbcontext
builder.Services.AddDbContext<IdentityServiceDbContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(builder.Configuration["Mysql:Version"]));
});

builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);
//.AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHealthChecks();

// Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllCrosDomainsPolicy", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});
// swagger
builder.Services.AddSwaggerGen(options =>
{
    // TODO: API 版本控制
    options.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "学英语Api文档",
        Version = "v1",
        Description = "学英语Api文档v1"
    });

    // 与OpenApiSecurityScheme的id相关联
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "JSON Web Token to access resources. Example: Bearer {token}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Authorization"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
       {
         new OpenApiSecurityScheme
         {
            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
            //Scheme = "Authorization",
            //Name = "oauth2",
            //In = ParameterLocation.Header
         },
         new [] { string.Empty }
       }
    });

    string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
    string[] xmlFiles = Directory.GetFiles(path, "IdentityService.*.xml");
    foreach (string xmlFile in xmlFiles)
    {
        options.IncludeXmlComments(xmlFile, true);
    }
});

// authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // https 默认为true
        //options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecurityKey"]!)),
            ClockSkew = TimeSpan.FromMinutes(1) // 偏差
        };

        // options.Events 事件处理
    });

// memory cache
builder.Services.AddMemoryCache();

// todo: redis

// 给接口添加返回值描述
builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IApplicationModelProvider, ProduceResponseTypeModelProvider>());

// auto mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// 配置模型校验返回错误格式
builder.Services.ConfigureApiBehaviorOptions();

// 配置时间格式
builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter());
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// mediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

// forward headersOptions
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.All;
});

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));
// TODO: AddFluentValidation

// service

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IdentityService.WebAPI v1"));
}

app.UseCors("AllCrosDomainsPolicy");

app.UseAppExceptionHandler(app.Environment);

//app.UseForwardedHeaders(new ForwardedHeadersOptions
//{
//    ForwardedHeaders = ForwardedHeaders.All
//});

//app.UseHttpsRedirection();

app.UseAuthentication();

app.UseCurrentUser();

app.UseAuthorization();

app.MapControllers();

app.UseAutoSaveChange<IdentityServiceDbContext>();

using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;

    var seederService = services.GetRequiredService<IIdentitySeederService>();
    await seederService.SeedAsync();
}

app.Run();
