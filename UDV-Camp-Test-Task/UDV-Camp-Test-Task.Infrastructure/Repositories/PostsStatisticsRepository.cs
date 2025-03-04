using Core.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDV_Camp_Test_Task.Domain.Models;

namespace UDV_Camp_Test_Task.Infrastructure.Repositories
{
	public class PostsStatisticsRepository(UDVAppContext dbContext) : BaseRepository<LettersCountResult>(dbContext, dbContext.LettersCountResults)
	{
	}
}
