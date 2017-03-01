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
    public class NovellProvider : IDisposable
    {
        private string res = "";
        private bool bHowToProceed = false;
        private bool removeFlag = false;
        private int bindCount = 0;

        private LdapConnection ldapConn;

        //to work with
        //using(Novell novell = new Novell())
        //{
        //novell.Bind("192.168.0.1", "cn=gtildap,ou=Tech,ou=ALL,o=World", "wemayont")
        //novell.BindEntry(novell.FindEntry("staranenko"), "fauleksi")
        //string Email = novell.FindEntry("staranenko").Email;
        //}


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

        public bool CommonNameExist(string commonName)
        {
            // bHowToProceed = false;
            //bindCount = 0;

            LdapEntry nextEntry = null;
            try
            {
                LdapSearchResults lsc = ldapConn.Search("", LdapConnection.SCOPE_SUB, "cn=" + commonName.Trim(), null, false);
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

        public bool VerifyPassword(string email, string password)
        {
            //bHowToProceed = false;
            //bindCount = 0;

            LdapEntry nextEntry = null;
            try
            {
                LdapSearchResults lsc = ldapConn.Search("", LdapConnection.SCOPE_SUB, "mail=" + email.Trim(), null, false);
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


        public bool FindEntryByName(string UserName)
        {
            bHowToProceed = false;
            bindCount = 0;

            LdapEntry nextEntry = null;
            try
            {
                LdapSearchResults lsc = ldapConn.Search("", LdapConnection.SCOPE_SUB, "cn=" + UserName.Trim(), null, false);
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

        public bool FindEntryByEmail(string Email)
        {
            bHowToProceed = false;
            bindCount = 0;

            LdapEntry nextEntry = null;
            try
            {
                LdapSearchResults lsc = ldapConn.Search("", LdapConnection.SCOPE_SUB, "mail=" + Email.Trim(), null, false);
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

        public bool CreateEntry(string email, string password)
        {
            bool created = false;

            int symbolPosition = email.IndexOf('@');
            string commonName = "new_LDAP_user";

            string containerName = "ou=users,o=alien";

            if (symbolPosition > 0)
            {
                commonName = email.Substring(0, email.IndexOf('@'));
            }
            commonName = GenerateCN(commonName);

            if (commonName != "")
            {
                try
                {
                    LdapAttributeSet attributeSet = new LdapAttributeSet();
                    attributeSet.Add(new LdapAttribute(
                    "objectclass", "inetOrgPerson"));
                    attributeSet.Add(new LdapAttribute("cn",
                    new string[] { commonName }));
                    attributeSet.Add(new LdapAttribute("givenname", commonName));
                    attributeSet.Add(new LdapAttribute("sn", commonName));
                    attributeSet.Add(new LdapAttribute("mail", email));
                    attributeSet.Add(new LdapAttribute("userpassword", password));

                    string dn = "cn=" + commonName + "," + containerName;
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

        private string GenerateCN(string commonName)
        {
            bool cnExist = true;

            if (CommonNameExist(commonName))
            {
                for (int i = 1; ; i++)
                {
                    cnExist = CommonNameExist(commonName + "_" + i.ToString().Trim());
                    if (cnExist == false)
                    {
                        commonName = commonName + "_" + i.ToString().Trim();
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