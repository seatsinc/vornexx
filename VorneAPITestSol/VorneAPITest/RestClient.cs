using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace VorneAPITest
{
    public enum httpVerb
    {
        GET,
        POST,
        PUT,
        DELETE
    }


    public class RestClient
    {

        private int timeout;
        public RestClient(int t)
        {
            this.timeout = t;
            
        }

        public string makeRequest(string endPoint, httpVerb httpMethod)
        {
            string strResponseValue = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPoint);

            request.Timeout = this.timeout;

            request.Method = httpMethod.ToString();

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new ApplicationException("error code " + response.StatusCode.ToString());
                }
                // Process the response stream... (could be JSON, XML or HTML etc...)

                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();
                        } // end of StreamReader
                    }
                } // end of ResponseStream
            }

            return strResponseValue;

        }

    }
}
