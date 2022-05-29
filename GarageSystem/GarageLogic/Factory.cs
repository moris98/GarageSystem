using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageLogic
{
    internal class Factory
    {
        private static readonly Dictionary<eVehicleTranslator, Func<Vehicle>> sr_SupportedVehiclesTypes = new Dictionary<eVehicleTranslator, Func<Vehicle>>()
                {
                    { eVehicleTranslator.FuelCar, () => new FuelCar() },
                    { eVehicleTranslator.ElectricCar, () => new ElectricCar() },
                    { eVehicleTranslator.FuelMotorcycle, () => new FuelMotorcycle() },
                    { eVehicleTranslator.ElectricMotorcycle, () => new ElectricMotorcycle() },
                    { eVehicleTranslator.FuelTruck, () => new FuelTruck() }
                };

        private Vehicle m_CurrentVehicle;

        internal static Dictionary<eVehicleTranslator, Func<Vehicle>> SupportedVehiclesTypes
        {
            get { return Factory.sr_SupportedVehiclesTypes; }
        }

        internal List<string> CreateBasicVehicle(int i_TypeOfCarToAddInt)
        {
            eVehicleTranslator vehicleType = (eVehicleTranslator)i_TypeOfCarToAddInt;
            this.m_CurrentVehicle = sr_SupportedVehiclesTypes[vehicleType].Invoke(); // Construct a new vehicle with the requested type

            return this.m_CurrentVehicle.GetAllQuestionsAndValidations().Keys.ToList();
        }

        internal void ValidateAnswer(string i_Question, string i_Answer)
        {
            this.m_CurrentVehicle.GetAllQuestionsAndValidations()[i_Question].Invoke(this.m_CurrentVehicle, i_Answer);
        }

        internal void AddCurrentVehicleToTicket(Ticket i_Ticket)
        {
            if(i_Ticket == null)
            {
                throw new Exception("No Ticket in garage, can't add vehicle");
            }

            i_Ticket.Vehicle = this.m_CurrentVehicle;
        }

        internal void DestroyCurrentVehicle()
        {
            this.m_CurrentVehicle = null;
        }

        internal enum eVehicleTranslator
        {
            FuelCar = 1,
            ElectricCar,
            FuelMotorcycle,
            ElectricMotorcycle,
            FuelTruck
        }
    }
}
