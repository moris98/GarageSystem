using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GarageLogic
{
    internal class Owner
    {
        private static readonly Dictionary<string, Action<Owner, string>> sr_InstantiationOwnerQueriesAndValidations = new Dictionary<string, Action<Owner, string>>
            {
                { "Please enter the vehicle's owner full name:", new Action<Owner, string>(ValidateOwnerName) },
                { "Please enter the vehicle's owner phone number :", new Action<Owner, string>(ValidateOwnerPhoneNumber) }
            };

        private string m_FullName;
        private string m_PhoneNumber;

        internal static Dictionary<string, Action<Owner, string>> InstantiationOwnerQueriesAndValidations
        {
            get { return Owner.sr_InstantiationOwnerQueriesAndValidations; }
        }

        public override string ToString()
        {
            return string.Format("Owner's full name: {0}, Owner's phone number: {1}\n", this.m_FullName, this.m_PhoneNumber);
        }

        internal static void ValidateOwnerName(Owner i_CurrentOwner, string i_OwnerFullName) // ADD regex validation
        {
            if(i_CurrentOwner == null)
            {
                throw new Exception("Owner is null");
            }

            if(i_OwnerFullName.Length < 2)
            {
                throw new FormatException("Owner name is not in a valid format - must be 2 characters long at least");
            }

            i_CurrentOwner.m_FullName = i_OwnerFullName;
        }

        internal static void ValidateOwnerPhoneNumber(Owner i_CurrentOwner, string i_OwnerPhoneNumber) // ADD digit validation
        {
            if (i_CurrentOwner == null)
            {
                throw new Exception("Owner is null");
            }

            if(!Regex.Match(i_OwnerPhoneNumber, "^[0-9]{10}$").Success)
            {
                throw new FormatException("Owner's phone number is not in a valid format - must be 10 digits");
            }
            
            i_CurrentOwner.m_PhoneNumber = i_OwnerPhoneNumber;
        }
    }
}
