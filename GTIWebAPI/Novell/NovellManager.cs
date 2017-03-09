﻿using GTIWebAPI.Models.Account;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Novell.Directory.Ldap;
using System.Security.Cryptography.X509Certificates;
using Mono.Security;
using Mono.Security.Cryptography;
using System.Runtime.CompilerServices;

namespace GTIWebAPI.Novell
{
    public static class NovellManager
    {
        //Миша писал в скайп 17.02 
        //сейчас только 0.9 работает в режиме non-TLS. Остальные требуют TLS для подключения к ДВФЗ-серверу. 
        //Вообще я бы хотел для авторизации и всех операций с LDAP использовать в порядке очередности: 0.20, 1.1, 0.6, 0.9.
        private static string[] ServerAddresses = { "192.168.0.20", "192.168.1.1", "192.168.0.6", "192.168.0.9" };


        public static string GenerateLogin(string login)
        {
            return GenerateNovellLogin(login);
        }

        public static bool CredentialsCorrect(string username, string password)
        {
            bool result = VerifyPassword(username, password);
            return result;
        }

        public static bool CreateOrganization(NovellOrganizationContactPerson person)
        {
            bool result = CreateNovellOrganization(person);
            return result;
        }

        public static string FindEmail(string username)
        {
            return FindEmailByCN(username);
        }

        private static string FindEmailByCN(string username)
        {
            string email = "";

            //connect to Novell with one of server addresses 
            foreach (string serverAddress in ServerAddresses)
            {
                try
                {
                    using (NovellProvider novell = new NovellProvider(serverAddress))
                    {
                        //bind with gtildap
                        novell.Bind();
                        email = novell.FindEmail(username);
                    }
                }
                catch (Exception e)
                {
                    continue;
                }

            }
            //if Connect Error happened, restart with another server address 
            return email;
        }

        private static string GenerateNovellLogin(string login)
        {
            string result = "";

            //no matter if such email exist 
            //no matter if such login exist 

            //we generate new login for user... 

            foreach (string serverAddress in ServerAddresses)
            {
                try
                {
                    using (NovellProvider novell = new NovellProvider(serverAddress))
                    {
                        //bind with gtildap
                        novell.Bind();
                        result = novell.GenerateLogin(login);
                    }
                }
                catch (Exception e)
                {
                    continue;
                }
            }

            return result;
        }
        
        //noone knows server address
        //it'll be better if we change location to web.config 

        private static bool VerifyPassword(string login, string password)
        {
            bool result = false;

            //connect to Novell with one of server addresses 
            foreach (string serverAddress in ServerAddresses)
            {
                try
                {
                    using (NovellProvider novell = new NovellProvider(serverAddress))
                    {
                        //bind with gtildap
                        novell.Bind();
                        //search if LdapEntry with such email exists 
                        bool entryExist = novell.EntryExists(login);
                        if (entryExist)
                        {
                            //verify password
                            result = novell.VerifyPassword(login, password);
                            break;
                        }
                    }
                }
                catch (Exception e)
                {
                    continue;
                }

            }
            //if Connect Error happened, restart with another server address 
            return result;
        }

        private static bool CreateNovellOrganization(NovellOrganizationContactPerson person)
        {
            bool result = false;

            //connect to Novell with one of server addresses 
            foreach (string serverAddress in ServerAddresses)
            {
                try
                {
                    using (NovellProvider novell = new NovellProvider(serverAddress))
                    {
                        //bind with gtildap
                        novell.Bind();
                        //search if LdapEntry with such email exists 
                        bool entryExist = novell.EntryExists(person.Login);
                        if (!entryExist)
                        {
                            //create new entry if we can 
                            result = novell.CreateEntry(person);
                        }
                        break;
                    }
                }
                catch (Exception e)
                {
                    continue;
                }
            }

            return result;
        }

        protected class NovellProvider : IDisposable
        {
            private string res = "";
            private bool bHowToProceed = false;
            private bool removeFlag = false;
            private int bindCount = 0;

            private LdapConnection ldapConn;

            public NovellProvider(string serverAddress)
            {
                ldapConn = new LdapConnection();
                ldapConn.SecureSocketLayer = true;
                bHowToProceed = true;
                ldapConn.UserDefinedServerCertValidationDelegate += new CertificateValidationCallback(MySSLHandler);
                bindCount = 0;
                ldapConn.Connect(serverAddress, 636);
            }

            //public void Bind(string LdapServer = "192.168.0.1", string DN = "cn=gtildap,ou=Tech,ou=ALL,o=World", string Password = "wemayont")
            public void Bind(string DN = "cn=gtildap,ou=odessa,o=world", string Password = "wemayont")
            {
                //bHowToProceed = true;
                ldapConn.Bind(DN, Password);
                bindCount++;
            }

            public string GenerateLogin(string login)
            {
                return GenerateUniqueCN(login);
            }

