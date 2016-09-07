using EventAppServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace EventAppServices.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TreasureHuntController : ApiController
    {
        // GET api/<controller>
        public THClueDetails GetCluesByUserId(int userId, int tHClueCategoryId)
        {
            List<THClueDetails> tHClueDetailsList = new List<THClueDetails>();
            const int EmpIDLength = 5;
            var empId = userId.ToString().PadLeft(EmpIDLength, '0');
            using (var context = new EventAppDataModelEntity())
            {

                var userDetails = (from t in context.THTeams
                                   join tc in context.THTeamClues on t.THTeamId equals tc.THTeamID
                                   join c in context.THClues on tc.THClueID equals c.THClueId
                                   join cc in context.THClueCategories on c.THClueCategoryId equals cc.THClueCategoryId
                                   join u in context.Users on t.THTeamId equals u.THTeamId
                                   join l in context.Locations on t.LocationId equals l.LocationId

                                   where u.EmpID == empId && cc.THClueCategoryId == tHClueCategoryId
                                   select new
                                   {
                                       THTeamID = t.THTeamId,
                                       THTeamDescription = t.THTeamDescription,
                                       THClueID = c.THClueId,
                                       THClueDescription = c.THCluesDescription,
                                       IsValid = tc.IsValid,
                                       UserName = u.FirstName + " " + u.LastName,
                                       LocationDescription = l.Name,
                                       ThRiddle = c.THRiddle,
                                       MemberNames = t.MemberNames 
                                   }).FirstOrDefault();
                THClueDetails tHclueDetails = new THClueDetails() { IsValid = false, UserName = "Not Found", LocationDescription = string.Empty, THClueDescription = string.Empty, THTeamDescription = string.Empty };
                if (userDetails != null)
                {
                    tHclueDetails.THTeamID = userDetails.THTeamID;
                    tHclueDetails.THTeamDescription = userDetails.THTeamDescription;
                    tHclueDetails.THClueID = userDetails.THClueID;
                    tHclueDetails.THClueDescription = userDetails.THClueDescription;
                    tHclueDetails.IsValid = (bool)userDetails.IsValid;
                    tHclueDetails.LocationDescription = userDetails.LocationDescription;
                    tHclueDetails.UserName = userDetails.UserName;
                    tHclueDetails.THRiddle = userDetails.ThRiddle;
                    tHclueDetails.MembersName = userDetails.MemberNames;
                }
                tHClueDetailsList.Add(tHclueDetails);
            }

            return tHClueDetailsList.FirstOrDefault();
        }

        [HttpPost]
        // GET: api/Sessions/5
        [ResponseType(typeof(OutputResult))]
        public IHttpActionResult UpdateTHClueDetails([FromBody]THClueDetailsSubmitDTO tHClueDetailsSubmitDTO)
        {
            var result = new OutputResult() { Message = "Failure", IsValid = "False" };

            using (var context = new EventAppDataModelEntity())
            {
                var answer = (from s in context.THClues
                              where s.THClueId == tHClueDetailsSubmitDTO.THClueID && s.THClueCategoryId == tHClueDetailsSubmitDTO.THClueCategoryID
                              select s
                                   ).FirstOrDefault();
                try
                {
                    if (answer != null)
                    {
                        if ((string.IsNullOrEmpty(answer.THAnswer) && string.IsNullOrEmpty(tHClueDetailsSubmitDTO.Answer))
                            ||
                            (!string.IsNullOrEmpty(tHClueDetailsSubmitDTO.Answer) && !string.IsNullOrEmpty(answer.THAnswer) 
                            && answer.THAnswer.ToLower() == tHClueDetailsSubmitDTO.Answer.ToLower()))
                        {
                            var clueUpdateRecord = context.THTeamClues.Where(e => e.THClueID == tHClueDetailsSubmitDTO.THClueID
                        && e.THTeamID == tHClueDetailsSubmitDTO.THTeamID).FirstOrDefault();
                            clueUpdateRecord.IsValid = true;
                            context.SaveChanges();
                            result.Message = "Success";
                            result.IsValid = "True";
                            return Ok(result);
                        }
                    }
                    return Ok(result);
                }
                catch (Exception)
                {

                    return Ok(result);
                }
            }
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
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