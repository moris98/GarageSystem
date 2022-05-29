using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageLogic
{
    internal class FuelEnergy : Energy
    {
        private static readonly Dictionary<string, Action<Vehicle, string>> sr_InstantiationFuelEnergyQueriesAndValidations = new Dictionary<string, Action<Vehicle, string>>
            {
                { "Please enter the current amount of fuel in vehicle", new Action<Vehicle, string>(ValidateFuelAmountInLiters) }
            };

        private readonly float r_MaxFuelAmountInLiters;
        private eFuelType m_FuelType;
        private float m_FuelAmountInLiters;

        internal FuelEnergy(float i_MaxFuelAmount, eFuelType i_FuelType)
        {
            this.r_MaxFuelAmountInLiters = i_MaxFuelAmount;
            this.m_FuelAmountInLiters = 0;
            this.EnergyPercentage = this.m_FuelAmountInLiters / this.r_MaxFuelAmountInLiters;
            this.m_FuelType = i_FuelType;
        }

        internal static Dictionary<string, Action<Vehicle, string>> InstantiationFuelEnergyQueriesAndValidations
        {
            get { return FuelEnergy.sr_InstantiationFuelEnergyQueriesAndValidations; }
        }

        internal eFuelType FuelType
        {
            get { return this.m_FuelType; }
            set { this.m_FuelType = value; }
        }

        internal float FuelAmountInLiters
        {
            get { return this.m_FuelAmountInLiters; }
            set { this.m_FuelAmountInLiters = value; }
        }

        internal static void ValidateFuelAmountInLiters(Vehicle i_CurrentVehicle, string i_Input)
        {
            if (i_CurrentVehicle == null)
            {
                throw new Exception("Vehicle is null");
            }

            if (!(i_CurrentVehicle.Energy is FuelEnergy))
            {
                throw new Exception("Vehicle does not have fuel energy");
            }

            if (!float.TryParse(i_Input, out float fInput))
            {
                throw new FormatException("Fuel amount is not a valid number");
            }

            if (fInput > ((FuelEnergy)i_CurrentVehicle.Energy).r_MaxFuelAmountInLiters || fInput < 0)
            {
                throw new ValueOutOfRangeException(0, ((FuelEnergy)i_CurrentVehicle.Energy).r_MaxFuelAmountInLiters);
            }

            i_CurrentVehicle.Energy.AddEnergy(fInput);
        }

        public override string ToString()
        {
            return string.Format("Fuel amount in liters: {0}, Max fuel amount in liters: {1}, Fuel type: {2}\n", this.m_FuelAmountInLiters, this.r_MaxFuelAmountInLiters, this.m_FuelType.ToString());
        }

        internal override void AddEnergy(float i_AmountInLitersToBeAdded)
        {
            if (i_AmountInLitersToBeAdded + this.m_FuelAmountInLiters <= this.r_MaxFuelAmountInLiters)
            {
                this.m_FuelAmountInLiters += i_AmountInLitersToBeAdded;
                this.EnergyPercentage = (this.m_FuelAmountInLiters / this.r_MaxFuelAmountInLiters) * 100;
            }
            else
            {
                throw new ValueOutOfRangeException(0, this.r_MaxFuelAmountInLiters - m_FuelAmountInLiters);
            }
        }

        public static List<string> GetAllPossibleFuelTypes()
        {
            return Enum.GetNames(typeof(eFuelType)).ToList();
        }

        internal enum eFuelType
        {
            Octan98 = 1,
            Octan96,
            Octan95,
            Soler
        }
    }
}
