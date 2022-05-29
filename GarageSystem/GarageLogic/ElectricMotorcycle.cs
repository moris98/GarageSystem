using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageLogic
{
    internal class ElectricMotorcycle : Motorcycle
    {
        internal static Dictionary<string, Action<Vehicle, string>> s_ListOfQuestionsAndValidations;

        internal ElectricMotorcycle()
        {
            if (s_ListOfQuestionsAndValidations == null)
            {
                s_ListOfQuestionsAndValidations = this.CreateAllQuestionsAndValidations();
            }

            this.Energy = new ElectricEnergy(2.5f);
        }

        private Dictionary<string, Action<Vehicle, string>> CreateAllQuestionsAndValidations()
        {
            Dictionary<string, Action<Vehicle, string>> questionsAndValidations = new Dictionary<string, Action<Vehicle, string>>();
            foreach (KeyValuePair<string, Action<Vehicle, string>> pair in Vehicle.InstantiationVehicleQueriesAndValidations)
            {
                questionsAndValidations[pair.Key] = pair.Value;
            }

            foreach (KeyValuePair<string, Action<Vehicle, string>> pair in Wheel.InstantiationWheelQueriesAndValidations)
            {
                questionsAndValidations[pair.Key] = pair.Value;
            }

            foreach (KeyValuePair<string, Action<Vehicle, string>> pair in Motorcycle.InstantiationMotorcycleQueriesAndValidations)
            {
                questionsAndValidations[pair.Key] = pair.Value;
            }

            foreach (KeyValuePair<string, Action<Vehicle, string>> pair in ElectricEnergy.InstantiationElectricEnergyQueriesAndValidations)
            {
                questionsAndValidations[pair.Key] = pair.Value;
            }

            return questionsAndValidations;
        }

        internal override Dictionary<string, Action<Vehicle, string>> GetAllQuestionsAndValidations()
        {
            return ElectricMotorcycle.s_ListOfQuestionsAndValidations;
        }
    }
}
