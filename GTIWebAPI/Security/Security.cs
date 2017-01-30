using GTIWebAPI.Models.Account;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace GTIWebAPI.Security
{
    public static class Security
    {
        public static void InitializeClass()
        {
            //DbService db = new DbService();
            ApplicationDbContext db = new ApplicationDbContext();
            Assembly asm = Assembly.GetExecutingAssembly();
            var result = asm.GetTypes()
                .Where(type => typeof(ApiController).IsAssignableFrom(type)).ToList();
            foreach (var item in result)
            {
                string controllerName = item.Name.Replace("Controller", "");
                var methods = item.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public)
                    .Where(m => m.GetCustomAttributes(typeof(Filters.GTIFilter), true).Any() || m.GetCustomAttributes(typeof(Filters.GTIOfficeFilter), true).Any()).ToList();
                if (methods.Count > 0)
                {
                    //поиск контроллера с таким нэймом
                    //если нет, то записать новый
                    Controller controller = db.Controllers.Where(c => c.Name == controllerName).FirstOrDefault();
                    if (controller == null)
                    {
                        controller = new Controller() { Name = controllerName };
                        controller.Id = controller.NewId(db);
                        db.Controllers.Add(controller);
                        db.SaveChanges();
                    }
                    int controllerId = controller.Id;

                    foreach (var method in methods)
                    {
                        //поиск метода с таким неймом
                        //если нет, то записать новый
                        string methodName = method.Name;
                        Models.Security.Action action = db.Actions.Where(m => m.ControllerId == controllerId && m.Name == methodName).FirstOrDefault();
                        if (action == null)
                        {
                            action = new Models.Security.Action() { Name = methodName, ControllerId = controllerId };
                            action.Id = action.NewId(db);
                            db.Actions.Add(action);
                            db.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}
