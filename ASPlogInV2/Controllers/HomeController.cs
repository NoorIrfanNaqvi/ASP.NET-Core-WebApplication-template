using System.Diagnostics;
using ASPlogInV2.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASPlogInV2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        //Session constants
        private const string UsernameSession = "_Username"; //For user session
        private const string ChangeEmailPasswordSession = "_Useremail"; //For when the user needs to change their password

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //LOGIN PAGE
        [Route ("")]
        [Route ("LogIn")]
        public IActionResult Index()
        {
            var newModel = new CheckRegistrationData()
            { useremail = "", username = "", password = "", confirmpassword = "", agreedToTerms = false };
            ViewBag.ErrorMessage = string.Empty;
            return View(newModel);
        }

        [HttpPost]
        [Route("")]
        [Route("LogIn")]
        public IActionResult Index(CheckRegistrationData LogInDetails)
        {
            //Checks if the account exsists and if the user placed in the correct password
            string MessageOfError = Helpers.LogInHelper.isAccountExsit(LogInDetails.username, LogInDetails.password);
            if (MessageOfError == "" || MessageOfError == null)
            {
                HttpContext.Session.SetString(UsernameSession, LogInDetails.username);
                return RedirectToAction("Dashboard", "Home");
            }
            else 
            {
                ViewBag.ErrorMessage = MessageOfError;
                return View();
            }
        }

        //Register page
        [Route ("SignUp")]
        public IActionResult RegisterAcount()
        {
            var newModel = new CheckRegistrationData()
            {useremail="", username="", password="", confirmpassword="",agreedToTerms= false};
            ViewBag.ErrorMessage = string.Empty;
            return View(newModel);
        }

        [HttpPost]
        [Route("SignUp")]
        public IActionResult RegisterAcount(CheckRegistrationData NewAccount)
        {
            if (NewAccount.agreedToTerms == false) 
            {
                //Tell user they need to accept terms to create account.
                ViewBag.ErrorMessage = "You need to accept terms & services to continue.";
                return View(NewAccount);
            }
            else
            {
                //If terms were ticked to be agreed to.
                string ErrorMessage = Helpers.RegistorAccountHelper.validatingAccountCreation(NewAccount);
                if (ErrorMessage == "" || ErrorMessage == null)
                {
                    HttpContext.Session.SetString(UsernameSession, NewAccount.username);
                    return RedirectToAction("Dashboard", "Home");
                }
                else
                {
                    ViewBag.ErrorMessage = ErrorMessage;
                    return View(NewAccount);
                }
                
            }
        }

        //Forgot password page
        [Route ("ForgotPassword")]
        public IActionResult ForgetPasswordPage()
        {
            ViewBag.ErrorMessage = "";
            CheckRegistrationData empty = new CheckRegistrationData() { useremail = ""};
            return View(empty);
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public IActionResult ForgetPasswordPage(CheckRegistrationData email)
        {
            //Checking if the email is null
            if (email.useremail == null)
            {
                ViewBag.ErrorMessage = "Email cannot be left blank";
                return View(email);
            }
            //Checking if email is not in database
            else if (!Helpers.ChangePasswordHelper.isEmailInDB(email.useremail))
            {
                ViewBag.ErrorMessage = "Email is not connected to any account with this website.";
                return View(email);
            }
            //If email is valid
            else
            {
                HttpContext.Session.SetString(ChangeEmailPasswordSession, email.useremail);
                return RedirectToAction("ChangePasswordPage", "Home");
            }
        }

        //Reset password page
        [Route ("ResetPassword")]
        public IActionResult ChangePasswordPage()
        {
            //Send user back to forgot password page if the email somehow gets missing
            if (HttpContext.Session.GetString(ChangeEmailPasswordSession) == null || HttpContext.Session.GetString(ChangeEmailPasswordSession) == "")
            {
                return RedirectToAction("ForgotPasswordPage", "Home");
            }

            //If session has stored the email in the changeEmailPasswordSession, grab email for View
            var Email = HttpContext.Session.GetString(ChangeEmailPasswordSession);
            //Make the email in a format the ChangePasswordPage will accept. (CheckRegistrationData model)
            CheckRegistrationData AccountEmail = new CheckRegistrationData() {useremail = Email};

            return View(AccountEmail);
        }

        [HttpPost]
        [Route("ResetPassword")]
        public IActionResult ChangePasswordPage(CheckRegistrationData userDetails)
        {
            //Send user back to forgot password page if the email somehow gets missing
            if (HttpContext.Session.GetString(ChangeEmailPasswordSession) == null || HttpContext.Session.GetString(ChangeEmailPasswordSession) == "")
            {
                return RedirectToAction("ForgotPasswordPage", "Home");
            }

            //Make sure the email is in the userDetails
            string AccountEmail = HttpContext.Session.GetString(ChangeEmailPasswordSession);
            userDetails.useremail = AccountEmail;
            //If password change is valid
            string ErrorMsg = Helpers.ChangePasswordHelper.changeUserPassword(AccountEmail, userDetails.password, userDetails.confirmpassword);
            if (ErrorMsg == "")
            {
                return RedirectToAction("Index", "Home");
            }
            //If fields were not valid
            else
            {
                ViewBag.ErrorMessage = ErrorMsg;
                return View(userDetails);
            }
        }

        //Dashboard
        public IActionResult Dashboard()
        {
            ViewBag.SessionUsername = HttpContext.Session.GetString(UsernameSession);
            if (HttpContext.Session.GetString(UsernameSession) == null || HttpContext.Session.GetString(UsernameSession) == "")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
