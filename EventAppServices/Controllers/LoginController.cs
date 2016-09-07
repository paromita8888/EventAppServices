using System;
using System.Web.Mvc;
using EventAppServices.Models;
using System.Configuration;
using System.Collections.Generic;
using Microsoft.Azure.NotificationHubs;
using System.Web.Security;
using System.DirectoryServices.AccountManagement;
using System.Web.Http.Cors;

namespace EventAppServices.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LoginController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string txtUserName, string txtPassword)
        {
            // Lets first check if the Model is valid or not
            using (EventAppDataModelEntity entities = new EventAppDataModelEntity())
            {
                try
                {
                    ValidateUserDTO retObject = new ValidateUserDTO();
                    string UserName = "domainname\\" + txtUserName;
                    using (PrincipalContext pCtx = new PrincipalContext(ContextType.Domain, "0.0.0.0", UserName, txtPassword))
                    {
                        FormsAuthentication.SetAuthCookie(UserName, false);
                        GroupPrincipal group = GroupPrincipal.FindByIdentity(pCtx, "USERS");
                        var user = UserPrincipal.FindByIdentity(pCtx, UserName);
                        retObject.FirstName = user.GivenName;
                        retObject.LastName = user.Surname;
                        retObject.Name = user.DisplayName;
                         retObject.EmpId = user.EmployeeId;
                        retObject.EmailId  = user.SamAccountName + "@doaminname.com";
                        Session["Username"] = user.DisplayName;
                        return RedirectToAction("SendPushNotification", "PushNotification");
                    }
                }
                catch (Exception ex)
                {
                    //string output = "Message: " + ex.Message + Environment.NewLine + "Stack Trace: " + ex.StackTrace;
                    ViewBag.Message = "Invalid User Credentials";
                    return View();
                }
            }
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }
        //public bool AuthenticateUser(bool val)
        //{
        //    if (val)
        //    { return true; }
        //    else
        //    { return false; }

        //}
    }
}
