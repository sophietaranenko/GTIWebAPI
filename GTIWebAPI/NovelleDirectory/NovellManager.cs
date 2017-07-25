using GTIWebAPI.Models.Account;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;
using System.DirectoryServices.Protocols;
using System.DirectoryServices;
using System.Net;
using GTIWebAPI.Exceptions;

namespace GTIWebAPI.NovelleDirectory
{
    public class NovelleDirectory : INovelleDirectory
    {
        private StringCollection ServerAddresses;

        public NovelleDirectory()
        {
            ServerAddresses = Properties.Settings.Default.NovellIPAddresses;
        }

        public NovellUser Connect(string login, string password)
        {
            bool res = false;
            NovellUser user = new NovellUser();

            foreach (var serverAddress in ServerAddresses)
            {
                try
                {
                    using (NovellProvider novell = new NovellProvider(serverAddress))
                    {
                        res = novell.CheckWorldPassword(login, password);
                        if (res == false)
                        {
                            res = novell.CheckAlienPassword(login, password);
                            if (res == false)
                            {
                                break;
                            }
                        }
                        if (res == true)
                        {
                            novell.Bind();
                            user = novell.Find(login);
                            break;
                        }
                    }
                    
                }
                catch (Exception e)
                {
                    //when ip address doesn't response - there will be exception 
                    continue;
                    //throw new NovelleDirectoryException("Cannot connect to Novell eDirectory");
                }
            }
            if (res == false)
            {
                throw new NovelleDirectoryException("Wrong login or password");
            }
            if (user.CN == null || user.CN == "")
            {
                throw new NovelleDirectoryException("User is not found");
            }
            return user;
        }

        public INovellOrganizationContactPerson CreateOrganization(INovellOrganizationContactPerson person)
        {
            string DN = "";
            foreach (string serverAddress in ServerAddresses)
            {
                try
                {
                    using (NovellProvider novell = new NovellProvider(serverAddress))
                    {
                        novell.Bind();
                        person.Login = novell.GenerateUniqueAlienCN(person.Login);
                        bool entryExist = novell.EntryExists(person.Login);
                        if (!entryExist)
                        {
                            DN = novell.CreateEntry(person);
                        }
                        if (DN != "")
                        {
                            break;
                        }
                    }
                }
                catch (Exception e)
                {
                    continue;
                }
            }
            return person;
        }





        protected class NovellProvider : IDisposable
        {
            public static string ServerAddress;

            public static LdapConnection Connection;

            public NovellProvider(string serverAddress)
            {
                ServerAddress = serverAddress;
            }

            public void Bind()
            {
                Connection = LDAPCommon.Connect(ServerAddress + ":636");
                LdapSessionOptions options = Connection.SessionOptions;
                options.ProtocolVersion = 3;
                options.SecureSocketLayer = true;

                Connection.AuthType = AuthType.Basic;
                NetworkCredential credential =
                        new NetworkCredential("cn=gtildap,ou=odessa,o=world", "wemayont");
                Connection.Credential = credential;

                try
                {
                    Connection.Bind();

                    if (options.SecureSocketLayer == true)
                    {
                        Console.WriteLine("SSL for encryption is enabled\nSSL information:\n" +
                        "\tcipher strength: {0}\n" +
                        "\texchange strength: {1}\n" +
                        "\tprotocol: {2}\n" +
                        "\thash strength: {3}\n" +
                        "\talgorithm: {4}\n",
                        options.SslInformation.CipherStrength,
                        options.SslInformation.ExchangeStrength,
                        options.SslInformation.Protocol,
                        options.SslInformation.HashStrength,
                        options.SslInformation.AlgorithmIdentifier);
                    }

                }
                catch (LdapException e)
                {
                    Console.WriteLine("\nCredential validation for User " +
                        "account {0} using ssl failed\n" +
                        "LdapException: {1}", "gtildap", e.Message);
                }
                catch (DirectoryOperationException e)
                {
                    Console.WriteLine("\nCredential validation for User " +
                    "account {0} using ssl failed\n" +
                    "DirectoryOperationException: {1}", "gtildap", e.Message);
                }
            }

