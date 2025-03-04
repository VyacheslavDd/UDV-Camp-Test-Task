using Core.Http.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Http
{
	public static class HttpClientStartup
	{
		public static IServiceCollection AddHttpClientsWithServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddHttpClient(configuration[Const.VkHttpClientName], config =>
			{
				config.BaseAddress = new Uri(configuration[Const.VkHttpClientBaseAddress]);
			});
			services.AddScoped<IHttpClientService, HttpClientService>();
			return services;
		}
	}
}
