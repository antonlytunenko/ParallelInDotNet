using System;
using System.Diagnostics;

namespace Utils
{
    public static class Benchmark
    {
        public static BenchmarkResult<T> Do<T>(Func<T> a, string name)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var res = a();
            stopwatch.Stop();
            return new BenchmarkResult<T>
            {               
                Name = name,
                Result = res,
                TotalSeconds = stopwatch.Elapsed.TotalSeconds
            };
        }
    }
}
