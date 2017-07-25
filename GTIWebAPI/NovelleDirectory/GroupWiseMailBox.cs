using GTIWebAPI.NovellGroupWiseSOAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.NovelleDirectory
{
    public class GroupWiseMailBox
    {
        public GroupWiseFolder Folder { get; set; }

        public List<NovellGroupWiseMail> Mails { get; set; }
    }
}
