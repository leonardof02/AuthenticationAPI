using MiddleApi.Models;
using MiddleApi.Services;
using MiddleApi.Services.Configurations;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

{
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
