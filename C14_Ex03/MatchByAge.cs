using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacebookWrapper.ObjectModel;

namespace C14_Ex03
{
    public class MatchByAge : IMatchFriendBehavior
    {
        public bool IsMatch(User i_UserA, User i_UserB)
        {
            int firstUserAge;
            int secondUserAge;
            bool returnValue;
            firstUserAge = findAge(i_UserA.Birthday);
            secondUserAge = findAge(i_UserB.Birthday);
   
            if(firstUserAge == secondUserAge)
            {
                 returnValue = true;
            }
            else
            {
                 returnValue = false;
            }

            return returnValue;
        }

        private int findAge(string i_UserBirthday)
        {
            int year;
            string[] date = new string[3];
            date = i_UserBirthday.Split('/');
            year = int.Parse(date[2]);
            return year;           
        }
    }
}
