using EventAppServices.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace EventAppServices.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        private EventAppDataModelEntity db = new EventAppDataModelEntity();
        [HttpPost]
        [ActionName("RegisterUser")]
        public string RegisterUser(ValidateUserDTO validateUser)
        {
            User newUser=new User();
            var outputMessage = "Successfully Registered";
            try
            { 
                var userExists = db.Users.Where(e=>e.EmpID == validateUser.EmpId ).FirstOrDefault(); 
                if (userExists == null)
                {
                    newUser = new User()
                    {
                        FirstName = validateUser.FirstName,
                        LastName = validateUser.LastName,
                        EmailId =  validateUser.EmailId,
                        UserId = Convert.ToInt64(validateUser.EmpId),
                        EmpID = validateUser.EmpId.PadLeft(5, '0'),
                        CreatedOn = DateTime.Now.ToLocalTime()
                        
                    };
                    db.Users.Add(newUser);
                    db.SaveChanges();
                }
                else
                {
                    outputMessage = "User Exists";
                }
            }
            catch (Exception ex)
            { outputMessage = "Registration Failed."; }
            return outputMessage;
        }
       
        [HttpGet] 
         [ResponseType(typeof(ValidateUserDTO))]
        public IHttpActionResult GetTopUsers()
        {
            var firstLoginCount = db.AppConfigs.Where(e => e.AppConfigKey == "FIRSTLOGINCOUNT").FirstOrDefault().AppConfigValue;
            var topFirstLoginCount = db.AppConfigs.Where(e => e.AppConfigKey.Equals("PICKFIRSTLOGINCOUNT")).FirstOrDefault().AppConfigValue;

            int pickfirstLoginCount = Convert.ToInt16(firstLoginCount);
            int pickCountOfLogins = Convert.ToInt16(topFirstLoginCount);
            // take random 
            Random randomPick = new Random();
            List<ValidateUserDTO> resultUser = new List<ValidateUserDTO>();
            //Get Top n Users from  top n Logged in Users ==> AppConfig
            var appConfig = (from q in db.Users orderby q.CreatedOn descending
                             select q).Take(pickfirstLoginCount);

            for (int i = 0; i < pickCountOfLogins; i++)
            {
                int randomNum = randomPick.Next(pickfirstLoginCount);
                if (randomNum < appConfig.Count())
                {
                   var pickUser= appConfig.ToList().ElementAt(randomNum);
                   resultUser.Add(new ValidateUserDTO() { EmpId = pickUser.EmpID, EmailId = pickUser.EmpID , FirstName = pickUser.FirstName , LastName = pickUser.LastName });
                }
            }
            return Ok(  resultUser) ;
        }

        [HttpGet] 
        [ActionName("GetValidateUser")]

        //public string ValidateUser()
        public ValidateUserDTO GetValidateUser(string UserName, string Password)
        {
            ValidateUserDTO retObject = new ValidateUserDTO();
            List<ValidateUserDTO> lstValidateUser = new List<ValidateUserDTO>();
            try
            {
                UserName = "hitachicc\\" + UserName;
                using (PrincipalContext pCtx = new PrincipalContext(ContextType.Domain, "192.168.192.12", UserName, Password))
                {
                    GroupPrincipal group = GroupPrincipal.FindByIdentity(pCtx, "USERS");
                    var user = UserPrincipal.FindByIdentity(pCtx, UserName);
                    retObject.FirstName = user.GivenName;
                    retObject.LastName = user.Surname;
                    retObject.Name = user.DisplayName;
                   retObject.EmpId = user.EmployeeId;
                    retObject.EmailId  = user.SamAccountName + "@hitachiconsulting.com";
                    lstValidateUser.Add(retObject);


                }
            }
            catch (Exception ex)
            {
                string output = "Message: " + ex.Message + Environment.NewLine + "Stack Trace: " + ex.StackTrace;
            }
            return retObject;
        }


        [HttpGet]
        [ResponseType(typeof(UserDTO))]
        public IHttpActionResult GetRegisteredUsers( )
        {
            var photoUrl = db.AppConfigs.Where(e => e.AppConfigKey == "PHOTOURL").FirstOrDefault().AppConfigValue; 
              
            List<UserDTO> resultUser = new List<UserDTO>();
           var registeredUsers= db.Users.OrderByDescending(e=>e.CreatedOn).ToList();
             foreach (var u in registeredUsers)
           {
               resultUser.Add(new UserDTO()
               { 
                   PhotoUrl = @photoUrl + u.EmpID + ".gif",
                   EmpId = u.EmpID, 
                   Name = u.FirstName + " " + u.LastName
               }); 
           }
            return Ok(resultUser);
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

        [HttpPost]
       [ResponseType(typeof(OutputResult))]
        public OutputResult ClaimGoody(string EmpId)
        {
            var outputResult = new OutputResult(){ IsValid = "False", Message ="Successfully Goody Claimed."} ;
            User newUser = new User(); 
            try
            {
                const int EmpIDLength = 5;
                var empId = EmpId.ToString().PadLeft(EmpIDLength, '0');

                var userExists = db.Users.Where(e => e.EmpID == empId).FirstOrDefault();
                if (userExists == null)
                {
                    outputResult.Message  = "User does not exists";
                    outputResult.IsValid = "False";
                }
                else
                {
                    if (userExists.ClaimGoody.GetValueOrDefault() == true)
                    {
                        outputResult.Message ="You have already claimed a Goody.";

                          outputResult.IsValid = "False";
                    }
                    else
                    {
                        userExists.ClaimGoody = true;
                        db.SaveChanges();
                        outputResult.Message = string.Format("Goody Claimed by Employee Id {0}", empId);
                        outputResult.IsValid = "True";
                    }
                }
            }
            catch (Exception ex)
            { outputResult.Message = "Goddy Claim Failed."; }
            return outputResult;
        }


    }
}