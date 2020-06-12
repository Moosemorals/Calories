using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Calories.Lib;
using Microsoft.AspNetCore.Mvc;

namespace Calories.Controllers
{
    public abstract class BaseController : Controller
    {
        public ActionResult RedirectWithMessage(string action, string message, params object[] param)
        {
            Message(message, param);
            return RedirectToAction(action);
        }

        public void Message(string message, params object[] param)
        {
            TempData.AddMessage(message, param);
        }
    }


}
