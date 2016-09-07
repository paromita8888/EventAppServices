using EventAppServices.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace EventAppServices.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SessionsController : ApiController
    {
        private EventAppDataModelEntity db = new EventAppDataModelEntity();

        // GET: api/Sessions
        [ResponseType(typeof(Session))]
        public IHttpActionResult GetSessions()

        {
            string dt = string.Empty;
            List<SessionModel> sessionModelList = new List<SessionModel>();
            SessionModel sessionModel;
            List<SessionDTO> sessionDto = new List<SessionDTO>();
            List<Slots> slotsList = new List<Slots>();
            Slots slots = new Slots();
            SessionDTO sessionTemp = new SessionDTO();
            DateTime dateStartTime = DateTime.MinValue;
            DateTime dateEndTime = DateTime.MinValue;
            var sessionList = db.Sessions.Where(e=>e.SessionId !=99).OrderBy(e => e.StartDate).ToList<Session>();
            foreach (Session s in sessionList)
            {
                if (s.EndTime.HasValue.Equals(true))
                {

                    dateEndTime = Convert.ToDateTime(s.EndTime.ToString());                          //dateEndTime.Add(s.EndTime).ToString("hh:mm tt", CultureInfo.InvariantCulture);
                }

                sessionTemp = new SessionDTO()
                {
                    SessionId = s.SessionId,
                    Description = s.Description,
                    StartDate = s.StartDate.ToString("dd MMM yyyy"),
                    EndDate = s.EndDate.GetValueOrDefault().ToString("dd MMM yyyy"),
                    StartTime = dateStartTime.Add(s.StartTime).ToString("hh:mm tt", CultureInfo.InvariantCulture),
                    EndTime = dateEndTime.ToString("hh:mm tt", CultureInfo.InvariantCulture),
                    Venue = s.Venue,
                    SpeakerId = s.SpeakerId,
                    Title = s.Title,
                };
                sessionTemp.PillarTag = new List<string>();
                foreach (var tag in s.TagDetails)
                {
                    sessionTemp.PillarTag.Add(tag.Tag.Title);
                };

                sessionDto.Add(sessionTemp);

            }
            var sessionGroup = sessionDto.GroupBy(e => e.StartDate).ToList();
            foreach (var item in sessionGroup)
            {
                sessionModel = new SessionModel();
                sessionModel.Date = item.Key.ToString();
                sessionModel.Slots = new List<Slots>();
           //     foreach (var itemSlot in item.OrderBy(e => e.StartTime))
                foreach (var itemSlot in item)
                {
                    Slots addSlots = new Slots() { Time = itemSlot.StartTime, Events = new List<EventSlot>() };
                    EventSlot eventSlots = new EventSlot()
                    {
                        Id = itemSlot.SessionId,
                        EndTime = itemSlot.EndTime,
                        Title = itemSlot.Title,
                        Description = itemSlot.Description,
                        Venue = itemSlot.Venue,
                        SpeakerId = itemSlot.SpeakerId,
                        Tags = itemSlot.PillarTag
                    };
                    addSlots.Events.Add(eventSlots);
                    sessionModel.Slots.Add(addSlots);
                }

                sessionModelList.Add(sessionModel);
            }

            return Ok(sessionModelList);
        }

        [ResponseType(typeof(UserSessionDTO))]
        public IHttpActionResult GetSessionsInterest()
        {
            List<UserSessionDTO> uSession = new List<UserSessionDTO>();
            foreach (UserSession us in db.UserSessions)
            {
                uSession.Add(new UserSessionDTO()
                {
                    SessionID = us.SessionID,
                    UserID = us.UserID,
                    Isinterested = us.Isinterested,
                    //       IsRegistered = us.IsRegistered
                });
            }
            return Ok(uSession);
        }

        [HttpGet]
        public IHttpActionResult ReportForSession()
        {
            DateTime dateStartTime = DateTime.MinValue;
            List<ReportUserSession> reportUserSession = new List<ReportUserSession>();

            using (var context = new EventAppDataModelEntity())
            {
                dateStartTime = DateTime.MinValue;
                var uSessionReport = (from b in context.UserSessions
                                      join u in context.Users on b.UserID equals u.UserId
                                      join s in context.Sessions on b.SessionID equals s.SessionId
                                      where u.UserId == b.UserID && s.SessionId == b.SessionID
                                      select new
                                      {
                                          SessionID = b.SessionID,
                                          UserID = b.UserID,
                                          IsInterested = b.Isinterested,
                                          Date = s.StartDate,
                                          Time = s.StartTime,
                                          FirstName = u.FirstName,
                                          LastName = u.LastName
                                      }).ToList();

                foreach (var u in uSessionReport)
                {
                    ReportUserSession ruS = new ReportUserSession();
                    ruS.SessionID = u.SessionID;
                    ruS.UserID = u.UserID;
                    ruS.IsInterested = u.IsInterested;
                    ruS.Date = u.Date.ToString("dd MMM yyyy");
                    ruS.Time = dateStartTime.Add(u.Time).ToString("hh:mm tt", CultureInfo.InvariantCulture);
                    ruS.FirstName = u.FirstName;
                    ruS.LastName = u.LastName;
                    reportUserSession.Add(ruS);
                }

                return Ok(reportUserSession);
            }
        }

        // GET: api/Sessions/5
        [ResponseType(typeof(Session))]
        public IHttpActionResult GetSession(long id)
        {
            Session session = db.Sessions.Find(id);
            if (session == null)
            {
                return NotFound();
            }

            return Ok(session);
        }

        [HttpPost]
        // GET: api/Sessions/5
        [ResponseType(typeof(UserSessionDTO))]
        public IHttpActionResult UpdateSessionInterest([FromBody]UserSessionDTO userSession)
        {
            User user = db.Users.Where(x => x.EmpID == userSession.EmpID).FirstOrDefault();
            if ((!ModelState.IsValid) || (userSession == null) || (user == null))
            {
                BadRequest(ModelState).Request.ToString();

                return Ok("Failed");
            }
            UserSession session = db.UserSessions.Where(e => e.SessionID == userSession.SessionID
                && e.UserID == user.UserId).FirstOrDefault();

            if (session != null)
            {
                session.Isinterested = userSession.Isinterested;
                //          session.IsRegistered = userSession.IsRegistered;
            }
            else
            {
                session = new UserSession()
                {
                    UserID = user.UserId,
                    SessionID = userSession.SessionID,
                    //         IsRegistered = userSession.IsRegistered,
                    Isinterested = userSession.Isinterested
                };
                db.UserSessions.Add(session);
            }
            try
            {
                db.SaveChanges();
                return Ok("Success");

            }
            catch (Exception ex)
            {
                return Ok("Failure");

            }

        }

        [HttpPost]
        // GET: api/Sessions/5
        [ResponseType(typeof(UserSession))]
        public IHttpActionResult UpdateSessionRegistered(UserSession userSession)
        {
            var session = db.UserSessions.Where(e => e.SessionID == userSession.SessionID
                && e.UserID == userSession.UserID);
            if (session.Any())
            {
                foreach (UserSession uSession in session)
                {
                    uSession.Isinterested = userSession.Isinterested;
                }
            }
            else { db.UserSessions.Add(userSession); }
            if (session == null)
            {
                return NotFound();
            }

            return Ok(session);
        }

        // PUT: api/Sessions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSession(long id, Session session)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != session.SessionId)
            {
                return BadRequest();
            }

            db.Entry(session).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SessionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Sessions
        [ResponseType(typeof(Session))]
        public IHttpActionResult PostSession(Session session)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Sessions.Add(session);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = session.SessionId }, session);
        }

        // DELETE: api/Sessions/5
        [ResponseType(typeof(Session))]
        public IHttpActionResult DeleteSession(long id)
        {
            Session session = db.Sessions.Find(id);
            if (session == null)
            {
                return NotFound();
            }

            db.Sessions.Remove(session);
            db.SaveChanges();

            return Ok(session);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SessionExists(long id)
        {
            return db.Sessions.Count(e => e.SessionId == id) > 0;
        }
    }
}