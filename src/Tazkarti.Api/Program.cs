using Microsoft.OpenApi.Models;
using Tazkarti.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Infrastructure Services (DbContext, Identity, Repositories, Unit of Work)
builder.Services.AddInfrastructure(builder.Configuration);

// 2. Add Controller services
builder.Services.AddControllers();

// 3. Add API Explorer & Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Tazkarti API",
        Version = "v1",
        Description = "Backend Web API for Tazkarti Ticketing Platform"
    });
});

// 4. Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

    var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tazkarti API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => Results.Ok(new 
{ 
    service = "Tazkarti Web API", 
    status = "Running",
    timestamp = DateTime.UtcNow 
}));

app.Run();
