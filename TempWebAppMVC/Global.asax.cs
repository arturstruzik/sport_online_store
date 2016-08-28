using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TempWebAppMVC.Models;

namespace TempWebAppMVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs args)
        {
            if (Context.User != null)
            {
                StoreOnlineEntities db = new StoreOnlineEntities();

                IEnumerable<UserCategory> roles =
                    db.UserCategories.Where(
                        x =>
                            x.UserCategoryId ==
                            db.Users.FirstOrDefault(y => y.Login == Context.User.Identity.Name).UserCategoryId);

                string[] rolesArray = new string[roles.Count()];
                for (int i = 0; i < roles.Count(); i++)
                {
                    rolesArray[i] = roles.ElementAt(i).Name.Trim();
                }

                GenericPrincipal gp = new GenericPrincipal(Context.User.Identity, rolesArray);
                Context.User = gp;
            }
        }
    }
}
