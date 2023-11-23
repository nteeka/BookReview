using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KK_BookStore.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base

        protected void SetAlert(string message, string type)
        {
            @TempData["AlertMessage"] = message;
            switch (type) 
            {
                case "success":
                    @TempData["AlertType"] = "alert-success";
                    break;
                case "error":
                    @TempData["AlertType"] = "alert-error";
                    break;
                case "warning":
                    @TempData["AlertType"] = "alert-warning";
                    break;
                default:
                    @TempData["AlertType"] = "";
                    break;
            }
        }
    }
}