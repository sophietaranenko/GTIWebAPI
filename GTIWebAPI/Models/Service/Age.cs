using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTIWebAPI.Models.Service
{
    /// <summary>
    /// Class for Age (for Russian language)
    /// </summary>
    public class Age
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="ageCount">ages in int</param>
        public Age(int? ageCount)
        {
            this.AgeCount = ageCount;
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dateOfBirth">DateTime date of birth to count ages</param>
        public Age(DateTime? dateOfBirth)
        {
            if (dateOfBirth != null)
            {
                DateTime now = DateTime.Today;
                int age = now.Year - ((DateTime)dateOfBirth).Year;
                if (now < ((DateTime)dateOfBirth).AddYears(age)) age--;
                AgeCount = age;
            }
        }
        /// <summary>
        /// field to store ages in int
        /// </summary>
        public int? AgeCount { get; set; }

        /// <summary>
        /// Ages in string "25 лет", "23 года", "21 год"
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string result = "";

            if (AgeCount != null)
            {
                result += AgeCount.ToString() + " ";

                if (AgeCount >= 5 && AgeCount <= 20)
                {
                    result += "лет";
                }
                else
                {
                    int dec = 0;
                    if (AgeCount < 100)
                    {
                        dec = ((int)AgeCount) % 10;
                    }
                    if (AgeCount > 100)
                    {
                        dec = ((int)AgeCount) % 100;
                    }

                    if (dec == 1)
                    {
                        result += "год";
                    }
                    else if (dec >= 2 && dec <= 4)
                    {
                        result += "года";
                    }
                    else
                    {
                        result += "лет";
                    }
                }
            }
            return result;
        }
    }
}