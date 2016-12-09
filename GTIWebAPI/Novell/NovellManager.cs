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
    public class NovellManager
    {
        private string ServerAddress { get; set; }
        private ApplicationUserManager userManager { get; set; }
        public NovellManager(string serverAddress, ApplicationUserManager manager)
        {
            if (serverAddress != null && serverAddress != "")
            {
                ServerAddress = serverAddress;
            }
            userManager = manager;   
        }

        private bool NovellEntryExist(string UserName, string Password, out string Email)
        {
            bool result = false;
            //проверка в коннекте на соответствие в eDirectory
            using (NovellProvider novell = new NovellProvider())
            {
                try
                {
                    novell.Bind(ServerAddress, "cn=gtildap,ou=Tech,ou=ALL,o=World", "wemayont");
                    NovellEntry entry = novell.FindEntry(UserName);
                    Email = entry.Email;
                    string DN = entry.DN;
                    novell.Bind(ServerAddress, DN, Password);
                    result = true;
                }
                catch
                {
                    Email = "";
                    result = false;
                }
            }
            return result;
        }

        public ApplicationUser Find(string UserName, string Password)
        {
            ApplicationUser user = null;
            String Email = "";
            bool result = NovellEntryExist(UserName, Password, out Email);
            if (result == false)
            {
                user = null;
            }
            else
            {
                user = userManager.Find(UserName, Password);
                if (user == null)
                {
                    ApplicationUser newUser = new ApplicationUser { UserName = UserName, Email = Email };
                    userManager.Create(user, Password);
                    userManager.AddToRole(user.Id, "User");
                    CreateUserEmployee(newUser.Id);

                    user = newUser;
                }
            }
            return user;
        }

        private void CreateUserEmployee(string userId)
        {
            using (Models.Context.DbPersonnel db = new Models.Context.DbPersonnel())
            {
                Models.Dictionary.Address address = new Models.Dictionary.Address();
                address.Id = address.NewId(db);
                //db.Address.Add(address);

                Models.Employees.Employee employee = new Models.Employees.Employee();
                employee.Id = employee.NewId(db);
                employee.UserId = userId;
                employee.AddressId = address.Id;
                db.Employee.Add(employee);
                db.SaveChanges();
            }

        }

        //private ApplicationUser CreateNovellUser(ApplicationUserManager userManager, ApplicationUser user, string Password)
        //{
        //    userManager.Create(user, Password);
        //    userManager.AddToRole(user.Id, "User");
        //    return user;
        //}





    }
}