using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GarageLogic
{
    internal abstract class Car : Vehicle
    {
        private const int k_NumOfWheels = 4;
        private const int k_MaxWheelPressure = 29;
        
        private static readonly Dictionary<string, Action<Vehicle, string>> sr_InstantiationCarQueriesAndValidations = new Dictionary<string, Action<Vehicle, string>>
            {
                { "Please enter the car color ('Red', 'White', 'Green', 'Blue'):", new Action<Vehicle, string>(ValidateCarColor) },
                { "Please enter the amount of doors in the car ('2', '3', '4', '5')", new Action<Vehicle, string>(ValidateAmountOfDoors) }
            };

        private eCarColor m_CarColor;
        private eAmountOfDoors m_AmountOfDoors;

        protected Car()
        {
            for(int i = 0; i < k_NumOfWheels; i++)
            {
                this.AddWheel(new Wheel(k_MaxWheelPressure));
            }
        }

        internal static Dictionary<string, Action<Vehicle, string>> InstantiationCarQueriesAndValidations
        {
            get { return Car.sr_InstantiationCarQueriesAndValidations; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            sb.Append(string.Format("Car color: {0}, Amount of doors: {1}\n", this.m_CarColor.ToString(), this.m_AmountOfDoors.ToString()));
            return sb.ToString();
        }

        internal static void ValidateCarColor(Vehicle i_CurrentVehicle, string i_Input)
        {
            Car carObject = i_CurrentVehicle as Car;
            if (carObject == null)
            { // Vehicle is not of type Car 
                throw new Exception("Vehicle must be a car");
            }

            bool isValidColor = Enum.IsDefined(typeof(eCarColor), i_Input);
            if (!isValidColor)
            {
                throw new ArgumentException("Requested color isn't valid");
            }

            carObject.m_CarColor = (eCarColor)Enum.Parse(typeof(eCarColor), i_Input);
        }

        internal static void ValidateAmountOfDoors(Vehicle i_CurrentVehicle, string i_Input)
        {
            Car carObject = i_CurrentVehicle as Car;
            if (carObject == null)
            {
                throw new Exception("Vehicle must be a car");
            }

            if(!int.TryParse(i_Input, out int amountOfDoorsInt))
            {
                throw new FormatException("Requested amount of doors isn't in right format");
            }

            if (!Enum.IsDefined(typeof(eAmountOfDoors), amountOfDoorsInt))
            {
                throw new ArgumentException("Requested amount of doors isn't valid");
            }

            carObject.m_AmountOfDoors = (eAmountOfDoors)amountOfDoorsInt;
        }

        internal enum eCarColor
        {
            Red,
            White,
            Green,
            Blue
        }
        
        internal enum eAmountOfDoors
        {
           Two = 2,
           Three = 3,
           Four = 4,
           Five = 5
        }
    }
}
