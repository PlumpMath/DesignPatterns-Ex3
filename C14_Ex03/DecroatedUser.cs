using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C14_Ex03
{
    public class DecoratedUser : IPostStatus
    {
        protected IPostStatus m_DecoratedPost;

        public DecoratedUser(IPostStatus i_DecoratedPost)
        {
            m_DecoratedPost = i_DecoratedPost;
        }

        public virtual void PostStatus(string i_StatusText, string i_PrivacyParameterValue = null)
        {
            m_DecoratedPost.PostStatus(i_StatusText, i_PrivacyParameterValue);
        }
    }
}
