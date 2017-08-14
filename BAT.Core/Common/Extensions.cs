using System;
using System.Collections.Generic;
using System.Linq;

namespace BAT.Core.Common
{
    public static class Extensions
	{
        public static TKey Mode<T,TKey>(this IEnumerable<T> sequence, Func<T,TKey> groupFunc)
        {
            return sequence.GroupBy(groupFunc).OrderBy(x => x.Count()).First().Key;
        }

		public static IEnumerable<IEnumerable<T>> FindConsecutiveMatch<T>(this IEnumerable<T> sequence, Predicate<T> predicate, int sequenceSize)
		{
			IEnumerable<T> window = Enumerable.Empty<T>();

			int count = 0;

			foreach (var item in sequence)
			{
				if (predicate(item))
				{
					window = window.Concat(Enumerable.Repeat(item, 1));
					count++;

					if (count == sequenceSize)
					{
						yield return window;
						window = window.Skip(1);
						count--;
					}
				}
				else
				{
					count = 0;
					window = Enumerable.Empty<T>();
				}
			}
		}
    }
}