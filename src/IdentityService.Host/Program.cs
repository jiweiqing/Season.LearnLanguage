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

var builder = WebApplication.CreateBuilder(args);

// ע�����ģ���µķ���
builder.Services.RunModuleInitializers();

// Add services to the container.

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

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); ;
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
    // TODO: API �汾����
    options.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "ѧӢ��Api�ĵ�",
        Version = "v1",
        Description = "ѧӢ��Api�ĵ�v1"
    });

    // ��OpenApiSecurityScheme��id�����
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
});

// authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // https Ĭ��Ϊtrue
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
            ClockSkew = TimeSpan.FromMinutes(1) // ƫ��
        };

        // options.Events �¼�����
    });

// memory cache
builder.Services.AddMemoryCache();

// todo: redis

// ���ӿ���ӷ���ֵ����
// builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IApplicationModelProvider, ProduceResponseTypeModelProvider>());

// auto mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// ����ģ��У�鷵�ش����ʽ
//builder.Services.ConfigureApiBehaviorOptions();

// ����ʱ���ʽ
builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter());
});

// mediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

// forward headersOptions
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.All;
});

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.Configure<JwtTokenOptions>(builder.Configuration.GetSection(JwtTokenOptions.SectionName));
// TODO: AddFluentValidation

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IdentityService.WebAPI v1"));
}

app.UseCors("AllCrosDomainsPolicy");

app.UseForwardedHeaders();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseCurrentUser();

app.UseAuthorization();

app.MapControllers();

using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;

    var seederService = services.GetRequiredService<IIdentitySeederService>();
    await seederService.SeedAsync();
}

app.Run();
