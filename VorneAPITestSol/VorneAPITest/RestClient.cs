using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Http;

namespace VorneAPITest
{
    
    public class RestClient
    {
        private int timeout;

        public RestClient(int t)
        {
            this.timeout = t;
        }


        public async Task<string> getAsync(string uri)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMilliseconds(this.timeout);

                    return await client.GetStringAsync(uri);
                }
            }
            catch (Exception exc)
            {
                return string.Empty;
            }
        }

    }
}
