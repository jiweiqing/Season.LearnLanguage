using Leaning.EventBus;
using Learning.AspNetCore;
using Learning.Infrastructure;
using Listening.Admin.Host;
using Listening.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using StackExchange.Redis;
using System.Reflection;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// ע�����ģ���µķ���
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

builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);
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
        Title = "��������Api�ĵ�",
        Version = "v1",
        Description = "��������Api�ĵ�v1"
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

    string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
    string[] xmlFiles = Directory.GetFiles(path, "*.xml");
    foreach (string xmlFile in xmlFiles)
    {
        options.IncludeXmlComments(xmlFile, true);
    }
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

// ���ӿ���ӷ���ֵ����
builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IApplicationModelProvider, ProduceResponseTypeModelProvider>());
// auto mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// ����ģ��У�鷵�ش����ʽ
builder.Services.ConfigureApiBehaviorOptions();

// ����ʱ���ʽ
builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter());
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
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

// ת�������
builder.Services.AddScoped<EncodingEpisodeHelper>();
// event bus
// rabbit mq options
builder.Services.Configure<IntegrationEventRabbitMQOptions>(builder.Configuration.GetSection("RabbitMQ"));
// todo:���򼯵Ļ�ȡ��,��Ҫ�Ż�
builder.Services.AddEventBus("Listening.Admin", ServiceCollectionExtension.assemblies);

// Redis
//Redis������ TODO: �����ļ����� rabbit�Լ�redis��ص�����
string redisConnStr = builder.Configuration.GetValue<string>("Redis:ConnectionString")!;
ConnectionMultiplexer redisConnMultiplexer = ConnectionMultiplexer.Connect(redisConnStr);
builder.Services.AddSingleton(typeof(IConnectionMultiplexer),redisConnMultiplexer);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/listening/swagger/v1/swagger.json", "FileService.WebAPI v1"));
}

// �¼�����
app.UseEventBus();

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
