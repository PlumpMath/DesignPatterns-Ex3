using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacebookWrapper.ObjectModel;

namespace C14_Ex03
{
    public class MatchByCity : IMatchFriendBehavior
    {
        public bool IsMatch(User i_UserA, User i_UserB)
        {
            string[] address = new string[2];
            address = i_UserA.Hometown.Name.Split(',');
            string firstUserCity = address[0];
            address = i_UserB.Hometown.Name.Split(',');
            string SecondeUserCity = address[0];

            return firstUserCity.Equals(SecondeUserCity);
        }
    }
}
