using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace C14_Ex03
{
    public class ReminderMessege
    {
        private string m_Subject;
        private string m_Body;

        public ReminderMessege(string i_Subject, string i_Body)
        {
            m_Body = i_Body;
            m_Subject = i_Subject;
        }

        public string MessegeBody
        {
            get 
            {
                return m_Body;
            }
        }

        public string MessegeSubject
        {
            get
            {
                return m_Subject;
            }
        }
    }
}
