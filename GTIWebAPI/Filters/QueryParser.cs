using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Filters
{
    public static class QueryParser
    {

        /// <summary>
        /// method that parse uri query to list of int parameters
        /// </summary>
        /// <param name="parameterName">parameter name</param>
        /// <param name="query">all query text</param>
        /// <param name="symbol">by what symbol integers are separated</param>
        /// <returns></returns>
        public static List<int> Parse(string parameterName, string query, char symbol)
        {
            List<int> listOfIds = new List<int>();
            

            if (parameterName != null && query != null)
            {
                query = query.ToUpper() + "&";
                parameterName = parameterName.ToUpper();

                string whatToCut = "?" + parameterName + "=";
                int beginIndex = whatToCut.Length;

                query = query.Substring(beginIndex);

                string[] idsString = query.Split(symbol);

                foreach (string value in idsString)
                {
                    int current = 0;
                    bool result = Int32.TryParse(value, out current);
                    if (result == true)
                    {
                        listOfIds.Add(current);
                    }
                }

            }
            return listOfIds;
        }
    }
}