            public bool VerifyPassword(string login, string password)
            {
                LdapEntry nextEntry = null;

                try
                {
                    LdapSearchResults lsc = ldapConn.Search("", LdapConnection.SCOPE_SUB, "cn=" + login.Trim(), null, false);
                    while (lsc.hasMore())
                    {
                        nextEntry = lsc.next();
                    }
                }
                catch (LdapException e)
                {
                    throw e;
                }

                string DN = nextEntry.DN;
                LdapAttribute passwordAttr = new LdapAttribute("userPassword", password);

                bool correct = ldapConn.Compare(DN, passwordAttr);

                return correct;
            }

            public string FindEmail(string username)
            {
                string email = "";

                LdapEntry nextEntry = null;

                try
                {
                    LdapSearchResults lsc = ldapConn.Search("", LdapConnection.SCOPE_SUB, "cn=" + username.Trim(), null, false);
                    while (lsc.hasMore())
                    {
                        nextEntry = lsc.next();
                    }
                }
                catch (LdapException e)
                {
                    throw e;
                }

                var attribute = nextEntry.getAttribute("mail");
                if (attribute != null)
                {
                    email = attribute.StringValueArray[0];
                }

                return email;
            }


            public bool EntryExists(string login)
            {
                bHowToProceed = false;
                bindCount = 0;

                LdapEntry nextEntry = null;
                try
                {
                    LdapSearchResults lsc = ldapConn.Search("", LdapConnection.SCOPE_SUB, "cn=" + login.Trim(), null, false);
                    while (lsc.hasMore())
                    {
                        nextEntry = lsc.next();
                    }
                }
                catch (LdapException e)
                {
                    throw e;
                }

                return nextEntry == null ? false : true;
            }

            public bool CreateEntry(NovellOrganizationContactPerson person)
            {
                bool created = false;
                string containerName = "ou=users,o=alien";

                if (person.Login != "")
                {
                    try
                    {
                        LdapAttributeSet attributeSet = new LdapAttributeSet();
                        attributeSet.Add(new LdapAttribute(
                        "objectclass", "inetOrgPerson"));
                        attributeSet.Add(new LdapAttribute("cn",
                        new string[] { person.Login }));
                        attributeSet.Add(new LdapAttribute("givenname", person.FirstName));
                        attributeSet.Add(new LdapAttribute("sn", person.Surname));
                        attributeSet.Add(new LdapAttribute("mail", person.Email));
                        attributeSet.Add(new LdapAttribute("userpassword", person.Password));

                        string dn = "cn=" + person.Login + "," + containerName;
                        LdapEntry newEntry = new LdapEntry(dn, attributeSet);
                        ldapConn.Add(newEntry);
                        created = true;
                    }
                    catch (LdapException e)
                    {
                        throw e;
                    }
                }
                return created;
            }

            private string GenerateUniqueCN(string commonName)
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


            //method for correct eDirectory work
            public bool MySSLHandler(X509Certificate certificate, int[] certificateErrors)
            {

                Mono.Security.X509.X509Store store = null;
                Mono.Security.X509.X509Stores stores = Mono.Security.X509.X509StoreManager.LocalMachine;
                store = stores.TrustedRoot;

                //Import the details of the certificate from the server.

                Mono.Security.X509.X509Certificate x509 = null;
                Mono.Security.X509.X509CertificateCollection coll = new Mono.Security.X509.X509CertificateCollection();
                byte[] data = certificate.GetRawCertData();
                if (data != null)
                    x509 = new Mono.Security.X509.X509Certificate(data);

                //List the details of the Server

                //if (bindCount == 1)
                //{

                res += "<b><u>CERTIFICATE DETAILS:</b></u> <br>";
                res += "  Self Signed = " + x509.IsSelfSigned + "  X.509  version=" + x509.Version + "<br>";
                res += "  Serial Number: " + CryptoConvert.ToHex(x509.SerialNumber) + "<br>";
                res += "  Issuer Name:   " + x509.IssuerName.ToString() + "<br>";
                res += "  Subject Name:  " + x509.SubjectName.ToString() + "<br>";
                res += "  Valid From:    " + x509.ValidFrom.ToString() + "<br>";
                res += "  Valid Until:   " + x509.ValidUntil.ToString() + "<br>";
                res += "  Unique Hash:   " + CryptoConvert.ToHex(x509.Hash).ToString() + "<br>";
                //}



                if (bHowToProceed == true)
                {
                    //Add the certificate to the store. This is \Documents and Settings\program data\.mono. . .
                    if (x509 != null)
                        coll.Add(x509);
                    store.Import(x509);
                    if (bindCount == 1)
                        removeFlag = true;
                }

                if (bHowToProceed == false)
                {
                    //Remove the certificate added from the store.

                    if (removeFlag == true && bindCount > 1)
                    {
                        foreach (Mono.Security.X509.X509Certificate xt509 in store.Certificates)
                        {
                            if (CryptoConvert.ToHex(xt509.Hash) == CryptoConvert.ToHex(x509.Hash))
                            {
                                store.Remove(x509);
                            }
                        }
                    }
                    //Response.Write("SSL Bind Failed.");
                }
                return bHowToProceed;
            }


            public void Dispose()
            {
                ldapConn.Disconnect();
            }
        }


    }
}