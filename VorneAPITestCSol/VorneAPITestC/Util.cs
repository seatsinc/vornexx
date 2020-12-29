using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VorneAPITestC
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

        public static string percentRound(string d, int decs)
        {
            double d2 = Convert.ToDouble(d);

            return Math.Round((d2 * 100), decs).ToString();
        }

        public static string removeWS(string s)
        {
            string ns = "";

            for (int i = 0; i < s.Length; ++i)
            {
                if (s[i] != ' ')
                    ns += s[i];
            }

            return ns;
        }


        public static string replaceStringBounds(string original, string substr, int a, int b)
        {
            Console.WriteLine(original);
            string newString = original.Remove(a, b - a);

            newString = newString.Insert(a, substr);

            Console.WriteLine(newString);

            return newString;
        }


        public static string jsonMakeover(string json)
        {

            json = Util.removeWS(json);


            // account for changeovers

            List<int> leftBounds = new List<int>(), rightBounds = new List<int>(); // index of left bound the first letter and index of right bounds the second letter

            int e = 1;

            bool firstLB = false;
            int lastRB = -1;
            while (e < json.Length)
            {

                if (json[e] == '[' && json[e - 1] == '[')
                {
                    if (firstLB != false)
                        json = json.Remove(e, 1);
                    else
                        firstLB = true;

                }
                else if (json[e] == ']' && json[e - 1] == ']')
                {
                    json = json.Remove(e, 1);
                    lastRB = e;

                    e--;
                }

                e++;

            }



            try
            {
                json = json.Insert(lastRB, "]");
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }



            return json;

        }

        
    }
}
