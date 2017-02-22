using GTIWebAPI.Models.Account;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTIWebAPI.Novell
{
    //class which knows how to user NovellProvider class
    //one method - one connect - one action with Novell Provider ??? 

    public static class NovellManager
    {
        //Миша писал в скайп 17.02 
        //сейчас только 0.9 работает в режиме non-TLS. Остальные требуют TLS для подключения к ДВФЗ-серверу. 
        //Вообще я бы хотел для авторизации и всех операций с LDAP использовать в порядке очередности: 0.20, 1.1, 0.6, 0.9.
        private static string[] ServerAddresses = { "192.168.0.20", "192.168.1.1", "192.168.0.6", "192.168.0.9" };

        //noone knows server address
        //it'll be better if we change location to web.config 

        private static bool VerifyPassword(string email, string password)
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
                        bool entryExist = novell.FindEntryByEmail(email);
                        if (entryExist)
                        {
                            //verify password
                            result = novell.VerifyPassword(email, password);
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


        public static bool CredentialsCorrect(string email, string password)
        {
            bool result = VerifyPassword(email, password);
            return result;
        }

        public static bool CreateOrganization(string email, string password)
        {
            bool result = CreateNovellOrganization(email, password);
            return result;
        }

        private static bool CreateNovellOrganization(string email, string password)
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
                        bool entryExist = novell.FindEntryByEmail(email);
                        if (!entryExist)
                        {
                            //create new entry if we can 
                            result = novell.CreateEntry(email, password);
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


    }
}