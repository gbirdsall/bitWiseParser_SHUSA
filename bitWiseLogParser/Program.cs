using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bitWiseLogParser
{

    public class ftpSession
    {
        public int sessionID;
        public string loginName;
        public string ftpFolder;

        public ftpSession(int sID, string lName, string fFolder)
        {
            this.sessionID = sID;
            this.loginName = lName;
            this.ftpFolder = fFolder;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //init variables

            string strSessionID;
            int curSessionID = 0;
            Boolean firstRecord = true;
            string curString = "";
            string clientLogin = "";
            string clientFolder = "";
            Boolean sessionFound = false;
            int listRecord = 0;

            //Read file into temp memory
            string[] lines = System.IO.File.ReadAllLines(args[0]);

            //collection init stuff
            List<ftpSession> sessions = new List<ftpSession>();
            


            System.Console.WriteLine("File Contents =");
            foreach (string line in lines)
            {

                //Find Sesssion Thread ID

                int indexSession = line.IndexOf("Session thread");

                if ( (indexSession >= 0) & (firstRecord) )
                {
                    firstRecord = false;
                    strSessionID = line.Substring(indexSession+14, 5);
                    curSessionID = Int32.Parse(strSessionID);
                    curString = line;
                    //curSessionID = strSessionID.toInt;
                }
                else if (indexSession >= 0)
                {
                    

                    if (curString.Contains("Windows account"))
                    {
                        int index = curString.IndexOf("Windows account '");
                        int lastIndex = curString.IndexOf("'", index + 17);

                        //Console.WriteLine("index: " + index + " lastIndex: " + lastIndex);

                        clientLogin = curString.Substring(index + 17, lastIndex - (index + 18));

                        //Console.WriteLine(curSessionID + " " + clientLogin);
                        //Console.WriteLine("\t" + index + " " + line);
                    }

                    if (curString.Contains("Opened directory '"))
                    {
                        int dirIndex = curString.IndexOf("Opened directory '");
                        int dirLastIndex = curString.IndexOf("'", dirIndex + 18);

                        //Console.WriteLine("index: " + index + " lastIndex: " + lastIndex);

                        clientFolder = curString.Substring(dirIndex + 18, dirLastIndex - (dirIndex + 19));
                    }

                    //Console.WriteLine(curSessionID + " " + clientLogin + " " + clientFolder);
                    //This is the point where we have all of the items


                    for (int i = 0; i < sessions.Count; i++)
                    {
                        if (sessions[i].sessionID == curSessionID)
                        {
                            sessionFound = true;
                            listRecord = i;
                        }
                    }

                    if (!sessionFound)
                    {
                        sessions.Add(new ftpSession(curSessionID, clientLogin, clientFolder));
                    }

                    sessionFound = false;
                    curString = line;
                    strSessionID = line.Substring(indexSession + 14, 5);
                    curSessionID = Int32.Parse(strSessionID);

                }
                else if (indexSession == -1)  // Not Found
                {
                    curString += line;
                }

                // if the string contains 'Windows Account' write the part that is in single quotes
                //Console.WriteLine("\t" + line);

                //if (line.Contains("Windows account"))
                //{
                 //   int index = line.IndexOf("'");
                 //   int lastIndex = line.IndexOf("'", index + 1);

                    //clientLogin = line.Substring(index + 1, lastIndex - (index+1) );

                    //int sessionIndex = line.IndexOf

                    //Console.WriteLine(curSessionID + " " + clientLogin);
                    //Console.WriteLine("\t" + index + " " + line);
                //}

            }


            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(args[1]))
            {
                foreach (ftpSession session in sessions)
                {

                    file.WriteLine(session.sessionID + "," + session.loginName + "," + session.ftpFolder);
                }
            }





         //   foreach (ftpSession session in sessions)
         //   {
         //       Console.WriteLine(session.sessionID + " " + session.loginName + " " + session.ftpFolder);
         //   }

         //   Console.ReadLine();
        }
    }
}
