using EventAppServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace EventAppServices.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class QuizController : ApiController
    {
        private EventAppDataModelEntity db = new EventAppDataModelEntity();
      
        //[ResponseType(typeof(PlayerDTO))]
        //public List<PlayerDTO> GetTopPlayersInfo(int playerCount)
        //{
        //    List<PlayerDTO> playerDto = new List<PlayerDTO>();
        //    var playerList = db.PlayerScores.OrderBy(e => e.CummulativeScore ).ToList();
        //    int incrementCount = 0;
        //    foreach (var player in playerList)
        //    {
        //        var userDetails = db.Users.Where(e => e.EmpID == player.EmpId).FirstOrDefault();
        //        playerDto.Add(new PlayerDTO()
        //        {
        //            EmpId = player.EmpId,
        //            Location = (player.Location != null) ? player.Location.Name : string.Empty,
        //            PlayerName = (userDetails != null) ? userDetails.FirstName + " " + userDetails.LastName : string.Empty,
        //            Quiz1Score = player.Quiz1Score.ToString(),
        //            Quiz2Score = player.Quiz2Score.ToString(),
        //            Quiz3Score = player.Quiz3Score.ToString(),
        //            Quiz4Score = player.Quiz4Score.ToString(),
        //            CommulativeScore = player.CummulativeScore.GetValueOrDefault().ToString(),
        //            LeadershipPosition = player.LeadershipPosition.GetValueOrDefault().ToString()
        //        });
        //        incrementCount++;
        //        if (incrementCount >= playerCount)
        //        {
        //            incrementCount = 0;
        //            break;
        //        }

        //    }
        //    return playerDto;
        //}


        [ResponseType(typeof(PlayerDTO))]
        public List<PlayerDTO> GetTopPlayersInfo(int playerCount)
        {
            List<PlayerDTO> playerDto = new List<PlayerDTO>();
             var playerList = db.PlayerScores.GroupBy(e => e.locationID).ToList();
            int incrementCount = 0;
             foreach (var ps in playerList)
            {
                 var psList = ps.OrderBy(e => (e.Quiz1Score + e.Quiz2Score + e.Quiz3Score + e.Quiz4Score)).ToList();

                foreach (var player in psList)
                {
                    var userDetails = db.Users.Where(e => e.EmpID == player.EmpId).FirstOrDefault(); 
                    playerDto.Add(new PlayerDTO()
                    {
                        EmpId = player.EmpId,
                        Location = (player.Location != null) ? player.Location.Name : string.Empty,
                        PlayerName = (userDetails != null) ? userDetails.FirstName + " " + userDetails.LastName : string.Empty,
                        Quiz1Score = player.Quiz1Score.ToString(),
                        Quiz2Score = player.Quiz2Score.ToString(),
                        Quiz3Score = player.Quiz3Score.ToString(),
                        Quiz4Score = player.Quiz4Score.ToString(),
                        CommulativeScore = player.CummulativeScore.GetValueOrDefault().ToString(),
                        LeadershipPosition = player.LeadershipPosition.GetValueOrDefault().ToString()
                    });
                    incrementCount++;
                    if (incrementCount >= playerCount)
                    {
                        incrementCount = 0;
                        break;
                    }
                }   
            }

            return playerDto.OrderBy(e=>e.Location).ToList();
        }


        [ResponseType(typeof(LocationPlayerDTO))]

        public List<LocationPlayerDTO> GetLocationBasedTopPlayersInfo(int playerCount)
        {
            List<LocationPlayerDTO> playerDTO = new List<LocationPlayerDTO>();
            var playerList = db.PlayerScores.GroupBy(e => e.locationID).ToList();
            int incrementCount = 0;
            foreach (var ps in playerList)
            {
                var psList = ps.OrderBy(e => (e.Quiz1Score + e.Quiz2Score + e.Quiz3Score + e.Quiz4Score)).ToList();
                var locationPlayerDTO = new LocationPlayerDTO()
                    {
                        Location = (psList.FirstOrDefault().Location != null) ? psList.FirstOrDefault().Location.Name : string.Empty,
                        PlayerDto = new List<PlayerDTO>()
                    };
                    playerDTO.Add(locationPlayerDTO);

                foreach (var player in psList)
                {
                    var userDetails = db.Users.Where(e => e.EmpID == player.EmpId).FirstOrDefault();
                    locationPlayerDTO.PlayerDto.Add(new PlayerDTO()
                    {
                        EmpId = player.EmpId,
                        PlayerName = (userDetails != null) ? userDetails.FirstName + " " + userDetails.LastName : string.Empty, 
                        Location = (psList.FirstOrDefault().Location != null) ? psList.FirstOrDefault().Location.Name : string.Empty,
                        Quiz1Score = player.Quiz1Score.ToString(),
                        Quiz2Score = player.Quiz2Score.ToString(),
                        Quiz3Score = player.Quiz3Score.ToString(),
                        Quiz4Score = player.Quiz4Score.ToString(),
                        CommulativeScore = player.CummulativeScore.GetValueOrDefault().ToString(),
                        LeadershipPosition = player.LeadershipPosition.GetValueOrDefault().ToString()
                    });
                    
                    incrementCount++;
                    if (incrementCount >= playerCount)
                    {
                        incrementCount = 0;
                        break;
                    }
                }
            }
            return playerDTO;
        }

       [ResponseType(typeof(PlayerDTO))]
       public List<PlayerDTO> GetAllScores()
        {
            List<PlayerDTO> playerDto = new List<PlayerDTO>(); 
            foreach(PlayerScore  ps in db.PlayerScores.ToList())
            {
                var userDetails = db.Users.Where(e => e.EmpID == ps.EmpId).FirstOrDefault();

                playerDto.Add(new PlayerDTO()
                {
                    EmpId = ps.EmpId,
                    Location = (ps.Location != null) ? ps.Location.Name : string.Empty,
                    PlayerName = (userDetails != null) ? userDetails.FirstName + " " + userDetails.LastName : string.Empty,
                    Quiz1Score = ps.Quiz1Score.ToString(),
                    Quiz2Score = ps.Quiz2Score.ToString(),
                    Quiz3Score = ps.Quiz3Score.ToString(),
                    Quiz4Score = ps.Quiz4Score.ToString(),
                    CommulativeScore = ps.CummulativeScore.GetValueOrDefault().ToString(),
                    LeadershipPosition = ps.LeadershipPosition.GetValueOrDefault().ToString()
                });
            }
            return playerDto;
        }



          [ResponseType(typeof(PlayerDTO))]
      
        public PlayerDTO  GetScoresbyUserId(string userId)
        {
            
           PlayerDTO  playerDto = new  PlayerDTO (); 
            var ps = db.PlayerScores.Where(e => e.EmpId == userId).FirstOrDefault(); 
            var userDetails = db.Users.Where(e => e.EmpID == userId).FirstOrDefault();
            if (ps != null)
            {
                playerDto = new PlayerDTO()
                {
                    EmpId = (userDetails != null) ? userDetails.EmpID.ToString() : string.Empty,
                    Location = (ps.Location != null) ? ps.Location.Name : string.Empty,
                    PlayerName = (userDetails != null) ? userDetails.FirstName + " " + userDetails.LastName : string.Empty,
                    Quiz1Score = ps.Quiz1Score.ToString(),
                    Quiz2Score = ps.Quiz2Score.ToString(),
                    Quiz3Score = ps.Quiz3Score.ToString(),
                    Quiz4Score = ps.Quiz4Score.ToString(),
                    CommulativeScore = ps.CummulativeScore.GetValueOrDefault().ToString(),
                    LeadershipPosition = ps.LeadershipPosition.GetValueOrDefault().ToString()
                };

            }
            return playerDto;
        }
        [ResponseType(typeof(PlayerDTO))]
        public List<LeadershipDTO> GetLeadershipInfo()
        {
            List<LeadershipDTO> leadershipDto = new List<LeadershipDTO>();
            leadershipDto.Add(new LeadershipDTO() { LeadershipPosition = "1", Location = "Hyd", Score = "50", UserName = "Player1" });
            leadershipDto.Add(new LeadershipDTO() { LeadershipPosition = "2", Location = "Pune", Score = "150", UserName = "Player1" });
            leadershipDto.Add(new LeadershipDTO() { LeadershipPosition = "2", Location = "Bglr", Score = "150", UserName = "Player1" });

            return leadershipDto;
        }

        [ResponseType(typeof(string))]
        public List<string> Geturl(string key)
        {
            var returnUrl = new List<string>();
            var keyValue = db.AppConfigs.Where(a => a.AppConfigKey.Equals(key)).FirstOrDefault();
            if (keyValue != null)
            {
                returnUrl.Add(keyValue.AppConfigValue.ToString());
            }
            return returnUrl;
        }
    }
}
