
using Core;
using Core.Base.Interfaces;
using Core.Http.Services;
using Core.Parsed.VkPosts;
using Microsoft.Extensions.Configuration;
using Moq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Filters;
using UDV_Camp_Test_Task.Domain.Models;
using UDV_Camp_Test_Task.Infrastructure;
using UDV_Camp_Test_Task.Infrastructure.Repositories;
using UDV_Camp_Test_Task.Services.StatisticsService;
using UDV_Camp_Test_Task.Tests.Common;
using Microsoft.Extensions.Hosting;
using CSharpFunctionalExtensions;
using Core.DTO;

namespace UDV_Camp_Test_Task.Tests
{
	[TestFixture]
	public class PostsStatisticsService_Tests
	{
		private UDVAppContext _dbContext;
		private IPostsStatisticsService _postsStatisticsService;
		private readonly Mock<ILogger> _loggerMock;
		private readonly Mock<IHttpClientService> _httpClientServiceMock;
		private readonly Mock<IConfiguration> _configurationMock;

		public PostsStatisticsService_Tests()
		{
			_loggerMock = new Mock<ILogger>();
			_loggerMock.Setup(l => l.Information(It.IsAny<string>())).Callback(() => { });
			_httpClientServiceMock = new Mock<IHttpClientService>();
			_configurationMock = new Mock<IConfiguration>();
			_configurationMock.Setup(c => c[It.IsAny<string>()]).Returns("");
		}

		[SetUp]
		public void SetUp()
		{
			_dbContext = DbContextMocker.CreateInMemoryAppDbContext();
			var resultsRepository = new PostsStatisticsRepository(_dbContext);
			_postsStatisticsService = new PostsStatisticsService(resultsRepository, _loggerMock.Object, _httpClientServiceMock.Object, _configurationMock.Object);
		}

		[TearDown]
		public void TearDown()
		{
			_dbContext.Dispose();
		}


		[Test]
		public async Task Service_ShouldReturn_EmptyResult_When_NoPosts()
		{
			var res = await CalculateIdenticalLettersAsync(p => [], new PostsFilter());
			Assert.That(res.Value.Result, Is.EqualTo(""));
		}

		[Test]
		public async Task Service_ShouldIgnoreAnything_WhichIsNotLetter()
		{
			var res = await CalculateIdenticalLettersAsync(p => p.Where(e => e.Text.All(c => !char.IsLetter(c))).ToList(), new PostsFilter());
			Assert.That(res.Value.Result, Is.EqualTo(""));
		}

		[Test]
		public async Task Service_DoesntCareAboutLetterCase()
		{
			var res = await CalculateIdenticalLettersAsync(p => p.Where(e => e.Date == 1 || e.Date == 5).ToList(), new PostsFilter());
			Assert.That(res.Value.Result, Is.EqualTo("[e, 1] [h, 2] [l, 2] [o, 1]"));
		}

		[Test]
		public async Task Service_ShouldUse_PostsFilter_Correctly()
		{
			var res = await CalculateIdenticalLettersAsync(p => p, new PostsFilter(1));
			Assert.That(res.Value.Result, Is.EqualTo("[d, 1] [f, 1] [i, 1]"));
			var res2 = await CalculateIdenticalLettersAsync(p => p, new PostsFilter(2));
			Assert.That(res2.Value.Result, Is.EqualTo("[d, 1] [f, 2] [h, 1] [i, 1] [o, 1] [r, 1] [u, 1]"));
		}

		[Test]
		public async Task Serivce_ShouldWorkCorrectly_WithDefaultParameters()
		{
			var res = await CalculateIdenticalLettersAsync(p => p, new PostsFilter());
			Assert.That(res.Value.Result, Is.EqualTo("[a, 1] [d, 1] [e, 1] [f, 2] [h, 3] [i, 1] [n, 1] [o, 2] [r, 2] [t, 1] [u, 1]"));
		}

		[Test]
		public async Task Service_ShouldFailure_When_NullResponse()
		{
			SetupGetAsyncOfHttpClientService(null);
			var res = await _postsStatisticsService.CalculateIdenticalLettersResultAsync(new PostsFilter());
			Assert.That(res.IsFailure, Is.True);
		}

		[Test]
		public async Task Service_ShouldFailure_When_NullPostsData()
		{
			var posts = DataProvider.VkPostsResponse;
			posts.Response = null;
			SetupGetAsyncOfHttpClientService(posts);
			var res = await _postsStatisticsService.CalculateIdenticalLettersResultAsync(new PostsFilter());
			Assert.That(res.IsFailure, Is.True);
		}

		private async Task<Result<LettersCountResultOutDTO>> CalculateIdenticalLettersAsync(Func<List<Post>, List<Post>> postsMocker, PostsFilter postsFilter)
		{
			var posts = DataProvider.VkPostsResponse;
			posts.Response.Items = postsMocker(posts.Response.Items);
			SetupGetAsyncOfHttpClientService(posts);
			return await _postsStatisticsService.CalculateIdenticalLettersResultAsync(postsFilter);
		}

		private void SetupGetAsyncOfHttpClientService(VkPostsResponse vkPostsResponse)
		{
			_httpClientServiceMock.Setup(cl => cl.GetAsync<VkPostsResponse>(It.IsAny<string>(), It.IsAny<string>()))
				.Returns(Task.FromResult(vkPostsResponse));
		}
	}
}
