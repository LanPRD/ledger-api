using FinancialLedger.Api.Filters;
using FinancialLedger.Api.Middleware;
using FinancialLedger.Application;
using FinancialLedger.Infrastructure;
using FinancialLedger.Infrastructure.Extensions;
using FinancialLedger.Infrastructure.Migrations;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddMvc(options => options.Filters.Add<ExceptionFilter>());

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddSwaggerGen(options => {
  options.SwaggerDoc("v1", new OpenApiInfo { Title = "Financial Ledger", Version = "v1" });
});

builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
  app.MapOpenApi();
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseMiddleware<CultureMiddleware>();

if (!builder.Configuration.IsTestEnvironment()) {
  await MigrateDatabase();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

async Task MigrateDatabase() {
  await using var scope = app.Services.CreateAsyncScope();
  await DatabaseMigration.ApplyMigrations(scope.ServiceProvider);
}

public partial class Program { }