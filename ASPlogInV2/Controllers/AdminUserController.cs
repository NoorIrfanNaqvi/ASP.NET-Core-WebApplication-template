using Microsoft.AspNetCore.Mvc;

namespace ASPlogInV2.Controllers
{
    public class AdminUserController : Controller
    {
        //Dashboard
        [Route("Dashboard")]
        public IActionResult DashboardPage()
        {
            //Send user back to log in page, if session expires or is found to be null
            if (HttpContext.Session.GetString("UsernameSession") == null || HttpContext.Session.GetString("UsernameSession") == "")
            {
                return RedirectToAction("LoginPage", "Home");
            }
            ViewBag.SessionUsername = HttpContext.Session.GetString("UsernameSession");
            return View();
        }
    }
}
