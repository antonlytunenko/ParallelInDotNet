using System;
using System.Threading;
using System.Threading.Tasks;

namespace PiCalculator
{
    class DataParallelPiCalculator : IPiCalculator
    {
        private readonly int _pointsNumber;

        public DataParallelPiCalculator(int pointsNumber)
        {
            _pointsNumber = pointsNumber;
        }

        public double Calculate()
        {
            var pointsInside = 0;
            var rnd = new ThreadLocal<Random>(() => new Random(Thread.CurrentThread.ManagedThreadId));
            Parallel.For(0, _pointsNumber,
                (i) =>
                {
                    if (PiUtil.TestPoint(rnd.Value))
                    {
                        Interlocked.Increment(ref pointsInside);
                    }
                });

            return 4.0 * pointsInside / _pointsNumber;
        }
    }
}
