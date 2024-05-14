using MiddleApi.Filters;
using MiddleApi.Models;
using MiddleApi.Services;
using MiddleApi.Services.Configurations;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<ExceptionFilter>();
    });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Db Context
    builder.Services.AddDbContext<ApplicationDbContext>();

    // My Configurations
    builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

    // My Services
    builder.Services.AddScoped<ITokenGenerator, JwtTokenGenerator>();
}
// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseExceptionHandler("/error");
app.UseAuthorization();
app.MapControllers();
app.Run();