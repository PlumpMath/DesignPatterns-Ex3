using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacebookWrapper.ObjectModel;

namespace C14_Ex03
{
    public class Matcher
    {
        public IMatchFriendBehavior MatcherTool 
        {
            get;
            set;
        }

        public Matcher(IMatchFriendBehavior i_Matcher)
        {
            MatcherTool = i_Matcher;
        }

        public Matcher()
        {
            MatcherTool = null;
        }

        public List<User> FindMatch(User i_UserToMatch, List<User> i_Users)
        {
            List<User> matchedUsers = new List<User>();

            foreach (User user in i_Users)
            {
                if(MatcherTool.IsMatch(i_UserToMatch, user))
                {
                    matchedUsers.Add(user);
                }
            }

            return matchedUsers;
        }
    }
}
