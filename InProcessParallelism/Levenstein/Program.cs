using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Utils;

namespace Levenstein
{
    class Program
    {
        static void Main(string[] args)
        {
            var calculators = new ILevenstein[]
            {
                new LevensteinSeq(),
                new LevensteinMicroParallel(),
                new LevensteinBatchParallel(1000)
            };

            var str1 = File.ReadAllText(@"Input\Input1.txt");
            var str2 = File.ReadAllText(@"Input\Input2.txt");

            DisplayRes(GetBenchmarks(calculators, str1, str2));

            Console.ReadLine();

        }

        private static IEnumerable<BenchmarkResult<int>> GetBenchmarks(IEnumerable<ILevenstein> calculators, string str1, string str2)
        {
            foreach (var calc in calculators)
            {
                yield return Benchmark.Do<int>(() => calc.Calculate(str1, str2), calc.GetType().Name);
            }
        }

        private static void DisplayRes(IEnumerable<BenchmarkResult<int>> results)
        {
            var seqRes = results.First();
            foreach (var res in results)
            {
                Console.WriteLine(string.Format(
                    "Name: {0}\tRes: {1}\tTime: {2}\tEff: {3}",
                    res.Name,
                    res.Result.ToString("0.#####"),
                    res.TotalSeconds.ToString("0.###"),
                    (seqRes.Name == res.Name ? 1
                        : seqRes.TotalSeconds / (res.TotalSeconds * Environment.ProcessorCount)).ToString("0.###")));
            }
        }
    }
}
