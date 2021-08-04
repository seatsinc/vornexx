﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Http;

namespace BarcodeScanner
{

    public enum httpVerb
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    class RestClient
    {

        

        public async void makeRequest(string endPoint, httpVerb httpMethod, byte[] postData = null)
        {

            await Task.Run(() =>
            {
                try
                {

                    System.Net.HttpWebRequest.DefaultWebProxy = null;
                    ServicePointManager.DefaultConnectionLimit = 20;
                    ServicePointManager.Expect100Continue = false;



                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPoint);


                    request.Method = httpMethod.ToString();

                    if (postData != null)
                    {
                        using (Stream requestStream = request.GetRequestStream())
                        {
                            requestStream.Write(postData, 0, postData.Length);

                            requestStream.Close();
                        }
                    }


                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {

                        // Process the response stream... (could be JSON, XML or HTML etc...)

                        using (Stream responseStream = response.GetResponseStream())
                        {
                            if (responseStream != null)
                            {
                                using (StreamReader reader = new StreamReader(responseStream))
                                {
                                    string strResponseValue = reader.ReadToEnd();

                                    reader.Close();
                                } // end of StreamReader
                            }

                            responseStream.Close();
                        } // end of ResponseStream

                        response.Close();

                    }
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.ToString());
                }

            });
        }
    }


}
