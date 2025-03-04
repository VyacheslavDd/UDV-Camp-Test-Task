using Core.Parsed.VkPosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDV_Camp_Test_Task.Tests.Common
{
	public static class DataProvider
	{
		public static VkPostsResponse VkPostsResponse => new VkPostsResponse()
		{
			Response = new PostsData()
			{
				Items =
				[
					new Post() { Date = 1, Text = "Hello" },
					new Post() { Date = 2, Text = "?$" },
					new Post() { Date = 3, Text = "idk" },
					new Post() { Date = 4, Text = "another" },
					new Post() { Date = 5, Text = "h" },
					new Post() { Date = 6, Text = "34" },
					new Post() { Date = 7, Text = "fourh" },
					new Post() { Date = 8, Text = "Dif" }
				]
			}
		};
	}
}
