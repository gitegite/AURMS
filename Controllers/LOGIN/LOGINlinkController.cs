using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AURMS.Models;

namespace AURMS.Controllers.LOGIN
{
    public class LOGINlinkController : Controller
    {
        // GET: LOGINlink
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            Session["username"] = form["UserID"];
            Session["password"] = form["userPassword"];
            using (RMSDataContext rmsd = new RMSDataContext())
            {
                var _usergroup = (from u in rmsd.Users
                                  where (u.UserCode == Session["username"].ToString())
                                  select u).FirstOrDefault();

                Session["userid"] = _usergroup.UserID;
                Session["usergroup"] = _usergroup.GroupID;

                //Admin Redirection (Username "Admin" has GroudID = 2)
                //  Login by using "Admin" as Username will trigger below If

                if (_usergroup.GroupID == 5555)
                {
                    return RedirectToAction("ListForAdmin", "ADMINreservation");
                }
                else if (_usergroup.GroupID > 200)
                {
                    //return RedirectToAction("../ADMINreservation/List", ManageReservation.ReservationList().ToList());
                    // Admin Redirection
                    return RedirectToAction("../ADMINreservation/List");
                }
                else
                {
                    // User Redirection

                    return RedirectToAction("UserPending", "USERrecord");
                }

            }

        }

        public ActionResult Logout()
        {

            Session.Clear();
            if (Request.Cookies["MyCookie"] != null)
            {
                var c = new HttpCookie("MyCookie");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }
            return RedirectToAction("../");
        }

    }
}