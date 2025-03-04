using Core.Base.Interfaces;
using Core.DTO;
using Core.Filters;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDV_Camp_Test_Task.Domain.Models;

namespace UDV_Camp_Test_Task.Services.StatisticsService
{
	public interface IPostsStatisticsService: IService<LettersCountResult>
	{
		Task<Result<LettersCountResultOutDTO>> CalculateIdenticalLettersResultAsync(PostsFilter postsFilter);
	}
}
