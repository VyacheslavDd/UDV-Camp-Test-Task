using Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDV_Camp_Test_Task.Domain.Models
{
	[Table("ParseResults")]
	public class LettersCountResult: Entity<Guid>
	{
		public string Result { get; set; }
	}
}
