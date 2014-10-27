using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacebookWrapper.ObjectModel;

namespace C14_Ex03
{
    public class MatchByCountry : IMatchFriendBehavior
    {
        public bool IsMatch(User i_UserA, User i_UserB)
        {
            string firstUserCountry = extractCountry(i_UserA);
            string SecondeUserCountry = extractCountry(i_UserB);

            return firstUserCountry.Equals(SecondeUserCountry);
            throw new NotImplementedException();
        }

        private string extractCountry(User i_User)
        {
            string[] address;
            address = i_User.Hometown.Name.Split(',');
            string returnValue;

            if (address.Length == 1)
            {
                returnValue = address[0];
            }
            else
            {
                returnValue = address[1];
            }

            return returnValue;
        }
    }
}
