using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacebookWrapper.ObjectModel;
using FacebookWrapper;

namespace C14_Ex03
{
    public class FacebookReminderBuilder : IReminderBuilder
    {
        private Reminder m_Reminder;

        public FacebookReminderBuilder()
        {
            m_Reminder = new Reminder();
        }

        public void buildMessege(string i_Subject, string i_Body)
        {
            ReminderMessege messege = new ReminderMessege(i_Subject, i_Body);
            m_Reminder.InviteMessegeInfo = messege;
        }

        public void buildContentsEvents(List<object> i_ListOfContent)
        {
            List<string> contentList = new List<string>();

            foreach (object item in i_ListOfContent)
            {
                if(item is Event)
                {
                    contentList.Add((item as Event).LinkToFacebook);
                }
                else
                {
                    continue;
                }
            }

            m_Reminder.EventsLink = contentList;
        }

        public void buildUserMail(string i_Mail)
        {
            m_Reminder.UserMailInfo = i_Mail;
        }

        public Reminder RemiderNote
        {
            get
            {
                return m_Reminder;
            }
        }
    }
}
