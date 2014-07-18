using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PiCalculator
{
    class DataParallelPlinqPiCalculator : IPiCalculator
    {
        private readonly int _pointsNumber;

        public DataParallelPlinqPiCalculator(int pointsNumber)
        {
            _pointsNumber = pointsNumber;
        }

        public double Calculate()
        {
            var rnd = new ThreadLocal<Random>(() => new Random(Thread.CurrentThread.ManagedThreadId));
            var pointsInside = Enumerable.Range(0, _pointsNumber)
                .AsParallel()
                .Count((i) => PiUtil.TestPoint(rnd.Value));
            
            return 4.0 * pointsInside / _pointsNumber;
        }
    }
}
