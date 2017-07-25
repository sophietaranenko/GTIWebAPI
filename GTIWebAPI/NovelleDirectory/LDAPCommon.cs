using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.NovelleDirectory
{
    public class LDAPCommon
    {
        static LdapConnection connection;

        // for connecting using the current context
        public static LdapConnection Connect(string ldapServerorDomainName)
        {
            // to use LDAP calls, you must first connect to the directory.
            try
            {
                // create an LDAP connection to the server
                connection = new LdapConnection(ldapServerorDomainName);

                // Autobind is true by default. However, specifying
                // it exclusively emphasizes that binding to the directory
                // is required. In this case bind is completed implicitly.
                connection.AutoBind = true;

                // this is the default for connecting to Windows Server 2003
                // coding this explicitly for clarity
                connection.AuthType = AuthType.Negotiate;

                Console.WriteLine("LDAP connection established");
                return connection;

            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\r\n\t" +
                                  e.GetType().Name + ":" + e.Message);

                return null;

            }

        }

        // for connecting using a different user name, password and domain name
        // while not required, you should always pass encrypted credentials
        // over the wire. This example uses a secure connection
        public static LdapConnection Connect(string ldapServerName,
            string userName, string password, string domainName)
        {
            #region //LDAP connection section
            //to use LDAP calls, you must first connect to the directory.
            try
            {
                //create an LDAP connection to the server
                connection = new LdapConnection(ldapServerName);
                connection.Credential = new System.Net.NetworkCredential(userName, password, domainName);
                Console.WriteLine("LDAP connection established");
                return connection;

            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\r\n\t" +
                                  e.GetType().Name + ":" + e.Message);

                return null;
            }
            #endregion //end LDAP connection section
        }

        // helper method to convert a byte array to a hex string.
        // used by one of the AttributeSearch methods.
        public static string ToHexString(byte[] bytes)
        {
            char[] hexDigits = {
                '0', '1', '2', '3', '4', '5', '6', '7',
                '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};

            char[] chars = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                int b = bytes[i];
                chars[i * 2] = hexDigits[b >> 4];
                chars[i * 2 + 1] = hexDigits[b & 0xF];
            }
            return new string(chars);
        }

    }
}
