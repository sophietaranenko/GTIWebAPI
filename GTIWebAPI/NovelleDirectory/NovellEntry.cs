using Novell.Directory.Ldap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTIWebAPI.NovelleDirectory
{
    public class NovellEntry
    {
        private LdapEntry Entry;

        public NovellEntry(LdapEntry entry)
        {
            Entry = entry;
        }

        public string Email
        {
            get
            {
                string result = "";

                var attribute = Entry.getAttribute("mail");
                if (attribute != null)
                {
                    result = attribute.StringValueArray[0];
                }
                return result;
            }
        }

        public string CommonName
        {
            get
            {
                string result = "";

                var attribute = Entry.getAttribute("cn");
                if (attribute != null)
                {
                    result = attribute.StringValueArray[0];
                }
                return result;
            }
        }



        public string DN
        {
            get
            {
                return Entry.DN;
            }
        }
    }
}