using Files.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Files.Services;
using Files.Filters;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["documentsconnection:blob"], preferMsi: true);
    clientBuilder.AddQueueServiceClient(builder.Configuration["documentsconnection:queue"], preferMsi: true);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("corsapp",
                      policy =>
                      {
                          policy.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
                      });
});

builder.Services.AddDbContext<DocumentsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.Configure<DocumentsDatabaseSettings>(
    builder.Configuration.GetSection("RoverClubDatabase"));

builder.Services.AddSingleton<DocumentsService>();

var app = builder.Build();


app.UseCors("corsapp");
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
