using FluentValidation;
using UnitConversion.Api.Contracts;
using UnitConversion.Api.Middleware;
using UnitConversion.Api.Validation;
using UnitConversion.Application.Converters;
using UnitConversion.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Controllers + validation
builder.Services.AddControllers();
builder.Services.AddScoped<IValidator<ConversionRequest>, ConversionRequestValidator>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Unit Conversion API", Version = "v1" });
});

// Application services — register one converter per category.
// Adding a new category = add one line here + one new IUnitConverter class.
builder.Services.AddSingleton<IUnitConverter, LengthConverter>();
builder.Services.AddSingleton<IUnitConverter, WeightConverter>();
builder.Services.AddSingleton<IUnitConverter, TemperatureConverter>();
builder.Services.AddSingleton<IConversionService, ConversionService>();

// CORS (adjust for real deployments — this is permissive for local dev)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCors();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Exposed for WebApplicationFactory-based integration tests
public partial class Program { }