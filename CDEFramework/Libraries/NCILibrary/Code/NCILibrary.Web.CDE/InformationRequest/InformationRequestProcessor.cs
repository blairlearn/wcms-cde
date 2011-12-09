using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace NCI.Web.CDE.InformationRequest
{
    public class InformationRequestProcessor
    {
        private string _returnMessage = "";
        private string _returnValue = "";

        public string ReturnMessage
        {
            get { return _returnMessage; }
        }

        public string ReturnValue
        {
            get { return _returnValue; }
        }
        
        public InformationRequestProcessor(string command)
        {
            string message = "";
            string value = "";
                        
            WebRequest request = WebRequest.Create(command);
            request.Credentials = CredentialCache.DefaultCredentials;
            WebResponse response = request.GetResponse();

            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            
            reader.Close();
            response.Close();

            int delimitorIndex = -1;
            if ((delimitorIndex = responseFromServer.IndexOf(':')) > -1)
            {
                _returnMessage = responseFromServer.Substring(0, delimitorIndex + 1);
                _returnValue = responseFromServer.Substring(delimitorIndex + 1).Replace('[', ' ').Replace(']', ' ').Trim();
            }
            else
            {
                _returnMessage = responseFromServer;
                _returnValue = "";
            }






        }
    }
}
