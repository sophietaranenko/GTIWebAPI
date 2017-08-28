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
//using Novell.Directory.Ldap;
//using Novell.Directory.Ldap.Utilclass;
//using Mono.Security.X509;

using System;
//using Syscert = System.Security.Cryptography.X509Certificates;
//using Mono.Security.X509;
//using Mono.Security.Cryptography;


namespace GTIWebAPI.NovelleDirectory
{
    public class NovelleDirectory : INovelleDirectory
    {
        private string message = "";
        private StringCollection ServerAddresses;

        public NovelleDirectory()
        {
            ServerAddresses = Properties.Settings.Default.NovellIPAddresses;
        }

        public string getMessage()
        {
            return message;
        }






        public NovellUser Connect(string login, string password)
        {
            bool res = false;
            NovellUser user = new NovellUser();

            foreach (var serverAddress in ServerAddresses)
            {
                message += serverAddress;
                try
                {
                    using (NovellProvider novell = new NovellProvider(serverAddress, 636))
                    {
                        novell.Connect();
                        novell.Bind(Properties.Settings.Default.NovellDN, Properties.Settings.Default.NovellPassword);
                        user = novell.Search(login);
                        res = novell.VerifyPassword(user.DN, password);
                        break;
                        //res = novell.CheckWorldPassword(login, password);

                        //if (res == false)
                        //{
                        //    res = novell.CheckAlienPassword(login, password);

                        //    if (res == false)
                        //    {
                        //        break;
                        //    }
                        //}
                        //if (res == true)
                        //{
                        //    novell.Bind();
                        //    user = novell.Find(login);
                        //    break;
                        //}


                        //novell.Connect()


                    }


                }
                catch (Exception e)
                {
                    //  message += " exc in novell provider, continue "; 
                    //when ip address doesn't response - there will be exception 
                    //continue;
                    //throw new NovelleDirectoryException("Cannot connect to Novell eDirectory");
                }
            }
            if (res == false)
            {
                throw new NovelleDirectoryException(message);

            }
            if (user.CN == null || user.CN == "")
            {
                throw new NovelleDirectoryException(message);
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
                    using (NovellProvider novell = new NovellProvider(serverAddress, 636))
                    {
                        novell.Connect();
                        novell.Bind(Properties.Settings.Default.NovellDN, Properties.Settings.Default.NovellPassword);
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
            private string message = "";

            public static string ServerAddress;

            public static int ServerPort { get; set; }

            public static LdapConnection connection;

           // protected static bool bHowToProceed, quit = false, removeFlag = false;

           // protected static int bindCount = 0;

            public void Connect()
            {
                //bHowToProceed = true;
                //String continueBind;
                //bindCount++;
                //Connection = new LdapConnection();
                //Connection.SecureSocketLayer = true;
                //Connection.UserDefinedServerCertValidationDelegate += new
                //    CertificateValidationCallback(MySSLHandler);
                //Connection.Connect(ServerAddress, ServerPort);

            }

            //public static bool MySSLHandler(Syscert.X509Certificate certificate,
            //                int[] certificateErrors)
            //{

            //    Mono.Security.X509.X509Store store = null;
            //    X509Stores stores = X509StoreManager.CurrentUser;
            //    String input;
            //    store = stores.TrustedRoot;


            //    //Import the details of the certificate from the server.

            //    Mono.Security.X509.X509Certificate x509 = null;
            //    Mono.Security.X509.X509CertificateCollection coll = new Mono.Security.X509.X509CertificateCollection();
            //    byte[] data = certificate.GetRawCertData();
            //    if (data != null)
            //        x509 = new Mono.Security.X509.X509Certificate(data);

            //    //List the details of the Server

            //    //check for ceritficate in store
            //    Mono.Security.X509.X509CertificateCollection check = store.Certificates;
            //    if (!check.Contains(x509))
            //    {
            //        //if (bindCount == 1)
            //        //{
            //        //    Console.WriteLine(" \n\nCERTIFICATE DETAILS: \n");
            //        //    Console.WriteLine(" {0}X.509 v{1} Certificate", (x509.IsSelfSigned ? "Self-signed " : String.Empty), x509.Version);
            //        //    Console.WriteLine("  Serial Number: {0}", CryptoConvert.ToHex(x509.SerialNumber));
            //        //    Console.WriteLine("  Issuer Name:   {0}", x509.IssuerName);
            //        //    Console.WriteLine("  Subject Name:  {0}", x509.SubjectName);
            //        //    Console.WriteLine("  Valid From:    {0}", x509.ValidFrom);
            //        //    Console.WriteLine("  Valid Until:   {0}", x509.ValidUntil);
            //        //    Console.WriteLine("  Unique Hash:   {0}", CryptoConvert.ToHex(x509.Hash));
            //        //    Console.WriteLine();
            //        //}

            //        //Get the response from the Client
            //        //  do
            //        //  {
            //        // Console.WriteLine("\nDo you want to proceed with the connection (y/n)?");
            //        //   input = Console.ReadLine();
            //        //  if (input == "y" || input == "Y")
            //        bHowToProceed = true;
            //        //   if (input == "n" || input == "N")
            //        //        bHowToProceed = false;
            //        // } while (input != "y" && input != "Y" && input != "n" && input != "N");
            //    }
            //    else
            //    {
            //        if (bHowToProceed == true)
            //        {
            //            //Add the certificate to the store.

            //            if (x509 != null)
            //                coll.Add(x509);
            //            store.Import(x509);
            //            if (bindCount == 1)
            //                removeFlag = true;
            //        }
            //    }
            //    if (bHowToProceed == false)
            //    {
            //        //Remove the certificate added from the store.

            //        if (removeFlag == true && bindCount > 1)
            //        {
            //            foreach (Mono.Security.X509.X509Certificate xt509 in store.Certificates)
            //            {
            //                if (CryptoConvert.ToHex(xt509.Hash) == CryptoConvert.ToHex(x509.Hash))
            //                {
            //                    store.Remove(x509);
            //                }
            //            }
            //        }
            //        // Console.WriteLine("SSL Bind Failed.");
            //    }
            //    return bHowToProceed;
            //}



            public NovellProvider(string serverAddress, int port)
            {
                //Connection = new LdapConnection();
                //Connection.SecureSocketLayer = true;
                //ServerAddress = serverAddress;
                //ServerPort = port;
                connection = new LdapConnection(new LdapDirectoryIdentifier(serverAddress, port));
                connection.SessionOptions.VerifyServerCertificate = new VerifyServerCertificateCallback((con, cer) => true);
                connection.SessionOptions.ProtocolVersion = 3;
                connection.AuthType = AuthType.Basic;
                connection.SessionOptions.SecureSocketLayer = true;
            }

            public string GetMessage()
            {
                return message;
            }

            public void Bind(string loginDN, string password)
            {
                //Connection.Bind(loginDN, password);
                connection.Bind(new System.Net.NetworkCredential(loginDN, password));
            }


            public NovellUser Search(string login)
            {
                //NovellUser user = new NovellUser();
                //LdapSearchResults lsc = Connection.Search(searchBase,
                //                                    LdapConnection.SCOPE_SUB,
                //                                    searchFilter,
                //                                    null,
                //                                    false);
                //while (lsc.hasMore())
                //{
                //    LdapEntry nextEntry = null;
                //    try
                //    {
                //        nextEntry = lsc.next();
                //    }
                //    catch (LdapException e)
                //    {
                //        Console.WriteLine("Error: " + e.LdapErrorMessage);
                //        // Exception is thrown, go for next entry
                //        continue;
                //    }
                //    user.DN = nextEntry.DN;
                //    user.CN = nextEntry.getAttribute("cn").StringValue;

                //    user.Attributes = new Dictionary<string, string[]>();

                //    LdapAttributeSet attributeSet = nextEntry.getAttributeSet();
                //    System.Collections.IEnumerator ienum = attributeSet.GetEnumerator();
                //    while (ienum.MoveNext())
                //    {
                //        LdapAttribute attribute = (LdapAttribute)ienum.Current;
                //        string attributeName = attribute.Name;
                //        string attributeVal = attribute.StringValue;
                //        if (!Base64.isLDIFSafe(attributeVal))
                //        {
                //            byte[] tbyte = SupportClass.ToByteArray(attributeVal);
                //            attributeVal = Base64.encode(SupportClass.ToSByteArray(tbyte));
                //        }
                //        user.Attributes.Add(attributeName, new string[] { attributeVal });
                //    }
                //}



                //return user;


                //search base должен быть заполнен
                //как сделать ИЛИ или И (o=world && o=alien) не знаю

                NovellUser user = new NovellUser();
                user.Attributes = new Dictionary<string, string[]>();
                SearchRequest r = new SearchRequest(
                    "o=world",
                    "cn="+login,
                    System.DirectoryServices.Protocols.SearchScope.Subtree,
                    "*");
                SearchResponse re = (SearchResponse)connection.SendRequest(r);

                if (re.Entries.Count == 0)
                {
                    r = new SearchRequest(
                    "o=alien",
                    "cn=" + login,
                    System.DirectoryServices.Protocols.SearchScope.Subtree,
                    "*");

                    re = (SearchResponse)connection.SendRequest(r);
                }
                user.DN = re.Entries[0].DistinguishedName;
                foreach (byte[] item in re.Entries[0].Attributes["cn"])
                {
                    user.CN = System.Text.Encoding.Default.GetString(item);
                }

                foreach (SearchResultEntry i in re.Entries)
                {
                    foreach (string attrName in i.Attributes.AttributeNames)
                    {
                        List<String> attrs = new List<string>();
                        foreach (byte[] item in re.Entries[0].Attributes["cn"])
                        {
                            attrs.Add(System.Text.Encoding.Default.GetString(item));
                        }
                        user.Attributes.Add(attrName, attrs.ToArray());
                    }
                }

                
                //while (lsc.hasMore())
                //{
                //    LdapEntry nextEntry = null;
                //    try
                //    {
                //        nextEntry = lsc.next();
                //    }
                //    catch (LdapException e)
                //    {
                //        Console.WriteLine("Error: " + e.LdapErrorMessage);
                //        // Exception is thrown, go for next entry
                //        continue;
                //    }
                //    user.DN = nextEntry.DN;
                //    user.CN = nextEntry.getAttribute("cn").StringValue;

                //    user.Attributes = new Dictionary<string, string[]>();

                //    LdapAttributeSet attributeSet = nextEntry.getAttributeSet();
                //    System.Collections.IEnumerator ienum = attributeSet.GetEnumerator();
                //    while (ienum.MoveNext())
                //    {
                //        LdapAttribute attribute = (LdapAttribute)ienum.Current;
                //        string attributeName = attribute.Name;
                //        string attributeVal = attribute.StringValue;
                //        if (!Base64.isLDIFSafe(attributeVal))
                //        {
                //            byte[] tbyte = SupportClass.ToByteArray(attributeVal);
                //            attributeVal = Base64.encode(SupportClass.ToSByteArray(tbyte));
                //        }
                //        user.Attributes.Add(attributeName, new string[] { attributeVal });
                //    }
                //}
                return user;


            }



            //public void Bind()
            //{
            //Connection = LDAPCommon.Connect(ServerAddress + ":636");
            //LdapSessionOptions options = Connection.SessionOptions;
            //options.ProtocolVersion = 3;
            //options.SecureSocketLayer = true;

            //Connection.AuthType = AuthType.Basic;
            //NetworkCredential credential =
            //        new NetworkCredential("cn=gtildap,ou=odessa,o=world", "wemayont");
            //Connection.Credential = credential;

            //try
            //{
            //    Connection.Bind();
            //    message += "binded";
            //    if (options.SecureSocketLayer == true)
            //    {
            //        message += String.Format("SSL for encryption is enabled\nSSL information:\n" +
            //        "\tcipher strength: {0}\n" +
            //        "\texchange strength: {1}\n" +
            //        "\tprotocol: {2}\n" +
            //        "\thash strength: {3}\n" +
            //        "\talgorithm: {4}\n",
            //        options.SslInformation.CipherStrength,
            //        options.SslInformation.ExchangeStrength,
            //        options.SslInformation.Protocol,
            //        options.SslInformation.HashStrength,
            //        options.SslInformation.AlgorithmIdentifier);
            //    }

            //}
            //catch (LdapException e)
            //{
            //    Console.WriteLine("\nCredential validation for User " +
            //        "account {0} using ssl failed\n" +
            //        "LdapException: {1}", "gtildap", e.Message);
            //}
            //catch (DirectoryOperationException e)
            //{
            //    Console.WriteLine("\nCredential validation for User " +
            //    "account {0} using ssl failed\n" +
            //    "DirectoryOperationException: {1}", "gtildap", e.Message);
            //}
            //catch (Exception e)
            //{
            //    //do something 
            //    string me = e.Message;
            //}
            //}

            //public NovellUser Find(string username)
            //{
            //    message += "finding an username "; 
            //    NovellUser user = new NovellUser();
            //    user.Attributes = new Dictionary<string, string[]>();
            //    try
            //    {
            //        string ldapSearchFilter = "cn=" + username.Trim();
            //        SearchRequest searchRequest = new SearchRequest
            //                                        ("",
            //                                          ldapSearchFilter,
            //                                          System.DirectoryServices.Protocols.SearchScope.Subtree,
            //                                          null);
            //        SearchResponse searchResponse =
            //                    (SearchResponse)Connection.SendRequest(searchRequest);
            //        Console.WriteLine("\r\nSearch Response Entries:{0}",
            //                    searchResponse.Entries.Count);
            //        if (searchResponse.Entries.Count > 0)
            //        {
            //            SearchResultEntry entry = searchResponse.Entries[0];
            //            user.CN = username;
            //            user.DN = entry.DistinguishedName;
            //            foreach (DirectoryAttribute attribute in entry.Attributes.Values)
            //            {
            //                List<string> values = new List<string>();
            //                for (int i = 0; i < attribute.Count; i++)
            //                {
            //                   values.Add(attribute[i].ToString());
            //                }
            //                user.Attributes.Add(attribute.Name, values.ToArray());
            //            }
            //        }
            //        else
            //        {

            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine("\nUnexpected exception occured:\n\t{0}: {1}",
            //                          e.GetType().Name, e.Message);
            //    }
            //    return user;
            //}

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

            public bool VerifyPassword(string DN, string password)
            {
                try
                {
                    connection.Bind(new System.Net.NetworkCredential(DN, password));
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
                //LdapAttribute attr = new LdapAttribute("userPassword", password);
                //bool correct = Connection.Compare(DN, attr);
                //return correct;
            }

            //public bool CheckAlienPassword(string login, string password)
            //{
            //    return CheckPassword(login, password, "Alien");
            //}

            //public bool CheckWorldPassword(string login, string password)
            //{
            //    return CheckPassword(login, password, "World");
            //}


            public bool EntryExists(string login)
            {
                //bool exists = false;
                //LdapSearchResults lsc = Connection.Search("",
                //                                    LdapConnection.SCOPE_SUB,
                //                                    "cn=" + login,
                //                                    null,
                //                                    false);
                //while (lsc.hasMore())
                //{
                //    LdapEntry nextEntry = null;
                //    try
                //    {
                //        nextEntry = lsc.next();
                //    }
                //    catch (LdapException e)
                //    {
                //        Console.WriteLine("Error: " + e.LdapErrorMessage);
                //        continue;
                //    }
                //    if (nextEntry != null)
                //    {
                //        exists = true;
                //    }
                //    else
                //    {
                //        exists = false;
                //    }
                //}
                //return exists;
                bool exists = false;
                SearchRequest r = new SearchRequest(
                    "o=alien",
                    "(cn="+login+")",
                    System.DirectoryServices.Protocols.SearchScope.Subtree,
                    "*");
                SearchResponse re = (SearchResponse)connection.SendRequest(r);
                exists = re.Entries.Count > 0 ? true : false; 
                return exists;

            }


            //public bool EntryExists(string login)
            //{
            //    bool exits = true;
            //    try
            //    {
            //        string ldapSearchFilter = "cn=" + login.Trim();
            //        message += ldapSearchFilter; 
            //        SearchRequest searchRequest = new SearchRequest
            //                                        ("",
            //                                          ldapSearchFilter,
            //                                          System.DirectoryServices.Protocols.SearchScope.Subtree,
            //                                          null);
            //        SearchResponse searchResponse =
            //                    (SearchResponse)Connection.SendRequest(searchRequest);

            //        message += String.Format("\r\nSearch Response Entries:{0}",
            //                    searchResponse.Entries.Count);
            //        if (searchResponse.Entries.Count > 0)
            //        {
            //            exits = true;
            //        }
            //        else
            //        {
            //            exits = false;
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        message += String.Format("\nUnexpected exception occured:\n\t{0}: {1}",
            //                          e.GetType().Name, e.Message);
            //        exits = false;
            //    }
            //    return exits;
            //}



            public string CreateEntry(INovellOrganizationContactPerson person)
            {
               // LdapAttributeSet attributeSet = new LdapAttributeSet();
               // attributeSet.Add(new LdapAttribute(
               //                     "objectclass", "inetOrgPerson"));
               ///attributeSet.Add(new LdapAttribute("cn",
               // new string[] { person.Login }));

                //attributeSet.Add(new LdapAttribute("givenname",
                //                     person.FirstName));
                //attributeSet.Add(new LdapAttribute("sn", person.Surname));
                //attributeSet.Add(new LdapAttribute("mail", person.Email));
                //attributeSet.Add(new LdapAttribute("userpassword", person.Password));
                 // string dn = "cn=" + person.Login + ",ou=Users,o=Alien";
                //LdapEntry newEntry = new LdapEntry(dn, attributeSet);
                //Connection.Add(newEntry);
                //return newEntry.DN;


                string containerName = "ou=Users,o=Alien";
                // string login = "new_user_created_by_MS_DirectoryServices_5";

                string dn = "cn=" +  person.Login + "," + containerName;
                string dirClassType = "inetOrgPerson";
                AddRequest addRequest = new AddRequest(dn, dirClassType);
                addRequest.Attributes.Add(new DirectoryAttribute("cn", new string[] { person.Login }));
                addRequest.Attributes.Add(new DirectoryAttribute("givenname", person.FirstName));
                addRequest.Attributes.Add(new DirectoryAttribute("sn", person.Surname));
                addRequest.Attributes.Add(new DirectoryAttribute("mail", person.Email));
                addRequest.Attributes.Add(new DirectoryAttribute("userpassword", person.Password));
                AddResponse addResponse = (AddResponse)connection.SendRequest(addRequest);
                return dn;

            }



            //public string CreateEntry(INovellOrganizationContactPerson person)
            //{
            //    string DN = "";
            //    string containerName = "ou=users,o=alien";
            //    try
            //    {
            //        string dn = "cn=" + person.Login + "," + containerName;
            //        string dirClassType = "inetOrgPerson";
            //        AddRequest addRequest = new AddRequest(dn, dirClassType);
            //        addRequest.Attributes.Add(new DirectoryAttribute("cn", new string[] { person.Login }));
            //        addRequest.Attributes.Add(new DirectoryAttribute("givenname", person.FirstName));
            //        addRequest.Attributes.Add(new DirectoryAttribute("sn", person.Surname));
            //        addRequest.Attributes.Add(new DirectoryAttribute("mail", person.Email));
            //        addRequest.Attributes.Add(new DirectoryAttribute("userpassword", person.Password));
            //        AddResponse addResponse = (AddResponse)Connection.SendRequest(addRequest);
            //        DN = dn;
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine("\nUnexpected exception occured:\n\t{0}: {1}",
            //                          e.GetType().Name, e.Message);
            //    }
            //    return DN;
            //}

            //private bool CheckPassword(string login, string password, string basedn)
            //{
            //    bool correct = false;

            //    string ldapserver = ServerAddress.ToString() + ":636";
            //    string ldapbasedn = "o=" + basedn;
            //    string ldapuser = "cn=gtildap,ou=odessa,o=world";
            //    string ldappassword = "wemayont";
            //    string ldapfilter = "(&(cn={0}))";
            //    try
            //    {
            //        string DN = "";
            //        using (DirectoryEntry entry = new DirectoryEntry("LDAP://" + ldapserver + "/" + ldapbasedn, ldapuser, ldappassword, AuthenticationTypes.None))
            //        {
            //            DirectorySearcher ds = new DirectorySearcher(entry);
            //            ds.SearchScope = System.DirectoryServices.SearchScope.Subtree;
            //            ds.Filter = string.Format(ldapfilter, login);
            //            SearchResult result = ds.FindOne();
            //            if (result != null)
            //            {
            //                DN = result.Path.Replace("LDAP://" + ldapserver + "/", "");
            //            }
            //        }
            //        if (DN != null && DN != "")
            //        {
            //            using (DirectoryEntry entry = new DirectoryEntry("LDAP://" + ldapserver + "/" + ldapbasedn, DN, password, AuthenticationTypes.None))
            //            {
            //                DirectorySearcher ds = new DirectorySearcher(entry);
            //                ds.SearchScope = System.DirectoryServices.SearchScope.Subtree;
            //                SearchResult result = ds.FindOne();
            //                if (result != null)
            //                {
            //                    correct = true;
            //                }
            //            }
            //        }
            //        else
            //        {
            //            correct = false;
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        correct = false;
            //    }
            //    return correct;
            //}

            public void Dispose()
            {
                if (connection != null)
                {
                    connection.Dispose();
                }
            }
        }
    }
}
