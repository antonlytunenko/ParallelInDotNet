using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace PiCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            var pointsNumber = 100000000;
            
            var calculators = new IPiCalculator[]
            {
                new SequencePiCalculator(pointsNumber),
                new ProdConsPiCalculator(pointsNumber),
                new DataParallelPiCalculator(pointsNumber),
                new DataParallelPlinqPiCalculator(pointsNumber),
                new DataParallelBatchPiCalculator(pointsNumber, 8)
            };

            DisplayRes(GetBenchmarks(calculators));

            Console.ReadLine();
        }

        private static IEnumerable<BenchmarkResult<double>> GetBenchmarks(IEnumerable<IPiCalculator> calculators)
        {
            foreach (var calc in calculators)
            {
                yield return Benchmark.Do<double>(calc.Calculate, calc.GetType().Name);
            }
        }

        private static void DisplayRes(IEnumerable<BenchmarkResult<double>> results)
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
