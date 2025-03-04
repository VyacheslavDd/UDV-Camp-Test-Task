using Core.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDV_Camp_Test_Task.Infrastructure;

namespace UDV_Camp_Test_Task.Tests.Common
{
	public static class DbContextMocker
	{
		public static UDVAppContext CreateInMemoryAppDbContext()
		{
			var dbContextOptions = new DbContextOptionsBuilder<UDVAppContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
			var iOptionsMock = new Mock<IOptions<DbOptions>>();
			iOptionsMock.Setup(o => o.Value).Returns(new DbOptions() { ConnectionString = "" });
			return new UDVAppContext(dbContextOptions, iOptionsMock.Object, isTesting: true);
		}
	}
}
