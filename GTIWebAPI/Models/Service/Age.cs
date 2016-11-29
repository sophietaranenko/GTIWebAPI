using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTIWebAPI.Models.Service
{
    public class Age
    {
        public Age(int? ageCount)
        {
            this.AgeCount = ageCount;
        }
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

        public int? AgeCount { get; set; }
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