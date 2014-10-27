using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C14_Ex03
{
    public interface IPostStatus
    {
        void PostStatus(string i_StatusText, string i_PrivacyParameterValue = null);
    }
}
