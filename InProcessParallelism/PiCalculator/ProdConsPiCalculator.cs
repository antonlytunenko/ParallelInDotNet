using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace PiCalculator
{
    class ProdConsPiCalculator : IPiCalculator
    {
        private int _pointsNumber;

        public ProdConsPiCalculator(int pointsNumber)
        {
            _pointsNumber = pointsNumber;
        }

        public double Calculate()
        {
            var queue = new BlockingCollection<Tuple<double, double>>(new ConcurrentQueue<Tuple<double, double>>());
            var producer = Task.Factory.StartNew(() =>
            {
                PiProducer(queue, _pointsNumber);
            });

            var consumer = Task.Factory.StartNew<int>(() =>
            {                
                return PiConsumer(queue);
            });

            Task.WaitAll(producer, consumer);

            return 4.0 * consumer.Result / _pointsNumber;
        }

        private static void PiProducer(BlockingCollection<Tuple<double, double>> queue, int numberOfPoints)
        {
            var rnd = new Random();
            for (var i = 0; i < numberOfPoints; i++)
            {
                queue.Add(new Tuple<double, double>(rnd.NextDouble(), rnd.NextDouble()));
            }

            queue.CompleteAdding();
        }

        private static int PiConsumer(BlockingCollection<Tuple<double, double>> queue)
        {
            var pointsInside = 0;
            while (!queue.IsCompleted)
            {
                var point = queue.Take();
                if (PiUtil.TestPoint(point.Item1, point.Item2))
                {
                    pointsInside++;
                }
            };

            return pointsInside;
        }
    }
}
