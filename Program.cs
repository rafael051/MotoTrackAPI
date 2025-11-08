// Program.cs
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using MotoTrackAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// ==========================
// DB (Oracle) - único AddDbContext
// ==========================
var connStr =
    builder.Configuration.GetConnectionString("OracleConnection")
    ?? Environment.GetEnvironmentVariable("ORACLE_CONNSTR")
    ?? throw new InvalidOperationException("Defina OracleConnection no appsettings.json ou ORACLE_CONNSTR nas variáveis de ambiente.");

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseOracle(connStr, o =>
    {
        o.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
    });

#if DEBUG
    opt.EnableSensitiveDataLogging();
    opt.LogTo(Console.WriteLine);
#endif
});

// ==========================
// Controllers + JSON BR
// ==========================
builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    o.JsonSerializerOptions.Converters.Add(new JsonDateTimeConverter());
    o.JsonSerializerOptions.Converters.Add(new JsonNullableDateTimeConverter());
});

// ==========================
// Versionamento de API
// ==========================
builder.Services.AddApiVersioning(opt =>
{
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("x-api-version"),
        new QueryStringApiVersionReader("api-version")
    );
});
builder.Services.AddVersionedApiExplorer(opt =>
{
    opt.GroupNameFormat = "'v'VVV"; // v1, v1.1...
    opt.SubstituteApiVersionInUrl = true;
});

// ==========================
// Auth (JWT) + Authorization
// ==========================
string jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "MotoTrack";
string jwtAudience = builder.Configuration["Jwt:Audience"] ?? Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "MotoTrackClients";
string jwtKey = builder.Configuration["Jwt:Key"] ?? Environment.GetEnvironmentVariable("JWT_KEY") ?? "change-me-dev-only";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
#if DEBUG
        // útil para testes locais sem HTTPS configurado
        opt.RequireHttpsMetadata = false;
#endif
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero // tokens expiram na hora certa
        };
    });
builder.Services.AddAuthorization();

// ==========================
// Health Checks
// ==========================
builder.Services.AddHealthChecks();

// ==========================
// Swagger
// ==========================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MotoTrack API",
        Version = "v1",
        Description = "API da Locadora de Motos (com exemplos de request/response)."
    });

    c.EnableAnnotations();
    c.ExampleFilters();

    // XML comments (habilite em Propriedades do projeto -> Build -> XML documentation file)
    var asmName = Assembly.GetExecutingAssembly().GetName().Name!;
    var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{asmName}.xml");
    if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);

    // JWT no Swagger (Authorize)
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Bearer token. Ex: **Bearer {seu_token}**",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
    };
    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
});
builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());

var app = builder.Build();

// ==========================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        o.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        o.DisplayRequestDuration();
        o.DefaultModelExpandDepth(0);
        o.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Model);
        o.EnableFilter();
        o.EnablePersistAuthorization();
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Health endpoint (para a rubrica da disciplina)
app.MapHealthChecks("/health");

app.Run();

// ==========================
// Converters
// ==========================
public class JsonDateTimeConverter : JsonConverter<DateTime>
{
    private const string Fmt = "dd/MM/yyyy HH:mm:ss";
    private static readonly CultureInfo Br = new("pt-BR");

    public override DateTime Read(ref Utf8JsonReader r, Type t, JsonSerializerOptions o)
    {
        var s = r.GetString();
        if (string.IsNullOrWhiteSpace(s)) return default;
        if (DateTime.TryParseExact(s, Fmt, Br, DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces, out var dt)) return dt;
        return DateTime.Parse(s!, Br);
    }

    public override void Write(Utf8JsonWriter w, DateTime v, JsonSerializerOptions o) =>
        w.WriteStringValue(v.ToString(Fmt, Br));
}

public class JsonNullableDateTimeConverter : JsonConverter<DateTime?>
{
    private const string Fmt = "dd/MM/yyyy HH:mm:ss";
    private static readonly CultureInfo Br = new("pt-BR");

    public override DateTime? Read(ref Utf8JsonReader r, Type t, JsonSerializerOptions o)
    {
        if (r.TokenType == JsonTokenType.Null) return null;
        var s = r.GetString();
        if (string.IsNullOrWhiteSpace(s)) return null;
        if (DateTime.TryParseExact(s, Fmt, Br, DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces, out var dt)) return dt;
        return DateTime.Parse(s!, Br);
    }

    public override void Write(Utf8JsonWriter w, DateTime? v, JsonSerializerOptions o) =>
        w.WriteStringValue(v?.ToString(Fmt, Br));
}

// Necessário para testes de integração (WebApplicationFactory)
public partial class Program { }
