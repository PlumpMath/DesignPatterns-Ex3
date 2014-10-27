using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacebookWrapper.ObjectModel;

namespace C14_Ex03
{
    public class CoredUser : IPostStatus
    {
        private User m_user;

        public CoredUser(User i_User)
        {
            m_user = i_User;
        }

        public void PostStatus(string i_StatusText, string i_PrivacyParameterValue = null)
        {
            m_user.PostStatus(i_StatusText, i_PrivacyParameterValue);
        }
    }
}
