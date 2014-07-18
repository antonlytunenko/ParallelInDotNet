using System;
using System.Threading.Tasks;

namespace Levenstein
{
    class LevensteinMicroParallel : ILevenstein
    {
        public int Calculate(string str1, string str2)
        {
            var prevRow = new int[str1.Length + 1];
            var prevCol = new int[str2.Length + 1];

            Parallel.For(0, str1.Length + 1, (k) => prevRow[k] = k);
            Parallel.For(0, str2.Length + 1, (k) => prevCol[k] = k);

            var curRow = new int[str1.Length + 1];
            var curCol = new int[str2.Length + 1];

            var i = 1;
            var limit = Math.Min(str1.Length, str2.Length);
            while (i < limit+1)
            {
                curRow[i-1] = prevCol[i];
                curCol[i-1] = prevRow[i];

                var itask = Task.Factory.StartNew(() =>
                    {
                        for (var k = i; k < str1.Length + 1; k++)
                        {
                            CalcCell(str1, str2, prevRow, curRow, k, i);
                        }

                        curRow.CopyTo(prevRow, 0);
                    });

                var jtask = Task.Factory.StartNew(() =>
                {
                    for (var k = i; k < str2.Length + 1; k++)
                    {
                        CalcCell(str2, str1, prevCol, curCol, k, i);
                    }

                    curCol.CopyTo(prevCol, 0);
                });

                Task.WaitAll(itask, jtask);

                i++;
            }

            return curRow[str1.Length];
        }

        private void CalcCell(string str1, string str2, int[] prev, int[] cur, int i, int j)
        {
            cur[i] = Math.Min(
                                Math.Min(cur[i - 1] + 1, prev[i] + 1),
                                prev[i - 1] + (str1[i - 1] == str2[j - 1] ? 0 : 1));
        }
    }
}
