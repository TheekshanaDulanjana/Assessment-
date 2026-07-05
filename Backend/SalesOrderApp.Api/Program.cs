using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalesOrderApp.Api.Middleware;
using SalesOrderApp.Application.Interfaces;
using SalesOrderApp.Application.Mappings;
using SalesOrderApp.Application.Services;
using SalesOrderApp.Infrastructure.Data;
using SalesOrderApp.Infrastructure.Reporting;
using SalesOrderApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ---- Database ----
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ---- AutoMapper ----
builder.Services.AddAutoMapper(typeof(MappingProfile));

// ---- Repositories & Unit of Work (Infrastructure -> Application interfaces) ----
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<ISalesOrderRepository, SalesOrderRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// ---- Application services ----
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<ISalesOrderService, SalesOrderService>();
builder.Services.AddScoped<IReportService, SalesOrderPdfReportService>();

// ---- Controllers ----
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler =
        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

// ---- CORS: allow the React dev server (Vite default ports) ----
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowReactApp");
app.UseAuthorization();
app.MapControllers();

// Apply pending migrations and seed reference data (Clients/Items) on startup.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbInitializer.SeedAsync(db);
}

app.Run();
