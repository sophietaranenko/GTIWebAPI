using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(GTIWebAPI.Startup))]

namespace GTIWebAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //при добавлении новых контроллеров / методов разблокировать
            //код заполнит БД контроллерами и соответствующими действиями
           // Security.Security.InitializeClass();

            
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
