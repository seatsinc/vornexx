using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VorneAPITest
{
    public class Util
    {
        public static string replAwBStr(string s, char a, char b)
        {
            string s2 = "";

            foreach (char c in s)
            {
                if (c == a)
                {
                    s2 += b;
                }
                else
                {
                    s2 += c;
                }
            }

            return s2;

        }

     
    }
}
