using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Diagnostics;

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
        private int retries;

        public RestClient(int to, int retr)
        {
            this.timeout = to;
            this.retries = retr;
        }


        public string makeRequest(string endPoint, httpVerb httpMethod, int numRetries = 0)
        {



            System.Net.HttpWebRequest.DefaultWebProxy = null;
            ServicePointManager.DefaultConnectionLimit = 20;
            ServicePointManager.Expect100Continue = false;

            string strResponseValue = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPoint);

            request.Timeout = this.timeout;
            request.ReadWriteTimeout = this.timeout;

            request.Method = httpMethod.ToString();

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {

                    // Process the response stream... (could be JSON, XML or HTML etc...)

                    using (Stream responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            using (StreamReader reader = new StreamReader(responseStream))
                            {
                                strResponseValue = reader.ReadToEnd();

                                reader.Close();
                            } // end of StreamReader
                        }

                        responseStream.Close();
                    } // end of ResponseStream

                    response.Close();

                }

                return strResponseValue;
            }
            catch (Exception exc)
            {
                if (numRetries >= this.retries)
                {
                    return string.Empty;
                }
                else
                {
                    return this.makeRequest(endPoint, httpMethod, numRetries + 1);
                }
            }

        }

    }
}
