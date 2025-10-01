// Program.cs
using System.Globalization;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using MotoTrackAPI.Data;

var builder = WebApplication.CreateBuilder(args);


// Program.cs
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection"));
    options.EnableSensitiveDataLogging(); // cuidado em prod
    options.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
});



// JSON BR
builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    o.JsonSerializerOptions.Converters.Add(new JsonDateTimeConverter());
    o.JsonSerializerOptions.Converters.Add(new JsonNullableDateTimeConverter());
});

// Swagger
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

    var asmName = Assembly.GetExecutingAssembly().GetName().Name;
    var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{asmName}.xml");
    if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);
});
builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());

// DbContext Oracle (sem compat, com logs em DEBUG) + MigrationsAssembly
var connStr = builder.Configuration.GetConnectionString("OracleConnection")
             ?? Environment.GetEnvironmentVariable("ORACLE_CONNSTR")
             ?? throw new InvalidOperationException("Defina OracleConnection no appsettings.json ou ORACLE_CONNSTR no ambiente.");

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseOracle(connStr, o =>
    {
        // 🔧 força o assembly onde estão as migrations (este próprio projeto)
        o.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
    });

#if DEBUG
    opt.EnableSensitiveDataLogging();
    opt.LogTo(Console.WriteLine);
#endif
});

var app = builder.Build();

// ❌ sem auto-migrate
// using var scope = app.Services.CreateScope();
// var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
// db.Database.Migrate();

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
app.UseAuthorization();
app.MapControllers();
app.Run();

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
    public override void Write(Utf8JsonWriter w, DateTime v, JsonSerializerOptions o) => w.WriteStringValue(v.ToString(Fmt, Br));
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
    public override void Write(Utf8JsonWriter w, DateTime? v, JsonSerializerOptions o) => w.WriteStringValue(v?.ToString(Fmt, Br));
}
