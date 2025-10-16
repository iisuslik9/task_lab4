using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks
{
    public static class RunTimeMeasure
    {
        public static async Task<TimeSpan> MeasureAsync(Func<Task> asyncAction, int iterations = 1)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                await asyncAction();
            }
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        public static async Task<(TimeSpan, T)> MeasureAsync<T>(Func<Task<T>> asyncFunc, int iterations = 1)
        {
            T result = default;
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                result = await asyncFunc();
            }
            stopwatch.Stop();
            return (stopwatch.Elapsed, result);
        }
    }

}
