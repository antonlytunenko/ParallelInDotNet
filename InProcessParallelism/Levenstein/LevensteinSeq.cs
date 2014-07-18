using System;

namespace Levenstein
{
    class LevensteinSeq : ILevenstein
    {
        public int Calculate(string str1, string str2)
        {
            var prevRow = new int[str2.Length + 1];

            for (var i = 0; i < str2.Length + 1; i++)
            {
                prevRow[i] = i;
            }

            var curRow = new int[str2.Length + 1];

            for (var i = 1; i < str1.Length + 1; i++)
            {
                curRow[0] = i;
                for (var j = 1; j < str2.Length + 1; j++)
                {
                    curRow[j] = Math.Min(
                                        Math.Min(curRow[j - 1] + 1, prevRow[j] + 1),
                                        prevRow[j - 1] + (str1[i - 1] == str2[j - 1] ? 0 : 1));
                }

                curRow.CopyTo(prevRow, 0);
            }

            return curRow[str2.Length];
        }
    }
}
