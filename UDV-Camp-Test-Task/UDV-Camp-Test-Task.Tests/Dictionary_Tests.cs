using Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDV_Camp_Test_Task.Tests
{
	[TestFixture]
	public class Dictionary_Tests
	{

		private readonly Dictionary<char, int> _dictionary = new();
		private void AddWhenNotExists(char key)
		{
			_dictionary[key] = 1;
		}

		private void AddWhenExists(char key)
		{
			_dictionary[key]++;
		}

		[SetUp]
		public void SetUp()
		{
			_dictionary.Clear();
		}

		[Test]
		public void AddWithConditions_WorksCorrectly_When_ElementWasNotPresent()
		{
			_dictionary.AddWithConditions('L', AddWhenNotExists, AddWhenExists);
			Assert.That(_dictionary['L'], Is.EqualTo(1));
		}

		[Test]
		public void AddWithConditions_WorksCorrectly_When_ElementWasPresent()
		{
			_dictionary['L'] = 1;
			_dictionary.AddWithConditions('L', AddWhenNotExists, AddWhenExists);
			Assert.That(_dictionary['L'], Is.EqualTo(2));
		}
	}
}
