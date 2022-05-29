using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageLogic
{
    internal abstract class Truck : Vehicle
    {
        private const int k_NumOfWheels = 16;
        private const int k_MaxWheelPressure = 24;
        
        private static readonly Dictionary<string, Action<Vehicle, string>> sr_InstantiationTruckQueriesAndValidations = new Dictionary<string, Action<Vehicle, string>>
            {
                { "Please enter 'y' if the truck is a refrigerated truck else 'n':", new Action<Vehicle, string>(ValidateIsRefrigerated) },
                { "Please enter the carry capacity", new Action<Vehicle, string>(ValidateCarryCapacity) }
            };

        private bool m_IsRefrigeratedTruck;
        private float m_CarryCapacity;

        internal Truck()
        {
            for (int i = 0; i < k_NumOfWheels; i++)
            {
                this.AddWheel(new Wheel(k_MaxWheelPressure));
            }
        }

        internal static Dictionary<string, Action<Vehicle, string>> InstantiationTruckQueriesAndValidations
        {
            get { return Truck.sr_InstantiationTruckQueriesAndValidations; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            sb.Append(string.Format("Is refrigerated: {0}, Carry capacity: {1}\n", this.m_IsRefrigeratedTruck.ToString(), this.m_CarryCapacity.ToString()));
            return sb.ToString();
        }

        internal static void ValidateIsRefrigerated(Vehicle i_CurrentVehicle, string i_Input)
        {
            Truck truckObject = i_CurrentVehicle as Truck;
            if (truckObject == null)
            {
                // Vehicle is not of type Truck 
                throw new Exception("Vehicle must be a truck");
            }

            if(!(i_Input.Equals("y") || i_Input.Equals("n")))
            {
                throw new FormatException("Invalid input ('y'/'n')");
            }

            truckObject.m_IsRefrigeratedTruck = i_Input.Equals("y") ? true : false;
        }

        internal static void ValidateCarryCapacity(Vehicle i_CurrentVehicle, string i_Input)
        {
            Truck truckObject = i_CurrentVehicle as Truck;
            if (truckObject == null)
            {
                // Vehicle is not of type Truck 
                throw new Exception("Vehicle must be a truck");
            }

            if (!int.TryParse(i_Input, out int truckCapacityInt))
            {
                throw new FormatException("Input must be an integer");
            }

            if(truckCapacityInt < 0)
            {
                throw new ArgumentException("Carry capacity must be a non negative integer");
            }

            truckObject.m_CarryCapacity = truckCapacityInt;
        }
    }
}
