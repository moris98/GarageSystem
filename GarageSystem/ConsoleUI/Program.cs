using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GarageLogic;

namespace ConsoleUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            UIManager uiManager = new UIManager();
            uiManager.showMenu();
        }
    }
}
