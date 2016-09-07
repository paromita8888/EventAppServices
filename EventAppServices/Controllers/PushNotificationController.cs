using EventAppServices.Models;
using Microsoft.Azure.NotificationHubs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Web.Http.Results;
using System.Web.Mvc; 

namespace EventAppServices.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PushNotificationController : Controller
    {
        // private IUserRepository userRepository;


        #region Private variables
         
        //private static string NOTIFICATION_HUB_NAME = ConfigurationManager.AppSettings["NOTIFICATION_HUB_NAME"];
        //private static string NOTIFICATION_HUB_FULL_ACCESS_CONNECTION_STRING = ConfigurationManager.AppSettings["NOTIFICATION_HUB_FULL_ACCESS_CONNECTION_STRING"];
        #endregion

        public PushNotificationController()
        {
            var context = new EventAppDataModelEntity();

            //PlaybookContext context = new PlaybookContext();
            //userRepository = new UserRepository(context);
        }

     //   [Authorize]
        //[SessionExpireFilter]
  //      [AllowAnonymous]
        public ActionResult SendPushNotification()
        {
            //if (!Request.IsAuthenticated)
            //{
            //    //if (UserPrincipal.Current.EmailAddress!=null)
            //    return RedirectToAction("Login", "Login");
            //}
            //if ( (Session["Username"]  ==   null) || (Session["Username"].ToString() == ""))
            //{
            //    //return new HttpUnauthorizedResult(); //This or the below statement should redirect the user to forms login page
            //    return RedirectToAction("Login", "Login");
            //}
            PushNotification model = new PushNotification();

            ViewBag.AppleConfig = FillDropDownItems();
            return View(model);
        }

        private List<SelectListItem> FillDropDownItems()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem
            {
                Text = "--Select--",
                Value = "1" 
            });
            //items.Add(new SelectListItem
            //{
            //    Text = "App Store Dev",
            //    Value = "2"
            //});
            //items.Add(new SelectListItem
            //{
            //    Text = "App Store Prod",
            //    Value = "3"
            //});
            //items.Add(new SelectListItem
            //{
            //    Text = "Enterprise Dev",
            //    Value = "4",
            //    Selected = true
            //});
            items.Add(new SelectListItem
            {
                Text = "Enterprise Prod",
                Value = "5",
                Selected = true
            });
            return items;
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult SendPushNotification(PushNotification model)
        {

            if (ModelState.IsValid)
            {
                string errormessage = string.Empty;
                if (model.AppleConfig == "1")
                {
                    errormessage += "<p>Please select valid Apple Configuration.</p>";
                }
                else
                {
                    bool isDevEnvironment = true;
                    if (model.AppleConfig.Equals("5"))
                    { isDevEnvironment = false; }
                    //if (ConfigurationManager.AppSettings["SearchServiceName"] == "uplaybooksearch")
                    //{
                    //    isDevEnvironment = false;
                    //}
                    string notificationHubName = string.Empty;
                    string notificationHubConnectionString = string.Empty;
                    switch (model.AppleConfig)
                    {
                        case "2":
                            if (isDevEnvironment)
                            {
                                notificationHubName = "TechVanatge2016_Test";
                                notificationHubConnectionString = ConfigurationManager.AppSettings["Microsoft.Azure.NotificationHubs.ConnectionString"];
                            }
                            else
                            {
                                ViewBag.Message = MvcHtmlString.Create("You dont have permission to send notifications using this notification hub.");
                                ViewBag.AppleConfig = FillDropDownItems();
                                return View(model);
                            }
                            break;
                        case "3":
                            if (!isDevEnvironment)
                            {
                               // notificationHubName = ConfigurationManager.AppSettings["NotificationHub_AppStoreProd"];
                                notificationHubName = "techvantage_2016";
                                notificationHubConnectionString = ConfigurationManager.AppSettings["Microsoft.Azure.NotificationHubs.ConnectionString_Prod"];
                            }
                            else
                            {
                                ViewBag.Message = MvcHtmlString.Create("You dont have permission to send notifications using this notification hub.");
                                ViewBag.AppleConfig = FillDropDownItems();
                                return View(model);
                            }
                            break;
                        case "4":
                            if (isDevEnvironment)
                            {
                                notificationHubName = "TechVanatge2016_Test";
                                notificationHubConnectionString = ConfigurationManager.AppSettings["Microsoft.Azure.NotificationHubs.ConnectionString"];
                            }
                            else
                            {
                                ViewBag.Message = MvcHtmlString.Create("You dont have permission to send notifications using this notification hub.");
                                ViewBag.AppleConfig = FillDropDownItems();
                                return View(model);
                            }
                            break;
                        case "5":
                            if (!isDevEnvironment)
                            {
                                notificationHubName = "techvantage_2016";

                                notificationHubConnectionString = ConfigurationManager.AppSettings["Microsoft.Azure.NotificationHubs.ConnectionString_Prod"];
                            }
                            else
                            {
                                ViewBag.Message = MvcHtmlString.Create("You dont have permission to send notifications using this notification hub.");
                                ViewBag.AppleConfig = FillDropDownItems();
                                return View(model);
                            }
                            break;
                    }
                    bool resultValid = false;
                    var notificationId=    SaveNotifications(model);
                    model.Body = model.Body + " ID: " + notificationId.ToString();
                    NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(notificationHubConnectionString, notificationHubName);
                    try
                    {
                        SendAppleNotificationAsync(model.Subject, model.Body, hub);
                        resultValid = true;
                        errormessage += "<p>Apple Notification Sent</p>";
                    }
                    catch (Exception ex)
                    {
                        resultValid = false;
                        errormessage += "<p>Error in sending Apple Notification.</p>";
                        errormessage += "<p>" + ex.InnerException.Message + "</p>";
                    }
                    try
                    {
                        resultValid = true;
                        SendAndroidNotificationAsync(model.Subject, model.Body, hub);
                        errormessage += "<p>Android Notification Sent</p>";
                    }
                    catch (Exception ex)
                    {
                        resultValid = false;
                        errormessage += "<p>Error in sending Android Notification.</p>";
                        errormessage += "<p>" + ex.InnerException.Message + "</p>";
                    }
                    try
                    {
                        resultValid = true;
                        SendWPNotificationAsync(model.Subject, model.Body, hub);
                        errormessage += "<p>Windows Notification Sent</p>";
                    }
                    catch (Exception ex)
                    {
                        resultValid = false;
                        errormessage += "<p>Error in sending Windows Notification.</p>";
                        errormessage += "<p>" + ex.InnerException.Message + "</p>";
                    }
                  //  if (resultValid)
                  //  { }
                        
                }
                ViewBag.Message = MvcHtmlString.Create(errormessage);
            }
            ViewBag.AppleConfig = FillDropDownItems();
            return View(model);
        }


        [System.Web.Mvc.HttpPost]
        public long SaveNotifications(PushNotification model)
        {
            long result = -1;
            var message = "{\"data\":{\"subject\":\"" + model.Subject + "\",\"content\":\"" + model.Body + "\"}}"; 
            var context = new EventAppDataModelEntity();
            var notificationAdd = new Models.Notification() { Message = @model.Body, Title = @model.Subject, CreatedOn = DateTime.UtcNow };
            context.Notifications.Add(notificationAdd);
            context.SaveChanges();
            result = notificationAdd.NotificationId;
            return result;
           // return result;
        }

        
        public string PushNotification()
        {
            string html = @"<p>Hi,</P>";
            html = html + @"<p>Push Notification has been sent.</P>";

            return html;
        }
        private static void SendAppleNotificationAsync(string subject, string content, NotificationHubClient hub)
        {
            //NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(NOTIFICATION_HUB_FULL_ACCESS_CONNECTION_STRING, NOTIFICATION_HUB_NAME);
            var alert = "{\"aps\":{\"alert\":\"" + subject + "\",\"content\":\"" + content + "\",\"badge\":1,\"sound\":\"bingbong.aiff\"}}";
            hub.SendAppleNativeNotificationAsync(alert).Wait(500);
        }

        private static void SendAndroidNotificationAsync(string subject, string content, NotificationHubClient hub)
        {
            //NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(NOTIFICATION_HUB_FULL_ACCESS_CONNECTION_STRING, NOTIFICATION_HUB_NAME);
            var alert = "{\"data\":{\"subject\":\"" + subject + "\",\"content\":\"" + content + "\"}}";
            hub.SendGcmNativeNotificationAsync(alert).Wait(500);
        }

        private static void SendWPNotificationAsync(string subject, string content, NotificationHubClient hub)
        {
            //NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(NOTIFICATION_HUB_FULL_ACCESS_CONNECTION_STRING, NOTIFICATION_HUB_NAME);

            string toast = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                           "<wp:Notification xmlns:wp=\"WPNotification\">" +
                           "<wp:Toast>" +
                           "<wp:Text1>Playbook Notification</wp:Text1>" +
                           "<wp:Text2>" + subject + " " + content + "</wp:Text2>" +
                           "</wp:Toast> " +
                           "</wp:Notification>";

            var tile = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                       "<wp:Notification xmlns:wp=\"WPNotification\" Version=\"2.0\">" +
                       "<wp:Tile Template=\"FlipTile\">" +
                       "<wp:Title>Playbook</wp:Title>" +
                       "<wp:BackTitle>Playbook !</wp:BackTitle>" +
                       "<wp:Count>!</wp:Count>" +
                        "<wp:BackContent>" + subject + Environment.NewLine + content + "</wp:BackContent>" +
                        "<wp:WideBackContent>" + subject + Environment.NewLine + content + "</wp:WideBackContent>" +
                       "</wp:Tile> " +
                       "</wp:Notification>";
            try
            {
                tile.Trim().Trim(new char[] { '\uFEFF' });
            }
            catch (Exception)
            {
            }
            try
            {
                toast.Trim().Trim(new char[] { '\uFEFF' });
            }
            catch (Exception)
            {
            }
            try
            {
                hub.SendMpnsNativeNotificationAsync(tile).Wait(500);
                hub.SendMpnsNativeNotificationAsync(toast).Wait(500);
            }
            catch (Exception)
            { }
            try
            {
                hub.SendWindowsNativeNotificationAsync(tile).Wait(500);
                hub.SendWindowsNativeNotificationAsync(toast).Wait(500);
            }
            catch (Exception)
            { }
        }
    }
}
