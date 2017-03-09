using GTIWebAPI.Models.Organizations;
using GTIWebAPI.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GTIWebAPI.Novell
{
    //class for adding to novell edirectory a new user 
    //it might take some operations to do: transliterate names and generate novell login
    public class NovellOrganizationContactPerson
    {
        public NovellOrganizationContactPerson(OrganizationContactPersonView person)
        {
            string login = NovellManager.GenerateLogin(ConstructLogin(TransliterateName(person.FirstName), TransliterateName(person.LastName))); 

            FirstName = TransliterateName(person.FirstName);
            Surname = TransliterateName(person.LastName);
            Email = person.Email;
            // Login = NovellManager.GenerateLogin(ConstructLogin(TransliterateName(person.FirstName), TransliterateName(person.LastName)));
            Login = login;
            Password = RandomPasswordGenerator.GeneratePassword(8);
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

