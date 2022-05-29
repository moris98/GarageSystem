using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;

namespace GarageLogic
{
    public class Garage
    {
        private readonly List<Ticket> r_GarageTickets = new List<Ticket>();
        private readonly Factory r_Factory = new Factory();
        private Owner m_OwnerOfVehicleInFactory;

        public static List<string> GetSupportedVehicles()
        {
            List<string> supportedVehicles = new List<string>();
            foreach(Factory.eVehicleTranslator vehicleType in Factory.SupportedVehiclesTypes.Keys)
            {
                supportedVehicles.Add(vehicleType.ToString());
            }

            return supportedVehicles;
        }

        public static List<string> GetAllPossibleFuelTypes()
        {
            return Enum.GetNames(typeof(FuelEnergy.eFuelType)).ToList();
        }

        public static void IsValidState(string i_NewStateNumericalStr)
        {
            if (!int.TryParse(i_NewStateNumericalStr, out int stateInt))
            {
                throw new FormatException("Input is not in the right format");
            }

            if (!Enum.IsDefined(typeof(Ticket.eGarageState), stateInt))
            {
                throw new ArgumentException("Requested state isn't valid");
            }
        }

        public static List<string> GetAllPossibleVehiclesStates()
        {
            return Enum.GetNames(typeof(Ticket.eGarageState)).ToList();
        }

        public List<string> AddBasicVehicle(int i_TypeOfCarToAddInt)
        {
            return this.r_Factory.CreateBasicVehicle(i_TypeOfCarToAddInt);
        }

        public void ValidateVehicleAnswer(string i_Question, string i_Answer, ref bool i_StopQuestioning)
        {
            if(i_Question.Equals("Please enter the vehicle's licence plate:"))
            {
                Vehicle foundVehicle = this.findVehicleByLicencePlate(i_Answer);
                if (foundVehicle != null)
                {
                    this.ChangeVehicleStateByPlateNumber(foundVehicle.LicencePlate, ((int)Ticket.eGarageState.InRepair).ToString());
                    this.r_Factory.DestroyCurrentVehicle();
                    i_StopQuestioning = true;
                    return;
                }
            }

            this.r_Factory.ValidateAnswer(i_Question, i_Answer);
        }

        public void ChangeVehicleStateByPlateNumber(string i_LicencePlate, string i_NewGarageStateNumericalStr)
        {
            if (!int.TryParse(i_NewGarageStateNumericalStr, out int newGarageStateInt))
            {
                throw new FormatException("Input is not in the right format");
            }

            if (!Enum.IsDefined(typeof(Ticket.eGarageState), newGarageStateInt))
            {
                throw new ArgumentException("Requested state isn't valid");
            }
            
            Ticket foundTicket = findTicketByLicencePlate(i_LicencePlate);
            if(foundTicket == null)
            {
                throw new ArgumentException("Vehicle not in garage");
            }

            foundTicket.State = (Ticket.eGarageState)newGarageStateInt;
        }

        public List<string> ShowGarageContent(string i_FilteringStateNumericalStr)
        {
            Enum.GetNames(typeof(Ticket.eGarageState)).ToList();
            List<string> vehiclesInGarage = new List<string>();
            List<Ticket> garageTicketsToIterate = this.r_GarageTickets;

            if (!int.TryParse(i_FilteringStateNumericalStr, out int garageStateInt))
            {
                throw new FormatException("Input is not in the right format");
            }

            if (garageStateInt != 0 && !Enum.IsDefined(typeof(Ticket.eGarageState), garageStateInt)) 
            {
                // 0 is allowed as it represents here - the no filter option
                throw new ArgumentException("Please enter an integer representing no filter option or representing any valid state shown");
            }

            if(garageStateInt == 0)
            {
                return this.showAllVehiclesInGarage();
            }
            else
            {
                return this.showVehiclesWithGivenState((Ticket.eGarageState)garageStateInt);
            }
        }

        private List<string> showAllVehiclesInGarage()
        {
            List<string> licencePlates = new List<string>();
            foreach(Ticket currentTicket in this.r_GarageTickets)
            {
                licencePlates.Add(currentTicket.Vehicle.LicencePlate);
            }

            return licencePlates;
        }

        private List<string> showVehiclesWithGivenState(Ticket.eGarageState i_GivenState)
        {
            List<string> licencePlates = new List<string>();
            foreach (Ticket currentTicket in this.r_GarageTickets)
            {
                if(currentTicket.State == i_GivenState)
                {
                    licencePlates.Add(currentTicket.Vehicle.LicencePlate);
                }
            }

            return licencePlates;
        }

        public void InflateWheelsToMaximum(string i_LicencePlate)
        {
            Vehicle vehicleFound = this.findVehicleByLicencePlate(i_LicencePlate);
            if(vehicleFound == null)
            {
                throw new ArgumentException("Vehicle not found in garage");
            }

            vehicleFound.InflateAllWheelsToMaximum();
        }

        public void IsVehicleInGarage(string i_LicencePlate)
        {
            if(this.findVehicleByLicencePlate(i_LicencePlate) == null)
            {
                throw new ArgumentException("Vehicle not in garage");
            }
        }

