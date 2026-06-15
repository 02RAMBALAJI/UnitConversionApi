using System.Reflection;
using UnitConversionApi.Middleware;
using UnitConversionApi.Services;

var builder = WebApplication.CreateBuilder(args);

// -------------------------------------------------
// Service Registration
// -------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title       = "Unit Conversion API",
        Version     = "v1",
        Description = "A RESTful API for converting numerical values between different units of measurement " +
                      "(length, temperature, weight/mass, and more)."
    });

    // Include XML comments in Swagger UI
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);
});

// UnitRegistryService is singleton — unit definitions are static data.
// UnitConversionService is scoped — stateless, but may hold per-request context in future.
builder.Services.AddSingleton<IUnitRegistryService, UnitRegistryService>();
builder.Services.AddScoped<IUnitConversionService, UnitConversionService>();

var app = builder.Build();

// -------------------------------------------------
// Middleware Pipeline
// -------------------------------------------------
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Swagger is available in all environments for convenience.
// Restrict to Development only in production deployments if needed.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Unit Conversion API v1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Expose Program for integration test projects
public partial class Program { }
