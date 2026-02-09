using ASPlogInV2.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASPlogInV2.Controllers
{
    public class AdminUserController : Controller
    {
        //Dashboard page
        [Route("AdminUser/Dashboard")]
        public IActionResult DashboardPage()
        {
            //Send user back to log in page, if session expires or is found to be null
            if (HttpContext.Session.GetString("UsernameSession") == null || HttpContext.Session.GetString("UsernameSession") == "")
            {
                return RedirectToAction("LoginPage", "Home");
            }

            //Checking if the user is an admin account or a normal account
            using (var db = new DbConnectorSQLite())
            {
                //Grab current username from user session
                UserAccounts activeUser = db.UserAccounts.Where(u => u.UserName == HttpContext.Session.GetString("UsernameSession")).FirstOrDefault();

                //If user not admin, send them to user dashboard
                if (!activeUser.isAdmin)
                {
                    //Send user back to normal dashboard
                    return RedirectToAction("DashboardPage", "User");
                }
            }
            ViewBag.SessionUsername = HttpContext.Session.GetString("UsernameSession");
            return View();
        }

    }
}
