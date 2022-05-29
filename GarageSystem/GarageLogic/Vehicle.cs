using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageLogic
{
    internal abstract class Vehicle
    {
        private static readonly Dictionary<string, Action<Vehicle, string>> sr_InstantiationVehicleQueriesAndValidations = new Dictionary<string, Action<Vehicle, string>>
            {
                { "Please enter the vehicle's model name:", new Action<Vehicle, string>(ValidateModelName) },
                { "Please enter the vehicle's licence plate:", new Action<Vehicle, string>(ValidateLicencePlate) }
            };

        private readonly List<Wheel> r_Wheels = new List<Wheel>();
        private string m_ModelName;
        private string m_LicencePlateNumber;
        private Energy m_Energy;

        internal static Dictionary<string, Action<Vehicle, string>> InstantiationVehicleQueriesAndValidations
        {
            get { return Vehicle.sr_InstantiationVehicleQueriesAndValidations; }
        }

        internal static void ValidateModelName(Vehicle i_VehicleToModify, string input)
        {
            if (i_VehicleToModify == null)
            {
                throw new Exception("Vehicle is null");
            }

            if (input == null)
            {
                throw new FormatException("Model name is not in a valid format");
            }

            i_VehicleToModify.ModelName = input;
        }

        internal static void ValidateLicencePlate(Vehicle i_VehicleToModify, string input)
        {
            if (i_VehicleToModify == null)
            {
                throw new Exception("Vehicle is null");
            }

            if (input == null || input.Length != 5)
            {
                throw new FormatException("Licence plate number is not in a valid format - must be 5 digits number");
            }

            i_VehicleToModify.LicencePlate = input;
        }

        internal abstract Dictionary<string, Action<Vehicle, string>> GetAllQuestionsAndValidations();

        internal string ModelName
        {
            get { return this.m_ModelName; }
            set { this.m_ModelName = value; }
        }

        internal string LicencePlate
        {
            get { return this.m_LicencePlateNumber; }
            set { this.m_LicencePlateNumber = value; }
        }

        internal List<Wheel> Wheels
        {
            get { return this.r_Wheels; }
        }

        internal float EnergyPercentage
        {
            get { return m_Energy.EnergyPercentage; }
        }

        internal Energy Energy
        {
            get { return this.m_Energy; }
            set { this.m_Energy = value; }
        }

        internal void AddWheel(Wheel i_Wheel)
        {
            this.r_Wheels.Add(i_Wheel);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("Licence Plate: {0}, Model Name: {1}\n", this.LicencePlate, this.ModelName));
            if (r_Wheels.Count > 0)
            {
                sb.Append(r_Wheels[0].ToString());
            }

            if (m_Energy != null)
            {
                sb.Append(m_Energy.ToString());
            }

            return sb.ToString();
        }

        internal void InflateAllWheelsToMaximum()
        {
            foreach (Wheel currentWheel in r_Wheels)
            {
                currentWheel.InflateToMaximum();
            }
        }
    }
}
