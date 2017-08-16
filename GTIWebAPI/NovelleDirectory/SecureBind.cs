using System;
using Novell.Directory.Ldap;
using Novell.Directory.Ldap.Utilclass;

namespace GTIWebAPI.NovelleDirectory
{
    /******************************************************************************
    * The MIT License
    * Copyright (c) 2003 Novell Inc.  www.novell.com
    * 
    * Permission is hereby granted, free of charge, to any person obtaining  a copy
    * of this software and associated documentation files (the Software), to deal
    * in the Software without restriction, including  without limitation the rights
    * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
    * copies of the Software, and to  permit persons to whom the Software is 
    * furnished to do so, subject to the following conditions:
    * 
    * The above copyright notice and this permission notice shall be included in 
    * all copies or substantial portions of the Software.
    * 
    * THE SOFTWARE IS PROVIDED AS IS, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
    * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
    * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
    * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    * SOFTWARE.
    *******************************************************************************/
    //
    // Samples.SecureBind.cs
    //
    // Author:
    //   Sunil Kumar (Sunilk@novell.com)
    //
    // (C) 2004 Novell, Inc (http://www.novell.com)
    //

        public class SecureBind
        {

            //SecureBind() { }

            public static void Main(string ldapHost, int ldapPort, string loginDN, string password)
            {

                LdapConnection conn = null;
                try
                {
                    conn = new LdapConnection();
                    conn.SecureSocketLayer = true;
                    Console.WriteLine("Connecting to:" + ldapHost);
                    conn.Connect(ldapHost, ldapPort);
                    conn.Bind(loginDN, password);
                    Console.WriteLine(" SSL Bind Successfull");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error:" + e.Message);
                }
                conn.Disconnect();
            }
        }
   


}
