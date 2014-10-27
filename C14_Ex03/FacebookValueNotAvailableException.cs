using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C14_Ex03
{
    public class FacebookValueNotAvailableException : Exception
    {
        public FacebookValueNotAvailableException(string i_Messege) : base(i_Messege)
        {
        }
    }
}
