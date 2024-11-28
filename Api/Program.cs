using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Application;
using Application.Interfaces;
using Application.Mapping;
using Application.Services;
using Application.Settings;
using Infrastructure.Background.ChapterBackground;
using Infrastructure.Background.CommentBackground;
using Infrastructure.Background.EmailBackgroundService;
using Infrastructure.Background.TagBackground;
using Infrastructure.Background.TagBackground.UserTagBackground;
using Infrastructure.Background.WorkBackground;
using Infrastructure.Background.WorkBackground.WorkLikeBackground;
using Infrastructure.Dal.EntityFramework;
using Infrastructure.Dal.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fanfic API", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    c.MapType<DateTime>(() => new OpenApiSchema { Type = "string", Format = "date" });
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { 
            new OpenApiSecurityScheme 
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "bearer",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        var enumConverter = new JsonStringEnumConverter(allowIntegerValues: false);
        options.JsonSerializerOptions.Converters.Add(enumConverter);
    });

builder.Services.AddScoped<IWorkRepository, WorkRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IChapterRepository, ChapterRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IWorkTagRepository, WorkTagRepository>();
builder.Services.AddScoped<IWorkLikeRepository, WorkLikeRepository>();
builder.Services.AddScoped<IUserTagRepository, UserTagRepository>();

builder.Services.AddScoped<WorkService>();
builder.Services.AddScoped<TagService>();
builder.Services.AddScoped<CommentService>();
builder.Services.AddScoped<ChapterService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<WorkTagService>();
builder.Services.AddScoped<WorkLikeService>();
builder.Services.AddScoped<UserTagService>();
builder.Services.AddScoped<AddToCache>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<EmailMessages>();
builder.Services.AddScoped<BotMessage>();

builder.Services.AddAutoMapper(typeof(WorkMappingProfile));
builder.Services.AddAutoMapper(typeof(TagMappingProfile));
builder.Services.AddAutoMapper(typeof(CommentMappingProfile));
builder.Services.AddAutoMapper(typeof(ChapterMappingProfile));
builder.Services.AddAutoMapper(typeof(UserMappingProfile));
builder.Services.AddAutoMapper(typeof(WorkTagMappingProfile));
builder.Services.AddAutoMapper(typeof(WorkLikeMappingProfile));
builder.Services.AddAutoMapper(typeof(UserTagMappingProfile));

builder.Services.AddHostedService<CreateTagBackgroundService>();
builder.Services.AddHostedService<UpdateTagBackgroundService>();
builder.Services.AddHostedService<UserTagBackgroundService>();

builder.Services.AddHostedService<CreateWorkBackgroundService>();
builder.Services.AddHostedService<UpdateWorkBackgroundService>();
builder.Services.AddHostedService<WorkLikeBackgroundService>();

builder.Services.AddHostedService<CreateChapterBackgroundService>();
builder.Services.AddHostedService<UpdateChapterBackgroundService>();

builder.Services.AddHostedService<CreateCommentBackgroundService>();
builder.Services.AddHostedService<UpdateCommentBackgroundService>();

builder.Services.AddHostedService<SendEmailBackgroundService>();


var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    if (key != null)
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    }
});

builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.Configure<GoogleSettings>(builder.Configuration.GetSection(nameof(GoogleSettings)));
builder.Services.AddScoped<GoogleCloudService>();


builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetSection("Redis:Configuration").Value;
    options.InstanceName = builder.Configuration.GetSection("Redis:InstanceName").Value;
});

var redisConfig = builder.Configuration.GetSection("Redis:Configuration").Value;
Console.WriteLine($"Redis Configuration: {redisConfig}");

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5120);
    serverOptions.ListenAnyIP(80);
    serverOptions.ListenAnyIP(443);
});

var connectionString = builder.Configuration.GetConnectionString("FanficDatabase");
Console.WriteLine(connectionString);

builder.Services.AddDbContext<FanficSiteDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        corsPolicyBuilder =>
        {
            corsPolicyBuilder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Logging.AddConsole();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.MapGet("/api/sample", () => "Hello, World!");

app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Content-Security-Policy", "frame-ancestors 'self' https://oauth.telegram.org;");
    await next();
});

app.Run();