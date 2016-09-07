using EventAppServices.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Web.Script.Serialization;

namespace EventAppServices.Controllers
{
     [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RateSessionsController : ApiController
    {
        private EventAppDataModelEntity db = new EventAppDataModelEntity();
        const int EmpIDLength = 5;
           
        // GET api/<controller>
        public List<RateSessionsDTO> Get()
        {
            using (var context = new EventAppDataModelEntity())
            {
                var rateSession = (from b in context.UserReviews
                          select new RateSessionsDTO()
                          {
                              UserId = b.UserId,
                              SessionId = b.SessionId,
                              Rating = b.Rating,
                              Comments = b.Comments
                          }).ToList();

                return rateSession; 
            }
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost] 
        [ActionName("RateSession")]
        public string RateSession(UserReview userReview) 
        { 
            var outputMessage = "Successfully Rated";
            try
            {
                if (!ModelState.IsValid)
                {
                    return "BadRequest";
                }
               
                var empId = userReview.UserId.ToString().PadLeft(EmpIDLength,'0'); 
                var user = db.Users.Where(e => e.EmpID == empId).FirstOrDefault(); 
                var sessions = db.Sessions.Where(e => e.SessionId == userReview.SessionId).FirstOrDefault(); 
                if ( (user != null) && (sessions !=null))
                {
                    var userReviews = db.UserReviews.Where(e => e.UserId == user.UserId && e.SessionId == userReview.SessionId).FirstOrDefault();
                    if (userReviews == null)
                    {
                        userReview.UserId = user.UserId; 
                        db.UserReviews.Add(userReview);
                    }
                    else
                    {
                        userReviews.UserId = user.UserId;
                        userReviews.Rating = userReview.Rating;
                        userReviews.Comments = userReview.Comments;
                        outputMessage = "Successfully Updated";
                    }
                }
                else
                { outputMessage = "User / Session Not Found"; }
                db.SaveChanges();
            }
            catch (Exception ex)
            { outputMessage = "Failed. - " + ex.Message; }
            return  outputMessage ;
        } 

        // POST api/<controller>
        [HttpPost]
        [ResponseType(typeof(UserReview))]
        [ActionName("RateApplication")]
        public IHttpActionResult RateApplication(ApplicationReviews appReview)
        {
            var outputMessage = "Successfully Registered";
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
              //  var user = db.Users.Where(e => e.UserId == appReview.UserId).FirstOrDefault();
                var empId = appReview.UserId.ToString().PadLeft(EmpIDLength, '0'); ;
                 var user = db.Users.Where(e => e.EmpID == empId).FirstOrDefault();

                var sessions = db.Sessions.Where(e => e.Description == "Application").FirstOrDefault();
                if ((user != null) && (sessions != null))
                {
                    var userReviews = db.UserReviews.Where(e => e.UserId == user.UserId && e.SessionId == sessions.SessionId).FirstOrDefault();
                    var mailSender = new Mailer();
                    var mailBody = string.Format("User : {0} {1} <br> Comments: {2}", user.FirstName, user.LastName ,   appReview.Comments );
                    var mailModel = new MailModel() { Body = mailBody, Subject = "TechVantage 2016 App Feedback." };
                    mailSender.SendEmail(mailModel); 
                    if (userReviews == null)
                    {
                        UserReview userReview = new UserReview() { UserId = user.UserId, SessionId = sessions.SessionId, Comments = appReview.Comments }; 
                        db.UserReviews.Add(userReview);
                    }
                    else
                    {
                        userReviews.UserId = user.UserId;
                        userReviews.Comments = appReview.Comments;
                        outputMessage = "Successfully Updated";
                    }
                }
                else
                { outputMessage = "User / Session Not Found"; }
                db.SaveChanges();
            }
            catch (Exception ex)
            { outputMessage = "Failed. - " + ex.Message; }
            return Ok(outputMessage);
        }
        [HttpGet]
        public IHttpActionResult ReportForRate()
        {
            DateTime dateStartTime = DateTime.MinValue;
            List<ReportForRates> uSessionRateReport = new List<ReportForRates>();
            using (var context = new EventAppDataModelEntity())
            {
                var reprtSessionRate     = (from b in context.UserReviews
                                                       join u in context.Users on b.UserId equals u.UserId
                                                       join s in context.Sessions on b.SessionId equals s.SessionId
                                                       select new 
                                                       {
                                                           UserId = b.UserId,
                                                           SessionId = b.SessionId,
                                                           Rating = b.Rating,
                                                           FirstName = u.FirstName,
                                                           LastName = u.LastName,
                                                           Date = s.StartDate,
                                                           Time = s.StartTime
                                                       }).ToList();

                foreach (var i in reprtSessionRate)
                {
                    ReportForRates rFS = new ReportForRates();
                    rFS.UserId = i.UserId;
                    rFS.SessionId = i.SessionId;
                    rFS.Rating = i.Rating;
                    rFS.FirstName = i.FirstName;
                    rFS.LastName = i.LastName;
                    rFS.Date = i.Date.ToString("dd MMM yyyy");

                    rFS.Time = dateStartTime.Add(i.Time).ToString("hh:mm tt", CultureInfo.InvariantCulture);

                    uSessionRateReport.Add(rFS);
                }

                return Ok(uSessionRateReport);
            }
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}