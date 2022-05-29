using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageLogic
{
    internal abstract class Motorcycle : Vehicle
    {
        private const int k_NumOfWheels = 2;
        private const int k_MaxWheelPressure = 31;
        
        private static readonly Dictionary<string, Action<Vehicle, string>> sr_InstantiationMotorcycleQueriesAndValidations = new Dictionary<string, Action<Vehicle, string>>
            {
                { "Please enter the motorcycle licence type ('A', 'A1', 'B1', 'BB'):", new Action<Vehicle, string>(ValidateLicenceType) },
                { "Please enter the motorcycle engine capacity", new Action<Vehicle, string>(ValidateEngineCapacity) }
            };

        private eLicenceType m_LicenceType;
        private int m_EngineCapacity;

        internal Motorcycle()
        {
            for (int i = 0; i < k_NumOfWheels; i++)
            {
                this.AddWheel(new Wheel(k_MaxWheelPressure));
            }
        }

        internal static Dictionary<string, Action<Vehicle, string>> InstantiationMotorcycleQueriesAndValidations
        {
            get { return Motorcycle.sr_InstantiationMotorcycleQueriesAndValidations; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            sb.Append(string.Format("Licence type: {0}, Engine capacity: {1}\n", this.m_LicenceType.ToString(), this.m_EngineCapacity.ToString()));
            return sb.ToString();
        }

        internal static void ValidateLicenceType(Vehicle i_CurrentVehicle, string i_Input)
        {
            Motorcycle motorcycleObject = i_CurrentVehicle as Motorcycle;
            if (motorcycleObject == null)
            {
                // Vehicle is not of type motorcycle
                throw new Exception("Vehicle must be a motorcycle");
            }

            bool isValidLicenceType = Enum.IsDefined(typeof(eLicenceType), i_Input);
            if (!isValidLicenceType)
            {
                throw new ArgumentException("Requested licence type isn't valid");
            }

            motorcycleObject.m_LicenceType = (eLicenceType)Enum.Parse(typeof(eLicenceType), i_Input);
        }

        internal static void ValidateEngineCapacity(Vehicle i_CurrentVehicle, string i_Input)
        {
            if(!int.TryParse(i_Input, out int engineCapacityInt))
            {
                throw new FormatException("Engine capacity must be an integer");
            }

            Motorcycle motorcycleObject = i_CurrentVehicle as Motorcycle;
            if (motorcycleObject == null)
            {
                // Vehicle is not of type motorcycle 
                throw new Exception("Vehicle must be a motorcycle");
            }

            motorcycleObject.m_EngineCapacity = engineCapacityInt;
        }

        internal enum eLicenceType
        {
           A,
           A1,
           B1,
           BB
        }
    }
}