        public void IsVehicleInGarageAndOfTypeFuel(string i_LicencePlate)
        {
            Vehicle foundVehicle = this.findVehicleByLicencePlate(i_LicencePlate);
            if(foundVehicle == null)
            {
                throw new ArgumentException("Vehicle not in garage");
            }

            if(!(foundVehicle.Energy is FuelEnergy))
            {
                throw new ArgumentException("Vehicle is not energized by fuel");
            }
        }

        public void IsVehicleInGarageAndOfTypeElectric(string i_LicencePlate)
        {
            Vehicle foundVehicle = this.findVehicleByLicencePlate(i_LicencePlate);
            if (foundVehicle == null)
            {
                throw new ArgumentException("Vehicle not in garage");
            }

            if (!(foundVehicle.Energy is ElectricEnergy))
            {
                throw new ArgumentException("Vehicle is not energized by Electricity");
            }
        }

        private Vehicle findVehicleByLicencePlate(string i_LicencePlate)
        {
            Vehicle vehicleFound = null;
            foreach(Ticket currentTicket in this.r_GarageTickets)
            {
                if(currentTicket.Vehicle.LicencePlate.Equals(i_LicencePlate))
                {
                    vehicleFound = currentTicket.Vehicle;
                }
            }

            return vehicleFound;
        }

        private Ticket findTicketByLicencePlate(string i_LicencePlate)
        {
            Ticket foundTicket = null;
            foreach (Ticket currentTicket in this.r_GarageTickets)
            {
                if (currentTicket.Vehicle.LicencePlate.Equals(i_LicencePlate))
                {
                    foundTicket = currentTicket;
                }
            }

            return foundTicket;
        }

        public void CreateTicket()
        {
            Ticket newTicket = new Ticket();
            this.r_Factory.AddCurrentVehicleToTicket(newTicket);
            newTicket.Owner = this.m_OwnerOfVehicleInFactory;
            this.r_GarageTickets.Add(newTicket);
        }

        public List<string> GetOwnerQuestions()
        {
            this.m_OwnerOfVehicleInFactory = new Owner();
            return Owner.InstantiationOwnerQueriesAndValidations.Keys.ToList();
        }

        public void ValidateOwnerAnswer(string i_Question, string i_Answer)
        {
            Owner.InstantiationOwnerQueriesAndValidations[i_Question].Invoke(this.m_OwnerOfVehicleInFactory, i_Answer);
        }

        public void ValidateFuelType(string i_LicencePlate, string i_FuelTypeNumericalStr)
        {
            if (!int.TryParse(i_FuelTypeNumericalStr, out int fuelTypeInt))
            {
                throw new FormatException("Input is not in the right format");
            }

            if (!Enum.IsDefined(typeof(FuelEnergy.eFuelType), fuelTypeInt))
            {
                throw new ArgumentException("Requested fuel type isn't valid");
            }

            this.validateFuelTypeForVehicle(i_LicencePlate, i_FuelTypeNumericalStr);
        }

        private void validateFuelTypeForVehicle(string i_LicencePlate, string i_FuelTypeNumericalStr)
        {
            Vehicle foundVehicle = this.findVehicleByLicencePlate(i_LicencePlate);
            if(foundVehicle == null)
            {
                throw new ArgumentException("Vehicle not in garage");
            }

            if(!(foundVehicle.Energy is FuelEnergy))
            {
                throw new ArgumentException("Vehicle isn't energized by fuel");
            }

            if (((FuelEnergy)foundVehicle.Energy).FuelType != (FuelEnergy.eFuelType)int.Parse(i_FuelTypeNumericalStr))
            {
                throw new ArgumentException("Vehicle isn't compatible with the requested fuel type");
            }
        }

        public void AddFuel(string i_LicencePlate, string i_FuelTypeNumericalStr, string i_FuelAmountStr)
        {
            if (!float.TryParse(i_FuelAmountStr, out float fuelAmountFloat))
            {
                throw new FormatException("fuel amount is not in the right format");
            }

            this.validateFuelTypeForVehicle(i_LicencePlate, i_FuelTypeNumericalStr);
            Vehicle foundVehicle = this.findVehicleByLicencePlate(i_LicencePlate);
            foundVehicle.Energy.AddEnergy(fuelAmountFloat); // An exception will be thrown if the fuel amount isn't a legal amount
        }

        public void AddElectricity(string i_LicencePlate, string i_AmountOfMinutes)
        {
            if (!float.TryParse(i_AmountOfMinutes, out float amountOfMinutesFloat))
            {
                throw new FormatException("Amount of minutes is not in the right format");
            }

            Vehicle foundVehicle = this.findVehicleByLicencePlate(i_LicencePlate);
            foundVehicle.Energy.AddEnergy(amountOfMinutesFloat);
        }

        public string showTicketDetails(string i_LicencePlate)
        {
            Ticket foundTicket = this.findTicketByLicencePlate(i_LicencePlate);
            if(foundTicket == null)
            {
                throw new ArgumentException("Vehicle is not in garage.");
            }

            return foundTicket.ToString();
        }
    }
}
