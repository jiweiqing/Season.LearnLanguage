using Learning.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.HttpOverrides;
using Learning.Domain;
using Listening.Infrastructure;
using System.Text.Json;
using Learning.Infrastructure;

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
builder.Services.AddDbContext<ListeningDbContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(builder.Configuration["Mysql:Version"]));
});

builder.Services.AddControllers();
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
        Title = "听力服务Api文档",
        Version = "v1",
        Description = "听力服务Api文档v1"
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
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.KebabCaseUpper;
});

// mediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

// forward headersOptions
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.All;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FileService.WebAPI v1"));
}

app.UseCors("AllCrosDomainsPolicy");

app.UseAppExceptionHandler(app.Environment);

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseStaticFiles();

app.UseAuthentication();

app.UseCurrentUser();

app.UseAuthorization();

app.MapControllers();

app.UseAutoSaveChange<ListeningDbContext>();

app.MapControllers();

app.Run();
