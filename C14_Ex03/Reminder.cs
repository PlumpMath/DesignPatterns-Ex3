using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;

namespace C14_Ex03
{
    public class Reminder
    {
        private List<string> m_Events;
        private ReminderMessege m_Messege;
        private string m_UserMail;

        public ReminderMessege InviteMessegeInfo
        {
            get
            {
                return m_Messege;
            }

            set
            {
                m_Messege = value;
            }
        }

        public string UserMailInfo
        {
            get
            {
                return m_UserMail;
            }

            set
            {
                m_UserMail = value;
            }
        }

        public List<string> EventsLink
        {
            get
            {
                return m_Events;
            }

            set
            {
                m_Events = value;
            }
        }

        public void sendUserEventsReminder()
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("lior032@gmail.com");
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("lior032", "c6535c6535");
            SmtpServer.EnableSsl = true;
            mail.Subject = m_Messege.MessegeSubject;
            mail.To.Add(m_UserMail);
            StringBuilder eventsLinks = new StringBuilder();
            int counter = 1;
            string detail = string.Empty;

            foreach (string eventLink in m_Events)
            {
                detail = string.Format("Event {0} : {1}. {2}", counter, eventLink, System.Environment.NewLine);
                eventsLinks.AppendLine(detail);
                counter++;
            }

            string detailsInfo = string.Format("{0}Your Events:{0}{1}", System.Environment.NewLine, eventsLinks.ToString());
            mail.Body = string.Concat(m_Messege.MessegeBody, detailsInfo);
            try
            {
                SmtpServer.Send(mail);
                MessageBox.Show("mail Send");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
