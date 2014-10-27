using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C14_Ex03
{
    public class DecoratedUserBackupPost : DecoratedUser
    {
        public DecoratedUserBackupPost(IPostStatus i_DecoratedPost) :
            base(i_DecoratedPost)
        {
        }

        public override void PostStatus(string i_StatusText, string i_PrivacyParameterValue = null)
        {
            m_DecoratedPost.PostStatus(i_StatusText, i_PrivacyParameterValue);
            LogFacebookPostByUser.Instance.WriteToLogFileSuccessfullPost(i_StatusText);
        }
    }
}
