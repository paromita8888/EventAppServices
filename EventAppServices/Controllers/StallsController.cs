using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using EventAppServices.Models;
using System.Web.Http.Cors;


namespace EventAppServices.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class StallsController : ApiController
    {
        // GET api/<controller>
        public IHttpActionResult Get()
        {
            
            using (var context = new EventAppDataModelEntity())
            {
                var st = (from b in context.Stalls.Where(e=>e.StallId !=99).ToList()
                          select new StallDTO()
                          {
                              StallId = b.StallId,
                              Title = b.Title,
                              Description = b.Description,
                              BeaconId = b.BeaconId ?? 0

                          }).ToList();


                return Ok(st);               
               
                
            }
           
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
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