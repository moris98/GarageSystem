using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageLogic
{
    internal class FuelMotorcycle : Motorcycle
    {
        internal static Dictionary<string, Action<Vehicle, string>> s_ListOfQuestionsAndValidations;

        internal FuelMotorcycle()
        {
            if (s_ListOfQuestionsAndValidations == null)
            {
                s_ListOfQuestionsAndValidations = this.CreateAllQuestionsAndValidations();
            }

            this.Energy = new FuelEnergy(6.2f, FuelEnergy.eFuelType.Octan98);
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

            foreach (KeyValuePair<string, Action<Vehicle, string>> pair in FuelEnergy.InstantiationFuelEnergyQueriesAndValidations)
            {
                questionsAndValidations[pair.Key] = pair.Value;
            }

            return questionsAndValidations;
        }

        internal override Dictionary<string, Action<Vehicle, string>> GetAllQuestionsAndValidations()
        {
            return FuelMotorcycle.s_ListOfQuestionsAndValidations;
        }
    }
}
