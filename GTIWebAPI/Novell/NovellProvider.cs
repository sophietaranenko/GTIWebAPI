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


        public NovellProvider()
        {
            ldapConn = new LdapConnection();
            ldapConn.SecureSocketLayer = true;
            bHowToProceed = true;
            ldapConn.UserDefinedServerCertValidationDelegate += new CertificateValidationCallback(MySSLHandler);
        }

        public void Bind(string LdapServer = "192.168.0.1", string DN = "cn=gtildap,ou=Tech,ou=ALL,o=World", string Password = "wemayont")
        {
            bHowToProceed = true;
            bindCount = 0;
            ldapConn.Connect(LdapServer, 636);
            ldapConn.Bind(DN, Password);
        }

        public NovellEntry FindEntry(string UserName)
        {
            bHowToProceed = false;
            bindCount = 0;

            LdapEntry nextEntry = null;
            try
            {
                LdapSearchResults lsc = ldapConn.Search("", LdapConnection.SCOPE_SUB, "cn=" + UserName.Trim(), null, false);
                while (lsc.hasMore())
                {
                    try
                    {
                        nextEntry = lsc.next();
                    }
                    catch (Exception e)
                    {
                        throw new Exception("No such user in eDirectory found", e);
                    }
                }
            }
            catch (Exception myException)
            {
                RuntimeWrappedException rwe = myException as RuntimeWrappedException;
                if (rwe != null)
                {
                    String s = rwe.WrappedException as String;
                    if (s != null)
                    {
                        Console.WriteLine(s);
                    }
                }
                else
                {
                    throw new Exception("Cannot connect to LDAP Server " + ldapConn.Host + ". LDAP Connection eDirectory error.", myException);
                }
            }
            return new NovellEntry(nextEntry);
        }

        public bool BindEntry(LdapEntry entry, string Password)
        {
            bool result = false;
            if (entry != null)
            {
                string DN = entry.DN;
                ldapConn.Disconnect();
                try
                {
                    bHowToProceed = true;
                    bindCount = 0;
                    Bind(DN = entry.DN, Password);
                }
                catch
                {
                    result = false;
                }
                result = true;
            }
            return result;
        }

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