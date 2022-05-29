using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageLogic
{
    internal abstract class Energy
    {
        private float m_EnergyPercentage;

        internal abstract void AddEnergy(float i_Amount);

        internal float EnergyPercentage
        {
            get { return this.m_EnergyPercentage; }
            set { this.m_EnergyPercentage = value; }
        }
    }
}
