using Core;
using Core.Base.Interfaces;
using Core.Http;
using Core.Options;
using Core.Serilog;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using UDV_Camp_Test_Task.Domain.Models;
using UDV_Camp_Test_Task.Infrastructure;
using UDV_Camp_Test_Task.Infrastructure.Repositories;
using UDV_Camp_Test_Task.Services.StatisticsService;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

builder.Services.Configure<DbOptions>(builder.Configuration.GetSection(Const.DbSectionConfigurationName));
builder.Host.AddSerilog();
builder.Services.AddDbContext<UDVAppContext>();

builder.Services.AddHttpClientsWithServices(builder.Configuration);
builder.Services.AddScoped<IRepository<LettersCountResult>, PostsStatisticsRepository>();
builder.Services.AddScoped<IPostsStatisticsService, PostsStatisticsService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
	var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
	setup.IncludeXmlComments(xmlPath);
});

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
