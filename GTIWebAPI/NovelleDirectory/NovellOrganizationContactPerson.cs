﻿using GTIWebAPI.Models.Organizations;
using GTIWebAPI.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GTIWebAPI.NovelleDirectory
{
    //class for adding to novell edirectory a new user 
    //it might take some operations to do: transliterate names and generate novell login
    public class NovellOrganizationContactPerson : INovellOrganizationContactPerson
    {
        public NovellOrganizationContactPerson(OrganizationContactPersonView person)
        {
            if (person.FirstName != null && person.LastName != null)
            {
                FirstName = TransliterateName(person.FirstName);
                Surname = TransliterateName(person.LastName);
                Email = person.Email;
                Login = ConstructLogin(TransliterateName(person.FirstName), TransliterateName(person.LastName));
                Password = RandomPasswordGenerator.GeneratePassword(8);
            }
            else
            {
                return;
            }

        }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Surname { get; set; }

        public string FirstName { get; set; }

        private string TransliterateName(string name)
        {
            if (name == null || name.Length < 2)
            {
                throw new ArgumentException("Invalid value to transliterate");
            }
            TransliterationProvider translit = new TransliterationProvider();
            name = translit.ToLatin(name);
            name = name.Replace(" ", "");
            return name;
        }

        private string ConstructLogin(string transliteratedFirstName, string transliteratedLastName)
        {
            string login = "";
            if (transliteratedFirstName.Length < 2 || transliteratedLastName.Length < 2
                || Regex.IsMatch(transliteratedFirstName, @"\p{IsCyrillic}")
                || Regex.IsMatch(transliteratedLastName, @"\p{IsCyrillic}"))
            {
                throw new ArgumentException("Invalid first name or last name");
            }
            transliteratedFirstName = (transliteratedFirstName.Substring(0, 1).ToUpper() + transliteratedFirstName.Substring(1, transliteratedFirstName.Length - 1).ToLower()).Trim();
            transliteratedLastName = (transliteratedLastName.Substring(0, 1).ToUpper() + transliteratedLastName.Substring(1, transliteratedLastName.Length - 1).ToLower()).Trim();
            login = transliteratedFirstName + transliteratedLastName;

            return login;
        }
    }


}

