using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C14_Ex03
{
    public interface IReminderBuilder
    {
        void buildMessege(string i_Subject, string i_Body);

        void buildContentsEvents(List<object> i_ListOfContent);

        void buildUserMail(string mail);

        Reminder RemiderNote 
        { 
            get; 
        }
    }
}
