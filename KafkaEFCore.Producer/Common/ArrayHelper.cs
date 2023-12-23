using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaEFCore.Producer.Common
{
    public static class ArrayHelper
    {
        public static IEnumerable<IEnumerable<TSource>> Batches<TSource>(this IEnumerable<TSource> sources, int amount)
        {
            int index = 0;
            var data = new List<TSource>();
            foreach (TSource source in sources)
            {
                if(index != 0 && (index % amount == 0))
                {
                    yield return data;
                    data = new List<TSource>();
                }
                data.Add(source);
                ++index;
            }
            if (data.Any())
            {
                yield return data;
            }
        }
    }
}
