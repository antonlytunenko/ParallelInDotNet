using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Levenstein
{
    class LevensteinBatchParallel : ILevenstein
    {
        private readonly int _batchSize;

        public LevensteinBatchParallel(int batchSize)
        {
            _batchSize = batchSize;
        }

        public int Calculate(string str1, string str2)
        {
            var horLength = str1.Length + 1;
            var vertLength = str2.Length + 1;

            var horNumber = ((horLength - 1) / _batchSize) + 1;
            var vertNumber = ((vertLength - 1) / _batchSize) + 1;

            var rows = new int[vertNumber+1, horLength];
            var cols = new int[horNumber+1, vertLength];

            for (var i = 0; i < horLength; i++)
            {
                rows[0, i] = i;
            }

            for (var i = 0; i < vertNumber + 1; i++)
            {
                rows[i, 0] = Math.Min(i*_batchSize, vertLength);
            }


            for (var i = 0; i < vertLength; i++)
            {
                cols[0, i] = i;
            }

            for (var i = 0; i < horNumber + 1; i++)
            {
                cols[i, 0] = Math.Min(i * _batchSize, horLength);
            }

            var batches = new List<LevensteinBatchData>();

            for(var i=0; i< horNumber; i++)
            {
                for(var j=0; j < vertNumber; j++)
                {
                    batches.Add(new LevensteinBatchData
                    {
                        HorMin = Math.Min(i*_batchSize, horLength)+1,
                        HorMax = Math.Min(i*_batchSize + _batchSize, horLength - 1),
                        VerMin = Math.Min(j*_batchSize, vertLength)+1,
                        VerMax = Math.Min(j*_batchSize + _batchSize, vertLength - 1),
                        HorNum = i,
                        VerNum=j
                    });
                }
            }

            var task = Task.Factory.StartNew(() => Start(str1, str2, rows, cols, batches, 0));
            task.Wait();
            return cols[horNumber, vertLength - 1];
        }

        private void Start(string str1, string str2, int[,] rows, int[,] cols, List<LevensteinBatchData> batches, int iteration)
        {
            var toCalculate = batches
                .Where(b => b.VerNum + b.HorNum == iteration)
                .Select(b => Task.Factory.StartNew(
                    b.GetCalculationAction(rows, cols, str1, str2),
                    TaskCreationOptions.AttachedToParent)).ToArray();
            if (toCalculate.Length == 0)
            {
                return;
            }

            Task.WaitAll(toCalculate);
            foreach (var t in toCalculate)
            {
                t.Result.Merge(rows, cols);
            }

            Start(str1, str2, rows, cols, batches, iteration + 1);
        }
    }
}
