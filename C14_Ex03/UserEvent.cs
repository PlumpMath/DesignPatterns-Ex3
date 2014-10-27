using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using FacebookWrapper.ObjectModel;
using FacebookWrapper;

namespace C14_Ex03
{
    public class UserEvent : IEnumerable
    {
        private readonly FacebookObjectCollection<Event> r_Events = new FacebookObjectCollection<Event>();
        private User m_User;

        public UserEvent(User i_User)
        {
            m_User = i_User;
            r_Events = FacebookService.GetCollection<Event>("Events", this.m_User.Id);           
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new EventAttendingsIterator(this);
        }

        private class EventAttendingsIterator : IEnumerator
        {
            private UserEvent m_Collection;
            private int m_CurrentIdx = -1;
            private int m_Count = -1;

            public EventAttendingsIterator(UserEvent i_Collection)
            {
                m_Collection = i_Collection;
                m_Count = m_Collection.r_Events.Count;
            }

            public object Current
            {
                get
                {
                    return m_Collection.r_Events[m_CurrentIdx];
                }
            }
            
            public bool MoveNext()
            {
                ++m_CurrentIdx;
               
                return m_CurrentIdx < m_Collection.r_Events.Count;
            }

            public void Reset()
            {
                m_CurrentIdx = 0;
            }
        }
    }
}
