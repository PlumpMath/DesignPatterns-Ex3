using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacebookWrapper.ObjectModel;

namespace C14_Ex03
{
    public interface IMatchFriendBehavior
    {
        bool IsMatch(User i_UserA, User i_UserB);  
    }
}
