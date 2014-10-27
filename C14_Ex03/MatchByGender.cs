using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacebookWrapper.ObjectModel;

namespace C14_Ex03
{
    public class MatchByGender : IMatchFriendBehavior
    {
        public bool IsMatch(User i_UserA, User i_UserB)
        {
            string firstUserGender = i_UserA.Gender.ToString();
            string secondUserGender = i_UserB.Gender.ToString();
            return firstUserGender.Equals(secondUserGender);
        }
    }
}
