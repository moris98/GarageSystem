using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageLogic
{
    internal class ElectricCar : Car
    {
        internal static Dictionary<string, Action<Vehicle, string>> s_ListOfQuestionsAndValidations;

        internal ElectricCar()
        {
            if (s_ListOfQuestionsAndValidations == null)
            {
                s_ListOfQuestionsAndValidations = this.CreateAllQuestionsAndValidations();
            }

            this.Energy = new ElectricEnergy(3.3f);
        }

        internal override Dictionary<string, Action<Vehicle, string>> GetAllQuestionsAndValidations()
        {
            return ElectricCar.s_ListOfQuestionsAndValidations;
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

            foreach (KeyValuePair<string, Action<Vehicle, string>> pair in Car.InstantiationCarQueriesAndValidations)
            {
                questionsAndValidations[pair.Key] = pair.Value;
            }

            foreach (KeyValuePair<string, Action<Vehicle, string>> pair in ElectricEnergy.InstantiationElectricEnergyQueriesAndValidations)
            {
                questionsAndValidations[pair.Key] = pair.Value;
            }

            return questionsAndValidations;
        }
    }
}
