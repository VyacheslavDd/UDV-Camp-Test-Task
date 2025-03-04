using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
	public static class DictionaryExtensions
	{
		public static void AddWithConditions<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,  TKey key, Action<TKey> addWhenNotExists,
			Action<TKey> addWhenExists) where TKey: notnull
		{
			if (!dictionary.ContainsKey(key))
				addWhenNotExists(key);
			else
				addWhenExists(key);
		}
	}
}
