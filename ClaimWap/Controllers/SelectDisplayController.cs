using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClaimWap.Controllers
{
    public class SelectDisplayController : Controller
    {
        //
        // GET: /SelectDisplay/

        public ActionResult Index()
        {
            // Redirect if either credential is missing
            if (Session["UserID"] == null || Session["UserPassword"] == null)
            {
                return RedirectToAction("LogIn", "Account");
            }
            else
            {
                string User = Convert.ToString(Session["UserID"]);
                string UserType = Convert.ToString(Session["UserType"]);
                string Company = Convert.ToString(Session["company"]);

                // If UserType is still empty/null, redirect to login (or handle appropriately)
                if (string.IsNullOrEmpty(UserType))
                {
                    return RedirectToAction("LogIn", "Account");
                }

                if (UserType == "3") //PM//
                {
                    if (User != "paxte")
                    {
                        return RedirectToAction("Index", "ProcessApprove");
                    }
                    else
                    {
                        return RedirectToAction("Index", "ProcessApprove_WHDM_MD");
                    }
                }
                else if (UserType == "8" || UserType == "9") //PM//
                {
                    return RedirectToAction("Index", "Disposal");
                }

                ViewBag.UserId = User;
                ViewBag.UserType = UserType;
                ViewBag.Company = Company;
            }
            return View();
        }

    }
}
