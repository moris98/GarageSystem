using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageLogic
{
    internal class Ticket
    {
        private Owner m_TicketOwner;
        private Vehicle m_Vehicle;
        private eGarageState m_State = eGarageState.InRepair;

        internal Vehicle Vehicle
        {
            get { return this.m_Vehicle; }
            set { this.m_Vehicle = value; }
        }

        internal eGarageState State
        {
            get { return this.m_State; }
            set { this.m_State = value; }
        }

        internal Owner Owner
        {
            get { return this.m_TicketOwner; }
            set { this.m_TicketOwner = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.Owner.ToString());
            sb.Append(string.Format("Garage state: {0}\n", this.State.ToString()));
            sb.Append(this.Vehicle.ToString());
            return sb.ToString();
        }

        internal enum eGarageState
        {
            InRepair = 1,
            Repaired,
            Paid
        }
    }
}
