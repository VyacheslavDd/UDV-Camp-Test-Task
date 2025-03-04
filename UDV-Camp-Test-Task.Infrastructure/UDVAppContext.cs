using Core.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UDV_Camp_Test_Task.Domain.Models;

namespace UDV_Camp_Test_Task.Infrastructure
{
	public class UDVAppContext(DbContextOptions<UDVAppContext> contextOptions, IOptions<DbOptions> dbOptions, bool isTesting=false): DbContext(contextOptions)
	{
		public virtual DbSet<LettersCountResult> LettersCountResults { get; set; }

		private readonly DbOptions _dbOptions = dbOptions.Value;
		private readonly bool _isTesting = isTesting;

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!_isTesting)
				optionsBuilder.UseNpgsql(_dbOptions.ConnectionString, options => options.MigrationsAssembly(Assembly.GetExecutingAssembly()));
			base.OnConfiguring(optionsBuilder);
		}
	}
}
