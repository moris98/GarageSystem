using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageLogic
{
    internal class ElectricEnergy : Energy
    {
        private static readonly Dictionary<string, Action<Vehicle, string>> sr_InstantiationElectricEnergyQueriesAndValidations = new Dictionary<string, Action<Vehicle, string>>
            {
                { "Please enter the remaining minutes in battery", new Action<Vehicle, string>(ValidateAmountOfMinutes) }
            };

        private readonly float r_MaxBatteryInHours;
        private float m_RemainingBatteryInHours;

        internal ElectricEnergy(float i_MaxBatteryInHours)
        {
            this.r_MaxBatteryInHours = i_MaxBatteryInHours;
            this.m_RemainingBatteryInHours = 0;
            this.EnergyPercentage = this.m_RemainingBatteryInHours / this.r_MaxBatteryInHours;
        }

        internal static Dictionary<string, Action<Vehicle, string>> InstantiationElectricEnergyQueriesAndValidations
        {
            get { return ElectricEnergy.sr_InstantiationElectricEnergyQueriesAndValidations; }
        }

        internal static void ValidateAmountOfMinutes(Vehicle i_CurrentVehicle, string i_Input)
        {
            if (i_CurrentVehicle == null)
            {
                throw new Exception("Vehicle is null");
            }

            if (!(i_CurrentVehicle.Energy is ElectricEnergy))
            {
                throw new Exception("Vehicle does not have electric energy");
            }

            if (!float.TryParse(i_Input, out float fInput))
            {
                throw new FormatException("Amount of minutes is not a valid number");
            }

            if (fInput > ((ElectricEnergy)i_CurrentVehicle.Energy).r_MaxBatteryInHours * 60 || fInput < 0)
            {
                throw new ValueOutOfRangeException(0, ((ElectricEnergy)i_CurrentVehicle.Energy).r_MaxBatteryInHours * 60);
            }

            i_CurrentVehicle.Energy.AddEnergy(fInput);
        }

        public override string ToString()
        {
            return string.Format("Remaining battery in hours: {0}, Max battery in hours: {1}\n", this.m_RemainingBatteryInHours, this.r_MaxBatteryInHours);
        }

        internal override void AddEnergy(float i_MinutesToBeAdded)
        {
            float hoursToBeAdded = i_MinutesToBeAdded / 60;
            if(hoursToBeAdded + this.m_RemainingBatteryInHours <= this.r_MaxBatteryInHours)
            {
                this.m_RemainingBatteryInHours += hoursToBeAdded;
                this.EnergyPercentage = (this.m_RemainingBatteryInHours / this.r_MaxBatteryInHours) * 100;
            }
            else
            {
                throw new ValueOutOfRangeException(0, (this.r_MaxBatteryInHours - m_RemainingBatteryInHours) * 60);
            }
        }
    }
}
