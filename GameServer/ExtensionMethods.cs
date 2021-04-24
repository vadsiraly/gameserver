using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public static class MyExtensions
    {
        public static IList<T> Shuffle<T>(this IList<T> array)
        {
            var random = new Random();
            int n = array.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = array[k];
                array[k] = array[n];
                array[n] = value;
            }

            return array;
        }
    }
}
