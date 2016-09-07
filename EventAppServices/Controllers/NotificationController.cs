using EventAppServices.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Globalization;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace EventAppServices.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class NotificationController : ApiController
    {
        private EventAppDataModelEntity db = new EventAppDataModelEntity();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
         

        [ResponseType(typeof(NotificationOutput))]
        [HttpGet]

        [ActionName("GetNotificationsAfterbyDate")]
        public List<NotificationOutput> GetNotificationsAfter(DateTime lastRetrievedDateTime)
        {
            var getNotifications = new List<NotificationModel>();
            var nOutput = new List<NotificationOutput>();
             using (var context = new EventAppDataModelEntity())
            {
                getNotifications = (from b in context.Notifications.Where(e => e.CreatedOn > lastRetrievedDateTime).OrderByDescending(e=>e.NotificationId)
                                    select new NotificationModel()
                                    {
                                        Id = b.NotificationId,
                                        content = b.Message,
                                        title = b.Title,
                                        sentAt = b.CreatedOn
                                    }).ToList();

                foreach (NotificationModel nm in getNotifications)
                {
                    nOutput.Add(new NotificationOutput()
                    {
                        content = nm.content + " ID: " + nm.Id.ToString(),
                        title = nm.title ,
                        sentAt = TimeZoneInfo.ConvertTimeFromUtc(nm.sentAt.GetValueOrDefault(), INDIAN_ZONE).ToString("dd MMM yyyy hh:mm tt")
                     
                    });
                }
            }
            return nOutput;
        }


        [ResponseType(typeof(NotificationOutput))]
        [HttpGet]
        [ActionName("GetNotificationsAfter")]
        public List<NotificationOutput> GetNotificationsAfter(int lastRetrievedNotificationId)
        {
            var getNotifications = new List<NotificationModel>();
            var nOutput = new List<NotificationOutput>();

            using (var context = new EventAppDataModelEntity())
            {
                getNotifications = (from b in context.Notifications.Where(e => e.NotificationId > lastRetrievedNotificationId).OrderByDescending(e => e.NotificationId)
                                    select new NotificationModel()
                                    {
                                        Id = b.NotificationId,
                                        content = b.Message,
                                        title = b.Title,
                                        sentAt = b.CreatedOn
                                    }).ToList();

                foreach (NotificationModel nm in getNotifications)
                {
                    nOutput.Add(new NotificationOutput()
                    {
                        content = nm.content + " ID: " + nm.Id.ToString(),
                        title = nm.title,
                        sentAt = TimeZoneInfo.ConvertTimeFromUtc(nm.sentAt.GetValueOrDefault(), INDIAN_ZONE).ToString("dd MMM yyyy hh:mm tt")
                    });

                }
            }
            return nOutput;
        }
    }
}