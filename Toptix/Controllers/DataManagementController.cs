using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Toptix.Controllers
{
    public class DataManagementController : Controller
    {
        //
        // GET: /DataManagement/
        //[Authorize(Roles = "DataManagementAndReports")]
        public ActionResult Index()
        {
            return View();
        }

    }
}
