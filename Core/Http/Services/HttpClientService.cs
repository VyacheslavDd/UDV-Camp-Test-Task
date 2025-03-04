using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Core.Http.Services
{
	public class HttpClientService(IHttpClientFactory httpClientFactory) : IHttpClientService
	{
		private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

		public async Task<T?> GetAsync<T>(string httpClientName, string uri)
		{
			using var client = _httpClientFactory.CreateClient(httpClientName);
			var result = await client.GetAsync(uri);
			result.EnsureSuccessStatusCode();
			return await result.Content.ReadFromJsonAsync<T>();
		}
	}
}
