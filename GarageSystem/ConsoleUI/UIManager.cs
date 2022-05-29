using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using GarageLogic;

namespace ConsoleUI
{
    public class UIManager
    {
        private static readonly List<string> sr_MenuOptions = new List<string>()
          {
              "1. Add a car to the garage",
              "2. Show all cars",
              "3. Modify vehicle state by licence plate",
              "4. Inflate all wheels to maximum by licence plate",
              "5. Fuel vehicle by licence plate, fuel type and amount to fuel",
              "6. Charge a vehicle by licence plate, and amount of minutes",
              "7. Show all details of a vehicle by licence plate",
              "8. Quit the system"
          };

        private readonly Garage r_GarageObj = new Garage();

        internal void showMenu()
        {
            bool choseToQuit = false;
            while(!choseToQuit)
            {
                Console.WriteLine("\n");
                Console.WriteLine("Please choose from the below options (Enter the digit for the desired action): \n");
                foreach(string option in sr_MenuOptions)
                {
                    Console.WriteLine(option);
                }

                Console.WriteLine();
                string actionToBeTakenStr = Console.ReadLine();
                bool isValidAction = int.TryParse(actionToBeTakenStr, out int actionToBeTakenInt);
                if(!isValidAction || actionToBeTakenInt < 1 || actionToBeTakenInt > sr_MenuOptions.Count)
                {
                    Console.WriteLine("Wrong input. Try again. \n");
                    showMenu();
                }
                else
                {
                    switch(actionToBeTakenInt)
                    {
                        case 1:
                            addVehicleToGarageFlow();
                            break;
                        case 2:
                            showAllVehiclesInGarage();
                            break;
                        case 3:
                            changeVehicleState();
                            break;
                        case 4:
                            inflateWheelsToMaximum();
                            break;
                        case 5:
                            addFuelToVehicle();
                            break;
                        case 6:
                            addElecrticityToVehicle();
                            break;
                        case 7:
                            showVehicleDetailsByLicencePlate();
                            break;
                        case 8:
                            choseToQuit = true;
                            break;
                    }
                }
            }
        }

        private void addVehicleToGarageFlow()
        {
            List<string> listSupportedVehicles = Garage.GetSupportedVehicles();
            Console.WriteLine("\nHere are the vehicles we support:\n");
            int indexSupportedVehicle = 1;
            foreach(string supportedVehicle in listSupportedVehicles)
            {
                Console.WriteLine(string.Format("{0}: {1}", indexSupportedVehicle, supportedVehicle));
                indexSupportedVehicle++;
            }

            Console.WriteLine();
            Console.WriteLine("Enter the digit your vehicle corresponds to:");
            string typeOfCarToAddStr = Console.ReadLine();

            bool isValidType = int.TryParse(typeOfCarToAddStr, out int typeOfCarToAddInt);
            if (!isValidType || typeOfCarToAddInt < 1 || typeOfCarToAddInt > listSupportedVehicles.Count)
            {
                Console.WriteLine("Wrong input. Try again.");
                addVehicleToGarageFlow();
            }
            else
            {
                List<string> vehicleQuestionsToAsk = this.r_GarageObj.AddBasicVehicle(typeOfCarToAddInt);
                askVehicleQuestions(vehicleQuestionsToAsk);
                List<string> ownerQuestionsToAsk = this.r_GarageObj.GetOwnerQuestions();
                askOwnerQuestions(ownerQuestionsToAsk);
                this.r_GarageObj.CreateTicket();
            }
        }

