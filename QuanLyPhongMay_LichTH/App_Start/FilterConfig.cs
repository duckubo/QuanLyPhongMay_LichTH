using System.Web;
using System.Web.Mvc;

namespace QuanLyPhongMay_LichTH
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
