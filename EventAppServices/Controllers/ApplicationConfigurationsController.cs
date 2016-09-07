using EventAppServices.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace EventAppServices.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ConfigurationsController : ApiController
    {
        private EventAppDataModelEntity db = new EventAppDataModelEntity();

        // GET: api/ApplicationConfigurations
        public IQueryable<AppConfig> GetAppConfigs()
        {
            return db.AppConfigs;
        }

        [HttpGet]
        public string GetConfigvalue(string Key)
        {
            var keyValue = (from a in db.AppConfigs where a.AppConfigKey.Equals(Key) select a.AppConfigValue).FirstOrDefault();
              return keyValue.ToString(); 
        }

        [HttpGet]
        [ResponseType(typeof(StreamResult))]
        public StreamResult GetStreamInfo()
        {
            var streamresult = new StreamResult() { };
            var keyValueStreamUrl = db.AppConfigs.Where(e => e.AppConfigKey == "STREAMURL").FirstOrDefault().AppConfigValue;
            var keyValueStreamUrlTime = db.AppConfigs.Where(e => e.AppConfigKey == "STREAMURLTIME").FirstOrDefault().AppConfigValue;
            streamresult.url = keyValueStreamUrl;
            streamresult.startsAt = keyValueStreamUrlTime;
            return streamresult;
        }

        // GET: api/ApplicationConfigurations/5
        [ResponseType(typeof(AppConfig))]
        public IHttpActionResult GetAppConfig(long id)
        {
            AppConfig appConfig = db.AppConfigs.Find(id);
            if (appConfig == null)
            {
                return NotFound();
            }

            return Ok(appConfig);
        }

        // PUT: api/ApplicationConfigurations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAppConfig(long id, AppConfig appConfig)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != appConfig.AppConfigId)
            {
                return BadRequest();
            }

            db.Entry(appConfig).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppConfigExists(id))
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

        // POST: api/ApplicationConfigurations
        [ResponseType(typeof(AppConfig))]
        public IHttpActionResult PostAppConfig(AppConfig appConfig)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AppConfigs.Add(appConfig);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AppConfigExists(appConfig.AppConfigId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = appConfig.AppConfigId }, appConfig);
        }

        // DELETE: api/ApplicationConfigurations/5
        [ResponseType(typeof(AppConfig))]
        public IHttpActionResult DeleteAppConfig(long id)
        {
            AppConfig appConfig = db.AppConfigs.Find(id);
            if (appConfig == null)
            {
                return NotFound();
            }

            db.AppConfigs.Remove(appConfig);
            db.SaveChanges();

            return Ok(appConfig);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AppConfigExists(long id)
        {
            return db.AppConfigs.Count(e => e.AppConfigId == id) > 0;
        }
    }
}