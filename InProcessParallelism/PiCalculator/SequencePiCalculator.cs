using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiCalculator
{
    class SequencePiCalculator : IPiCalculator
    {
        private readonly int _pointsNumber;

        public SequencePiCalculator(int pointsNumber)
        {
            _pointsNumber = pointsNumber;
        }

        public double Calculate()
        {
            var pointsInside = 0;
            var rnd = new Random();
            for (int i = 0; i < _pointsNumber; i++)
            {
                if (PiUtil.TestPoint(rnd))
                {
                    pointsInside++;
                }
            }

            return 4.0 * pointsInside / _pointsNumber;
        }
    }
}