        private void askOwnerQuestions(List<string> i_Questions)
        {
            foreach(string question in i_Questions)
            {
                bool isValidAnswer = false;
                while (!isValidAnswer)
                {
                    try
                    {
                        Console.WriteLine(question);
                        string answer = Console.ReadLine();
                        this.r_GarageObj.ValidateOwnerAnswer(question, answer);
                        isValidAnswer = true; // Will reach this statement only if exception didn't occur
                    }
                    catch (Exception e)
                    { 
                        // Exception has occurred
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }

        private void askVehicleQuestions(List<string> i_Questions)
        {
            foreach(string question in i_Questions)
            {
                bool stopQuestioning = false;
                bool isValidAnswer = false;
                while(!isValidAnswer)
                {
                    try
                    {
                        Console.WriteLine(question);
                        string answer = Console.ReadLine();
                        this.r_GarageObj.ValidateVehicleAnswer(question, answer, ref stopQuestioning); // The our variable to indicate if the vehicle already exists 
                        if(stopQuestioning == true)
                        {
                            Console.WriteLine("Vehicle is already in garage, The Vehicle's state was change to - in repair");
                            return;
                        }

                        isValidAnswer = true;
                    }
                    catch(Exception e) 
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }

        private void showAllVehiclesInGarage()
        {
            List<string> garageVehicles = new List<string>();
            string filteringState = string.Empty;
            bool isValidInput = false;
            List<string> validStates = Garage.GetAllPossibleVehiclesStates();
            while(!isValidInput)
            {
                Console.WriteLine("Please enter if you want to filter vehicles by state and if so, by which one");
                Console.WriteLine("0. Show all cars");
                int indexOfPossibleVehicle = 1;
                foreach (string validState in validStates)
                {
                    Console.WriteLine(string.Format("{0}: {1}", indexOfPossibleVehicle, validState));
                    indexOfPossibleVehicle++;
                }

                filteringState = Console.ReadLine();

                try
                {
                    garageVehicles = this.r_GarageObj.ShowGarageContent(filteringState);
                    isValidInput = true; // If we reached this statement, no exception occurred
                }
                catch(FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch(ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            foreach (string licencePlate in garageVehicles)
            {
                Console.WriteLine(licencePlate);
            }
        }

        private void changeVehicleState()
        {
            string inputLicencePlate = string.Empty;
            string inputNewVehicleStateNumericalStr = string.Empty;
            bool isValidInput = false;
            while(!isValidInput)
            {
                Console.WriteLine("Please enter the vehicle's licence plate: ");
                inputLicencePlate = Console.ReadLine();

                try
                {
                    this.r_GarageObj.IsVehicleInGarage(inputLicencePlate);
                    isValidInput = true;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            isValidInput = false;
            while (!isValidInput)
            {
                Console.WriteLine("Please enter the new vehicle's state from the below options: (Enter the digit for the desired state)");
                List<string> possibleVehiclesStates = Garage.GetAllPossibleVehiclesStates();
                int indexOfPossibleState = 1;
                foreach(string stateStr in possibleVehiclesStates)
                {
                    Console.WriteLine(string.Format("{0}. {1}", indexOfPossibleState, stateStr));
                    indexOfPossibleState++;
                }

                inputNewVehicleStateNumericalStr = Console.ReadLine();
                try
                {
                    Garage.IsValidState(inputNewVehicleStateNumericalStr);
                    isValidInput = true;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            this.r_GarageObj.ChangeVehicleStateByPlateNumber(inputLicencePlate, inputNewVehicleStateNumericalStr);
        }

        private void inflateWheelsToMaximum()
        {
            string inputLicencePlate = string.Empty;
            bool isValidInput = false;
            while (!isValidInput)
            {
                Console.WriteLine("Please enter the vehicle's licence plate: ");
                inputLicencePlate = Console.ReadLine();

                try
                {
                    this.r_GarageObj.InflateWheelsToMaximum(inputLicencePlate);
                    Console.WriteLine(string.Format("Wheels of Vehicle {0} were inflated successfully", inputLicencePlate));
                    isValidInput = true;
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private void addFuelToVehicle()
        {
            string inputLicencePlate = string.Empty;
            string inputFuelTypeNumericalStr = string.Empty;
            string inputFuelAmountStr = string.Empty;
            bool isValidInput = false;
            while(!isValidInput)
            {
                Console.WriteLine("Please enter the licence plate number of the vehicle");
                inputLicencePlate = Console.ReadLine();
                try
                {
                    this.r_GarageObj.IsVehicleInGarageAndOfTypeFuel(inputLicencePlate);
                    isValidInput = true;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
            }

            isValidInput = false;
            while(!isValidInput)
            {
                Console.WriteLine("Enter desired fuel type to be added from the following options (select the type of fuel by digit):");
                List<string> possibleFuelTypes = Garage.GetAllPossibleFuelTypes();
                int fuelTypeIndex = 1;
                foreach(string fuelType in possibleFuelTypes)
                {
                    Console.WriteLine(string.Format("{0}. {1}", fuelTypeIndex, fuelType));
                    fuelTypeIndex++;
                }

                inputFuelTypeNumericalStr = Console.ReadLine();
                try
                {
                    this.r_GarageObj.ValidateFuelType(inputLicencePlate, inputFuelTypeNumericalStr);
                    isValidInput = true;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            isValidInput = false;
            while(!isValidInput)
            {
                Console.WriteLine("Enter desired amount of fuel to be added");
                inputFuelAmountStr = Console.ReadLine();
                try
                {
                    this.r_GarageObj.AddFuel(inputLicencePlate, inputFuelTypeNumericalStr, inputFuelAmountStr);
                    isValidInput = true;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private void addElecrticityToVehicle()
        {
            string inputLicencePlate = string.Empty;
            string inputAmountOfMinutes = string.Empty;
            bool isValidInput = false;
            while (!isValidInput)
            {
                Console.WriteLine("Please enter the licence plate number of the vehicle");
                inputLicencePlate = Console.ReadLine();
                try
                {
                    this.r_GarageObj.IsVehicleInGarageAndOfTypeElectric(inputLicencePlate);
                    isValidInput = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
            }

            isValidInput = false;
            while (!isValidInput)
            {
                Console.WriteLine("Enter desired amount of minutes to charge");
                inputAmountOfMinutes = Console.ReadLine();
                try
                {
                    this.r_GarageObj.AddElectricity(inputLicencePlate, inputAmountOfMinutes);
                    isValidInput = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private void showVehicleDetailsByLicencePlate()
        {
            string inputLicencePlate = string.Empty;
            bool isValidInput = false;
            string vehicleDetails = string.Empty;
            while (!isValidInput)
            {
                Console.WriteLine("Please enter the licence plate number of the vehicle");
                inputLicencePlate = Console.ReadLine();
                try
                {
                    vehicleDetails = this.r_GarageObj.showTicketDetails(inputLicencePlate);
                    isValidInput = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            Console.WriteLine(vehicleDetails);
        }
    }
}
