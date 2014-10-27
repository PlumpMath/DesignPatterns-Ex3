using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C14_Ex03
{
    public class ReminderManufacturer
    {
        public void Construct(IReminderBuilder i_InviteBuilder, string i_MessegeSubject, string i_MessegeBody, List<object> i_Contents, string i_MailOfUser)
        {
            i_InviteBuilder.buildMessege(i_MessegeSubject, i_MessegeBody);
            i_InviteBuilder.buildContentsEvents(i_Contents);
            i_InviteBuilder.buildUserMail(i_MailOfUser);
        }
    }
}
