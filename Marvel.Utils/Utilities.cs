using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvel.Utils
{
    public static class Utilities
    {
        /// <summary>
        /// Splits the string using the specified seperators, and removes the delimiters.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="seperators">The seperators.</param>
        /// <param name="delimiters">The delimiters.</param>
        /// <returns>The split array of strings</returns>
        public static string[] Split(this string input, string seperators, string delimiters)
        {
            string[] splitBySeperator = input.Split(seperators.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (!string.IsNullOrEmpty(delimiters))
            {
                // Trim away the delimiters
                for (int i = 0; i < splitBySeperator.Length; i++)
                {
                    splitBySeperator[i] = splitBySeperator[i].Trim(delimiters.ToCharArray());
                }
            }

            return splitBySeperator;
        }
    }
}
