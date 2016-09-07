using EventAppServices.Models;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace EventAppServices.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TagsController : ApiController
    {
        // GET api/<controller>
        public IHttpActionResult Get()
        {
            using (var context = new EventAppDataModelEntity())
            {

                var tags = (from t in context.Tags
                            join td in context.TagDetails on t.TagId equals td.TagId

                            where t.TagId == td.TagId
                          
                           select new TagDTO()
                           {
                              TagId = t.TagId,
                              Title = t.Title,
                              Description = t.Description,
                              SessionId = td.SessionId,
                              SessionCount = t.TagDetails.Where(e=>e.SessionId == td.SessionId).Count()
                          }).ToList();

             
                return Ok(tags);

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