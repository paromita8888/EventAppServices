using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using EventAppServices.Models;

namespace EventAppServices.Controllers
{
    public class UserAttendancesController : ApiController
    {
        private EventAppDataModelEntity db = new EventAppDataModelEntity();

        [HttpPost]
        [ActionName("UserAttendance")]
        public string UserAttendance(UserAttendanceDTO userAttendanceDTO)
        {
            var outputMessage = "Attendance Successfully Registered";
            try
            {
                const int EmpIDLength = 5;
                var empId = userAttendanceDTO.EmpID.ToString().PadLeft(EmpIDLength, '0');
                var userAttendanceExists = db.UserAttendances.Where(e => e.User.EmpID == empId && e.SessionID == userAttendanceDTO.SessionID 
                    && e.StallID == userAttendanceDTO.StallID).FirstOrDefault();
                var user = db.Users.Where(e => e.EmpID == empId).FirstOrDefault();

                if (user == null)
                {
                    outputMessage = "User Doesnt Exists";
                    return outputMessage;
                }
                if (userAttendanceExists == null)
                {
                  var  newUser = new UserAttendance()
                    {
                        UserID = user.UserId,
                        SessionID = userAttendanceDTO.SessionID ,
                        StallID = userAttendanceDTO.StallID ,
                        IsAttendee = userAttendanceDTO.IsAttendee,
                        Createdon = DateTime.UtcNow
                    };
                  db.UserAttendances.Add(newUser);
                  outputMessage = "User Attendance Added.";
                    
                }
                else
                {
                    userAttendanceExists.IsAttendee = userAttendanceDTO.IsAttendee;
                    outputMessage = "User Attendance Updated";
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            { outputMessage = "Attendance Registration Failed."; }
            return outputMessage;
        }

        [HttpGet]
        [ResponseType(typeof(UserAttendanceDTO))]
        [ActionName("GetUserAttendance")]
        public List<UserAttendanceDTO> GetUserAttendance()
        {
            var userAttendanceDTO = new List<UserAttendanceDTO>();
            var outputMessage = "Successfully";
            try
            {
                var userAttendanceExists = db.UserAttendances.OrderBy(e=>e.User.EmpID).OrderBy(e=>e.SessionID).OrderBy(e=>e.StallID).ToList();
                foreach (UserAttendance  ud in userAttendanceExists)
                {
                    userAttendanceDTO.Add(new UserAttendanceDTO()
                    {
                        EmpID = ud.User.EmpID,
                        UserID = ud.UserID,
                        SessionID = ((ud.SessionID !=99)? ud.SessionID : 0),
                        StallID = ((ud.StallID != 99) ? ud.StallID : 0),
                        SessionName =((ud.SessionID !=99) ? ud.Session.Title : string.Empty ),
                        StallName =  ((ud.StallID != 99) ?ud.Stall.Title : string.Empty ),
                        IsAttendee = ud.IsAttendee
                    });
                
                } 
            }
            catch (Exception ex)
            { outputMessage = "Error"; }
            return userAttendanceDTO;
        }


        [HttpGet]
        [ResponseType(typeof(UserAttendanceDTO))]
        [ActionName("GetUserAttendancebyEmpId")]
        public List<UserAttendanceDTO> GetUserAttendancebyEmpId(string EmpId)
        {
            var userAttendanceDTO = new List<UserAttendanceDTO>();
            try
            {
                userAttendanceDTO = GetUserAttendance().Where(e => e.EmpID == EmpId).ToList();
            }
            catch (Exception ex)
            { }
            return userAttendanceDTO;
        }
        [HttpGet]
        [ResponseType(typeof(UserAttendanceSessionDTO))]
        [ActionName("EventReport")]
        public List<UserAttendanceSessionDTO> EventReport()
        {
            var userAttendanceDTO = new List<UserAttendanceDTO>();
            var userAttendanceSessionDTO = new List<UserAttendanceSessionDTO>();
            try
            {

                var userAttendanceSession = db.UserAttendances.Where(e => e.SessionID == 99 && e.StallID ==99).GroupBy(e => e.SessionID).ToList();
                foreach (var u in userAttendanceSession)
                {
                    userAttendanceSessionDTO.Add(
                      new UserAttendanceSessionDTO()
                      {
                          Count = u.Count(),
                          SessionID = u.FirstOrDefault().SessionID,
                          SessionName = u.FirstOrDefault().Session.Description
                      });
                }
                return userAttendanceSessionDTO;
            }
            catch (Exception ex)
            { }
            return userAttendanceSessionDTO;
        }

        [HttpGet]
        [ResponseType(typeof(UserAttendanceSessionDTO))]
        [ActionName("SessionReport")]
        public List<UserAttendanceSessionDTO> SessionReport()
        {
            var userAttendanceDTO = new List<UserAttendanceDTO>();
            var userAttendanceSessionDTO = new List<UserAttendanceSessionDTO>();
            try
            {

                var userAttendanceSession = db.UserAttendances.Where(e => e.SessionID != 99).OrderBy(e => e.SessionID).GroupBy(e => e.SessionID).ToList();
                foreach (var u in userAttendanceSession)
                {
                    userAttendanceSessionDTO.Add(
                      new UserAttendanceSessionDTO()
                      {
                          Count = u.Count(),
                          SessionID = u.FirstOrDefault().SessionID,
                          SessionName = u.FirstOrDefault().Session.Title 
                      });
                }
                return userAttendanceSessionDTO;
            }
            catch (Exception ex)
            {   }
            return userAttendanceSessionDTO;
        }

        [HttpGet]
        [ResponseType(typeof(UserAttendanceStallDTO))]
        [ActionName("StallReport")]
        public List<UserAttendanceStallDTO> StallReport()
        {
            var userAttendanceDTO = new List<UserAttendanceDTO>();
            var userAttendanceStallDTO = new List<UserAttendanceStallDTO>();
            try
            {
                var userAttendanceStall = db.UserAttendances.Where(e=>e.StallID !=99 && e.IsAttendee == true).OrderBy(e => e.StallID).GroupBy(e => e.StallID).ToList(); 
                foreach (var u in userAttendanceStall)
                {
                    userAttendanceStallDTO.Add(
                      new UserAttendanceStallDTO()
                      {
                          Count = u.Count(),
                          StallID =  u.FirstOrDefault().StallID ,
                          StallName = u.FirstOrDefault().Stall.Title  
                      });
                }
                return userAttendanceStallDTO.OrderByDescending(e=>e.Count).ToList();
            }
            catch (Exception ex)
            { }
            return userAttendanceStallDTO;
        }
        // GET: api/UserAttendances
        public IQueryable<UserAttendance> GetUserAttendances()
        {
            return db.UserAttendances;
        }

        // GET: api/UserAttendances/5
        [ResponseType(typeof(UserAttendance))]
        public IHttpActionResult GetUserAttendance(long id)
        {
            UserAttendance userAttendance = db.UserAttendances.Find(id);
            if (userAttendance == null)
            {
                return NotFound();
            }

            return Ok(userAttendance);
        }

        // PUT: api/UserAttendances/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUserAttendance(long id, UserAttendance userAttendance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userAttendance.UserID)
            {
                return BadRequest();
            }

            db.Entry(userAttendance).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserAttendanceExists(id))
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

        // POST: api/UserAttendances
        [ResponseType(typeof(UserAttendance))]
        public IHttpActionResult PostUserAttendance(UserAttendance userAttendance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserAttendances.Add(userAttendance);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (UserAttendanceExists(userAttendance.UserID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = userAttendance.UserID }, userAttendance);
        }

        // DELETE: api/UserAttendances/5
        [ResponseType(typeof(UserAttendance))]
        public IHttpActionResult DeleteUserAttendance(long id)
        {
            UserAttendance userAttendance = db.UserAttendances.Find(id);
            if (userAttendance == null)
            {
                return NotFound();
            }

            db.UserAttendances.Remove(userAttendance);
            db.SaveChanges();

            return Ok(userAttendance);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserAttendanceExists(long id)
        {
            return db.UserAttendances.Count(e => e.UserID == id) > 0;
        }



        [HttpGet]
        [ResponseType(typeof(UserDTO))]
        public IHttpActionResult GetEventAttandance()
        {
            var photoUrl = db.AppConfigs.Where(e => e.AppConfigKey == "PHOTOURL").FirstOrDefault().AppConfigValue;

            List<UserDTO> resultUser = new List<UserDTO>();
            var userAttendance = db.UserAttendances.Where(e => e.StallID == 99 && e.SessionID == 99 && e.IsAttendee == true).OrderByDescending(e => e.Createdon).ToList();

            var registeredUsers = db.Users.OrderByDescending(e => e.CreatedOn).ToList();

            foreach (var u in userAttendance)
            {
                resultUser.Add(new UserDTO()
                {
                    PhotoUrl = @photoUrl + u.User.EmpID + ".gif",
                    EmpId = u.User.EmpID,
                    Name = u.User.FirstName + " " + u.User.LastName
                });
            }
            return Ok(resultUser);
        }

    }
}