using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageLogic
{
    public class ValueOutOfRangeException : Exception
    {
        private readonly float r_MinValue;
        private readonly float r_MaxValue;

        public ValueOutOfRangeException(float i_MinValue, float i_MaxValue) : base(string.Format("Minimal value is {0}, and maximal value is {1}", i_MinValue, i_MaxValue))
        {
            this.r_MinValue = i_MinValue;
            this.r_MaxValue = i_MaxValue;
        }
    }
}
