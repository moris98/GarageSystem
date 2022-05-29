using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageLogic
{
    internal class Wheel
    {
        private static readonly Dictionary<string, Action<Vehicle, string>> sr_InstantiationWheelQueriesAndValidations = new Dictionary<string, Action<Vehicle, string>>
            {
                { "Please enter the tires manufacturer", new Action<Vehicle, string>(ValidateManufacturerName) },
                { "Please enter the tires pressure", new Action<Vehicle, string>(ValidateTiresPressure) }
            };

        private readonly float r_MaxTirePressure;
        private string m_ManufacturerName;
        private float m_TirePressure = 0;

        internal Wheel(float i_MaxTirePressure)
        {
            this.r_MaxTirePressure = i_MaxTirePressure;
        }

        internal static Dictionary<string, Action<Vehicle, string>> InstantiationWheelQueriesAndValidations
        {
            get { return Wheel.sr_InstantiationWheelQueriesAndValidations; }
        }

        internal static void ValidateManufacturerName(Vehicle i_CurrentVehicle, string i_Input)
        {
            if (i_CurrentVehicle == null)
            {
                throw new Exception("Vehicle is null");
            }

            foreach (Wheel currentWheel in i_CurrentVehicle.Wheels)
            {
                currentWheel.ManufacturerName = i_Input;
            }
        }

        internal static void ValidateTiresPressure(Vehicle i_CurrentVehicle, string i_Input)
        {
            if (i_CurrentVehicle == null)
            {
                throw new Exception("Vehicle is null");
            }

            if (!float.TryParse(i_Input, out float fInput))
            {
                throw new FormatException("Tire pressure is not a valid number");
            }

            if (fInput > i_CurrentVehicle.Wheels[0].r_MaxTirePressure)
            {
                throw new ValueOutOfRangeException(0, i_CurrentVehicle.Wheels[0].r_MaxTirePressure);
            }

            foreach (Wheel currentWheel in i_CurrentVehicle.Wheels)
            {
                currentWheel.m_TirePressure = fInput;
            }
        }

        public override string ToString()
        {
            return string.Format("Manufacturer name: {0}, Tire pressure: {1}, Max tire pressure: {2}\n", this.m_ManufacturerName, this.m_TirePressure, this.r_MaxTirePressure);
        }

        internal string ManufacturerName
        {
            get { return this.m_ManufacturerName; }
            set { this.m_ManufacturerName = value; }
        }

        internal void AddAir(float i_AmountOfAirToAdd)
        {
            // Check if not surpassing max
            if(this.m_TirePressure + i_AmountOfAirToAdd <= this.r_MaxTirePressure)
            {
                this.m_TirePressure += i_AmountOfAirToAdd;
            }
            else
            {
                throw new ValueOutOfRangeException(0, this.r_MaxTirePressure - m_TirePressure);
            }
        }

        internal void InflateToMaximum()
        {
            this.AddAir(this.r_MaxTirePressure - this.m_TirePressure);
        }
    }
}
