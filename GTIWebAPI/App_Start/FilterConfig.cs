using System.Web;
using System.Web.Mvc;

namespace GTIWebAPI
{
    /// <summary>
    /// Filter config
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// f.e. [Autorized] 
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
