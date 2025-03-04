using Core;
using Core.Base;
using Core.Base.Interfaces;
using Core.DTO;
using Core.Filters;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDV_Camp_Test_Task.Domain.Models;
using System.Net.Http.Json;
using Core.Parsed.VkPosts;
using System.Text.Json;
using Newtonsoft.Json;
using Core.Extensions;
using System.Collections.Specialized;
using Core.Http.Services;

namespace UDV_Camp_Test_Task.Services.StatisticsService
{
	public class PostsStatisticsService(IRepository<LettersCountResult> repository, ILogger logger, IHttpClientService httpClientService, IConfiguration configuration)
		: BaseService<LettersCountResult>(repository, logger), IPostsStatisticsService
	{
		private readonly IHttpClientService _httpClientService = httpClientService;
		private readonly IConfiguration _configuration = configuration;

		public async Task<Result<LettersCountResultOutDTO>> CalculateIdenticalLettersResultAsync(PostsFilter postsFilter)
		{
			_logger.Information("{@service}: Получение данных о постах пользователя...", Type);
			var postsResponse = await GetPostsData();
			if (postsResponse == null || postsResponse.Response == null) return Result.Failure<LettersCountResultOutDTO>("Не удалось получить корректный ответ от API");
			_logger.Information("{@service}: Запуск подсчета...", Type);
			var postTexts = postsResponse.Response.Items
				.OrderByDescending(p => p.Date)
				.Select(p => p.Text.ToLowerInvariant())
				.Take(postsFilter.Count)
				.ToList();
			var lettersCountDictionary = new SortedDictionary<char, int>();
			foreach (var text in postTexts)
			{
				foreach (var chr in text)
				{
					if (char.IsLetter(chr))
						lettersCountDictionary.AddWithConditions(chr, (c) => lettersCountDictionary[c] = 1, (c) => lettersCountDictionary[c]++);
				}
			};
			var lettersCountResult = new LettersCountResult() { CalculatedAt = DateTime.UtcNow, Result = string.Join(" ", lettersCountDictionary) };
			var createdResult = await AddAsync(lettersCountResult);
			if (createdResult.IsSuccess)
			{
				_logger.Information("{@service}: Подсчет завершен и данные успешно записаны", Type);
				return Result.Success(new LettersCountResultOutDTO(createdResult.Value, lettersCountResult.Result, lettersCountResult.CalculatedAt.ToLongDateString()));
			}
			_logger.Information("{@service}: Ошибка при сохранении данных: {@error}", Type, createdResult.Error);
			return Result.Failure<LettersCountResultOutDTO>(createdResult.Error);
		}

		private async Task<VkPostsResponse?> GetPostsData()
		{
			var accessToken = Environment.GetEnvironmentVariable(Const.AccessToken) ?? "";
			var userDomain = _configuration[Const.VkUserDomain];
			var query = $"?access_token={accessToken}&domain={userDomain}&v=5.199";
			return await _httpClientService.GetAsync<VkPostsResponse>(_configuration[Const.VkHttpClientName], _configuration[Const.WallMethodName] + query);
		}
	}
}
