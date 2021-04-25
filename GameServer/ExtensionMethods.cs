using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public static class MyExtensions
    {
        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            var random = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }

        public static T GetRandomElement<T>(this IList<T> list, Random random, Func<T, bool> predicate)
        {
            var filteredList = list.Where(x => predicate(x)).ToList();

            if (filteredList.Count == 0) return default(T);

            int index = random.Next(filteredList.Count);
            return filteredList[index];
        }
    }
}
