using Core;
using Core.Options;
using Core.Serilog;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using UDV_Camp_Test_Task.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DbOptions>(builder.Configuration.GetSection(Const.DbSectionConfigurationName));
builder.Host.AddSerilog();
builder.Services.AddDbContext<UDVAppContext>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<UDVAppContext>();
	dbContext.Database.Migrate();
}

app.Run();