            public NovellUser Find(string username)
            {
                NovellUser user = new NovellUser();
                user.Attributes = new Dictionary<string, string[]>();
                try
                {
                    string ldapSearchFilter = "cn=" + username.Trim();
                    SearchRequest searchRequest = new SearchRequest
                                                    ("",
                                                      ldapSearchFilter,
                                                      System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                      null);
                    SearchResponse searchResponse =
                                (SearchResponse)Connection.SendRequest(searchRequest);
                    Console.WriteLine("\r\nSearch Response Entries:{0}",
                                searchResponse.Entries.Count);
                    if (searchResponse.Entries.Count > 0)
                    {
                        SearchResultEntry entry = searchResponse.Entries[0];
                        user.CN = username;
                        user.DN = entry.DistinguishedName;
                        foreach (DirectoryAttribute attribute in entry.Attributes.Values)
                        {
                            List<string> values = new List<string>();
                            for (int i = 0; i < attribute.Count; i++)
                            {
                               values.Add(attribute[i].ToString());
                            }
                            user.Attributes.Add(attribute.Name, values.ToArray());
                        }
                    }
                    else
                    {
                        
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("\nUnexpected exception occured:\n\t{0}: {1}",
                                      e.GetType().Name, e.Message);
                }
                return user;
            }

            public string GenerateUniqueAlienCN(string commonName)
            {
                bool cnExist = true;

                if (EntryExists(commonName))
                {
                    for (int i = 1; ; i++)
                    {
                        cnExist = EntryExists(commonName + i.ToString().Trim());
                        if (cnExist == false)
                        {
                            commonName = commonName + i.ToString().Trim();
                            break;
                        }
                    }
                }
                return commonName;
            }

            public bool CheckAlienPassword(string login, string password)
            {
                return CheckPassword(login, password, "Alien");
            }

            public bool CheckWorldPassword(string login, string password)
            {
                return CheckPassword(login, password, "World");
            }

            public bool EntryExists(string login)
            {
                bool exits = true;
                try
                {
                    string ldapSearchFilter = "cn=" + login.Trim();
                    SearchRequest searchRequest = new SearchRequest
                                                    ("",
                                                      ldapSearchFilter,
                                                      System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                      null);
                    SearchResponse searchResponse =
                                (SearchResponse)Connection.SendRequest(searchRequest);
                    Console.WriteLine("\r\nSearch Response Entries:{0}",
                                searchResponse.Entries.Count);
                    if (searchResponse.Entries.Count > 0)
                    {
                        exits = true;
                    }
                    else
                    {
                        exits = false;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("\nUnexpected exception occured:\n\t{0}: {1}",
                                      e.GetType().Name, e.Message);
                    exits = false;
                }
                return exits;
            }

            public string CreateEntry(INovellOrganizationContactPerson person)
            {
                string DN = "";
                string containerName = "ou=users,o=alien";
                try
                {
                    string dn = "cn=" + person.Login + "," + containerName;
                    string dirClassType = "inetOrgPerson";
                    AddRequest addRequest = new AddRequest(dn, dirClassType);
                    addRequest.Attributes.Add(new DirectoryAttribute("cn", new string[] { person.Login }));
                    addRequest.Attributes.Add(new DirectoryAttribute("givenname", person.FirstName));
                    addRequest.Attributes.Add(new DirectoryAttribute("sn", person.Surname));
                    addRequest.Attributes.Add(new DirectoryAttribute("mail", person.Email));
                    addRequest.Attributes.Add(new DirectoryAttribute("userpassword", person.Password));
                    AddResponse addResponse = (AddResponse)Connection.SendRequest(addRequest);
                    DN = dn;
                }
                catch (Exception e)
                {
                    Console.WriteLine("\nUnexpected exception occured:\n\t{0}: {1}",
                                      e.GetType().Name, e.Message);
                }
                return DN;
            }

            private bool CheckPassword(string login, string password, string basedn)
            {
                bool correct = false;

                string ldapserver = ServerAddress.ToString() + ":636";
                string ldapbasedn = "o=" + basedn;
                string ldapuser = "cn=gtildap,ou=odessa,o=world";
                string ldappassword = "wemayont";
                string ldapfilter = "(&(cn={0}))";
                try
                {
                    string DN = "";
                    using (DirectoryEntry entry = new DirectoryEntry("LDAP://" + ldapserver + "/" + ldapbasedn, ldapuser, ldappassword, AuthenticationTypes.None))
                    {
                        DirectorySearcher ds = new DirectorySearcher(entry);
                        ds.SearchScope = System.DirectoryServices.SearchScope.Subtree;
                        ds.Filter = string.Format(ldapfilter, login);
                        SearchResult result = ds.FindOne();
                        if (result != null)
                        {
                            DN = result.Path.Replace("LDAP://" + ldapserver + "/", "");
                        }
                    }
                    if (DN != null && DN != "")
                    {
                        using (DirectoryEntry entry = new DirectoryEntry("LDAP://" + ldapserver + "/" + ldapbasedn, DN, password, AuthenticationTypes.None))
                        {
                            DirectorySearcher ds = new DirectorySearcher(entry);
                            ds.SearchScope = System.DirectoryServices.SearchScope.Subtree;
                            SearchResult result = ds.FindOne();
                            if (result != null)
                            {
                                correct = true;
                            }
                        }
                    }
                    else
                    {
                        correct = false;
                    }
                }
                catch (Exception e)
                {
                    correct = false;
                }
                return correct;
            }

            public void Dispose()
            {
                if (Connection != null)
                { 
                    Connection.Dispose();
                }
            }
        }
    }
}
