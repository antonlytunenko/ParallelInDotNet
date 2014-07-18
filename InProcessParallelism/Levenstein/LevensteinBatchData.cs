using System;

namespace Levenstein
{
    class LevensteinBatchData
    {
        public int HorMin { get; set; }
        public int HorMax { get; set; }
        public int VerMin { get; set; }
        public int VerMax { get; set; }
        public int VerNum { get; set; }
        public int HorNum { get; set; }
        public int[] ResultRow { get; set; }
        public int[] ResultCol { get; set; }

        public Func<LevensteinBatchData> GetCalculationAction(int[,] matrix, string str1, string str2)
        {
            return () =>
                {
                    var horSize = HorMax - HorMin + 1;
                    var verSize = VerMax - VerMin + 1;
                    var strToCompare1 = str1.Substring(HorMin-1, horSize-1);
                    var strToCompare2 = str2.Substring(VerMin-1, verSize-1);
                    var result= new int[horSize, verSize];
                    for (var i = 0; i < horSize; i++)
                    {
                        result[i, 0] = matrix[HorMin + i - 1, VerMin - 1];
                    }

                    for (var i = 0; i < verSize; i++)
                    {
                        result[0, i] = matrix[HorMin - 1, VerMin + i - 1];
                    }


                    for(var i=1; i< horSize; i++)
                    {
                        for(var j=1; j< verSize; j++)
                        {
                            result[i, j] = Math.Min(
                                                Math.Min(result[i, j - 1] + 1, result[i - 1, j] + 1),
                                                result[i - 1, j - 1] + (strToCompare1[i - 1] == strToCompare2[j - 1] ? 0 : 1));
                        }
                    }

                    //Result = result;
                    return this;
                };
        }

        public Func<LevensteinBatchData> GetCalculationAction(int[,] rows, int[,] cols, string str1, string str2)
        {
            return () =>
            {
                var horSize = HorMax - HorMin + 1;
                var verSize = VerMax - VerMin + 1;
                var strToCompare1 = str1.Substring(HorMin - 1, horSize);
                var strToCompare2 = str2.Substring(VerMin - 1, verSize);

                var prevRow = new int[horSize+1];

                for (var i = 0; i < horSize+1; i++)
                {
                    prevRow[i] = rows[VerNum, HorMin + i - 1];
                }

                var prevCol = new int[verSize+1];

                for (var i = 0; i < verSize+1; i++)
                {
                    prevCol[i] = cols[HorNum, VerMin + i - 1];
                }

                var curCol = new int[verSize+1];

                for (var i = 1; i < strToCompare1.Length + 1; i++)
                {
                    curCol[0] = prevRow[i];
                    for (var j = 1; j < strToCompare2.Length + 1; j++)
                    {
                        curCol[j] = Math.Min(
                                            Math.Min(curCol[j - 1] + 1, prevCol[j] + 1),
                                            prevCol[j - 1] + (strToCompare1[i - 1] == strToCompare2[j - 1] ? 0 : 1));
                    }

                    curCol.CopyTo(prevCol, 0);
                    prevRow[i] = curCol[strToCompare2.Length];
                }

                ResultRow = prevRow;
                ResultCol = curCol;
                return this;
            };
        }

        public void Merge(int[,] rows, int[,] cols)
        {
            var horSize = HorMax - HorMin + 1;
            var verSize = VerMax - VerMin + 1;

            for (var i = 0; i < horSize; i++)
            {
                rows[VerNum +1, HorMin + i] = ResultRow[i+1];
            }

            for (var j = 0; j < verSize; j++)
            {
                cols[HorNum +1, VerMin + j] = ResultCol[j+1];
            }
        }

        public void Merge(int[,] matrix)
        {
            var horSize = HorMax - HorMin + 1;
            var verSize = VerMax - VerMin + 1;

            for (var i = 0; i < horSize; i++)
            {
                matrix[HorMin + i, verSize - 1] = ResultRow[i];
            }

            for (var j = 0; j < verSize; j++)
            {
                matrix[horSize - 1, VerMin + j] = ResultCol[j];
            }
        }
    }
}
