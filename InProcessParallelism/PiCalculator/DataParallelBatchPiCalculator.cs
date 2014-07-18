using System;
using System.Threading;
using System.Threading.Tasks;

namespace PiCalculator
{
    class DataParallelBatchPiCalculator : IPiCalculator
    {
        private readonly int _pointsNumber;
        private readonly int _batchNumber;

        public DataParallelBatchPiCalculator(int pointsNumber, int batchNumber)
        {
            _pointsNumber = pointsNumber;
            _batchNumber = batchNumber;
        }

        public double Calculate()
        {
            var pointsInside = 0;
            var rnd = new ThreadLocal<Random>(() => new Random(Thread.CurrentThread.ManagedThreadId));
            var batchSize = _pointsNumber / _batchNumber;
            Parallel.For(0, _batchNumber,                
                (i) =>
                {
                    var pointsInsideLocal = 0;
                    for (var j = 0; j < batchSize; j++)
                    {
                        if (PiUtil.TestPoint(rnd.Value))
                        {
                            pointsInsideLocal++;
                        }
                    }

                    Interlocked.Add(ref pointsInside, pointsInsideLocal);
                });

            return 4.0 * pointsInside / _pointsNumber;
        }
    }
}
