using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.NovelleDirectory
{
    public class NovellUser
    {
        public string CN { get; set; }

        public string DN { get; set; }

        public string Email { get; set; }

        public bool IsAlien
        {
            get
            {
                if (DN.Contains("o=Alien"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public string PostOfficeAddress { get; set; }

        public Dictionary<string, string[]> Attributes { get; set; }
    }
}
