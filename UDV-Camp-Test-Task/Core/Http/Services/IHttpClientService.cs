using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Http.Services
{
	public interface IHttpClientService
	{
		Task<T> GetAsync<T>(string httpClientName, string uri);
	}
}
