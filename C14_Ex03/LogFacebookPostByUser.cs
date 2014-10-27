using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace C14_Ex03
{
    public sealed class LogFacebookPostByUser
    {      
        private static LogFacebookPostByUser s_LogSystem = null;
        private static object s_LockObj = new object();
        private FileStream m_LogFile;
        private string m_FileName = "FacebookLogFile.txt"; 

        private LogFacebookPostByUser() 
        {
            if (File.Exists(m_FileName))
            {
                m_LogFile = File.OpenWrite(m_FileName);
                m_LogFile.Seek(0, SeekOrigin.End);
            }
            else
            {
                m_LogFile = File.Create(m_FileName);
            }

            m_LogFile.Close();
        }

        public static LogFacebookPostByUser Instance
        {
            get
            {
                if (s_LogSystem == null)
                {
                    lock (s_LockObj)
                    {
                        if (s_LogSystem == null)
                        {
                            s_LogSystem = new LogFacebookPostByUser();
                        }
                    }
                }

                return s_LogSystem;
            }
        }

        public void WriteToLogFileSuccessfullPost(string i_Messege)
        {
            string writeToFile = string.Format("Data Time: {0}{1}Status: Success to post{1}Posted Messege:{2}{1}", DateTime.Now, System.Environment.NewLine, i_Messege);
            File.AppendAllText(m_FileName, writeToFile);
        }

        public void WriteToLogFileException(string i_MessegeTryToPost, string i_Exception)
        {
            string writeToFile = string.Format("Data Time: {0}{1}Status: Failed to post{1}Desired To Post Messege:{2}{1}Exception:{3}{1}", DateTime.Now, System.Environment.NewLine, i_MessegeTryToPost, i_Exception);
            File.AppendAllText(m_FileName, writeToFile);
        }
    }
}
