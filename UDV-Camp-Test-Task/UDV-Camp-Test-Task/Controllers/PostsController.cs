using Core.DTO;
using Core.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UDV_Camp_Test_Task.Services.StatisticsService;

namespace UDV_Camp_Test_Task.Controllers
{
	[Route("api/posts")]
	[ApiController]
	public class PostsController(IPostsStatisticsService postStatisticsService) : ControllerBase
	{
		private readonly IPostsStatisticsService _postStatisticsService = postStatisticsService;

		/// <summary>
		/// Получение всех подсчетов одинаковых букв в постах
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns>Все подсчеты одинаковых букв в постах</returns>
		[HttpGet]
		[Route("letters-stats")]
		public async Task<IActionResult> GetIdenticalLettersCalculationResultsAsync(CancellationToken cancellationToken)
		{
			var statistics = await _postStatisticsService.GetAllAsync(cancellationToken);
			var mappedData = statistics.Select(s => new LettersCountResultOutDTO(s.Id, s.Result, s.CalculatedAt.ToLongDateString())).ToList();
			if (mappedData.Count > 0) return Ok(mappedData);
			return NoContent();
		}

		/// <summary>
		/// Посчитать вхождение одинаковых букв в постах (по умолчанию в 5 последних)
		/// </summary>
		/// <param name="postsFilter"></param>
		/// <returns>Подсчет вхождения одинаковых букв в N последних постах (по умолчанию N = 5). Результат отсортирован по алфавиту</returns>
		[HttpGet]
		[Route("count-letters")]
		public async Task<IActionResult> CalculateIdenticalLettersStatisticsAsync([FromQuery] PostsFilter postsFilter)
		{
			var result = await _postStatisticsService.CalculateIdenticalLettersResultAsync(postsFilter);
			if (result.IsSuccess) return Ok(result.Value);
			return BadRequest(result.Error);
		}
	}
}
