using EventAppServices.Models;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace EventAppServices.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    public class SpeakersController : ApiController
    {
        // GET api/<controller>
        [HttpGet]
        [ActionName("GetSpeakerList")]
        public IHttpActionResult GetSpeakerList()
        
        {
            using (var context = new EventAppDataModelEntity())
            {
                var speakerList = (from b in context.Speakers
                                   select new SpeakerDTO()
                                   {
                                       SpeakerId = b.SpeakerId,
                                       FirstName = b.FirstName,
                                       LastName = b.LastName,
                                       Intro = b.Intro,
                                       Description = b.Description,
                                       Image = b.Image
                                   }).Where(e => e.SpeakerId != 99).ToList();
                return Ok(speakerList);
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