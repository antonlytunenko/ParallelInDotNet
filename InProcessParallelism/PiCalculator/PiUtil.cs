using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiCalculator
{
    class PiUtil
    {
        public static bool TestPoint(Random rnd)
        {
            return TestPoint(rnd.NextDouble(), rnd.NextDouble());
        }

        public static bool TestPoint(double x, double y)
        {
            const double centerX = 0.5;
            const double centerY = 0.5;

            return Math.Sqrt((centerX - x) * (centerX - x) +
                    (centerY - y) * (centerY - y)) <= 0.5;
        }
    }
}
