using System;
using System.Collections.Generic;
using Mogade.Achievements;
using Mogade.Configuration;
using Mogade.Leaderboards;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Mogade
{
   public class Driver : IDriver, IRequestContext
   {      
      public const int VERSION = 1;
      public Driver(string gameKey, string secret)
      {
         ValidationHelper.AssertNotNullOrEmpty(gameKey, "gameKey");
         ValidationHelper.AssertNotNullOrEmpty(secret, "secret");
         Key = gameKey;
         Secret = secret;
      }

      public int ApiVersion
      {
         get { return VERSION; }
      }


      public string Key { get; private set; }
      public string Secret { get; private set; }
      
      public void GetGameVersion(Action<Response<int>> callback)
      {
         var payload = new Dictionary<string, object>(0);
         var communicator = new Communicator(this);
         communicator.SendPayload<int>(Communicator.POST, "conf/version", payload, r =>
         {            
            if (r.Success)
            {
               var container = ((JContainer) JsonConvert.DeserializeObject(r.Raw))["version"];
               r.Data = container == null ? 0 : container.Value<int>();
            }
            callback(r);
         });
      }

      public void GetUserSettings(string userName, string uniqueIdentifier, Action<Response<UserSettings>> callback)
      {
         ValidationHelper.AssertNotNullOrEmpty(userName, 20, "username");
         ValidationHelper.AssertNotNullOrEmpty(uniqueIdentifier, 50, "unique identifier");
         var payload = new Dictionary<string, object> { { "username", userName }, { "unique", uniqueIdentifier } };
         var communicator = new Communicator(this);
         communicator.SendPayload<UserSettings>(Communicator.POST, "conf/my", payload, r =>
         {
            if (r.Success) { r.Data = JsonConvert.DeserializeObject<UserSettings>(r.Raw); }
            callback(r);
         });
         
      }

      public void GetGameConfiguration(Action<Response<GameConfiguration>> callback)
      {         
         var payload = new Dictionary<string, object>(0);
         var communicator = new Communicator(this);
         communicator.SendPayload<GameConfiguration>(Communicator.POST, "conf", payload, r =>
         {
            if (r.Success) { r.Data = JsonConvert.DeserializeObject<GameConfiguration>(r.Raw); }
            callback(r);
         });
      }

      public void SaveScore(string leaderboardId, Score score, string uniqueIdentifier, Action<Response<Ranks>> callback)
      {
         ValidationHelper.AssertValidId(leaderboardId, "leaderboardId");
         ValidationHelper.AssertNotNull(score, "score");
         ValidationHelper.AssertMaximumLength(score.Data, 25, "score data");
         ValidationHelper.AssertNotNullOrEmpty(score.UserName, 20, "score username");
         ValidationHelper.AssertNotNullOrEmpty(uniqueIdentifier, 50, "unique identifier");

         var payload = new Dictionary<string, object>
                       {
                          {"leaderboard_id", leaderboardId}, 
                          {"score", new {username = score.UserName, points = score.Points, data = score.Data, unique = uniqueIdentifier}},
                       };
         var communicator = new Communicator(this);
         communicator.SendPayload<Ranks>(Communicator.PUT, "scores", payload, r =>
         {
            if (r.Success) { r.Data = JsonConvert.DeserializeObject<Ranks>(r.Raw); }
            callback(r);
         });
      }

      public void GetLeaderboard(string leaderboardId, LeaderboardScope scope, int page, Action<Response<LeaderboardScores>> callback)
      {
         GetLeaderboard(leaderboardId, scope, page, 10, callback);
      }

      public void GetLeaderboard(string leaderboardId, LeaderboardScope scope, int page, int records, Action<Response<LeaderboardScores>> callback)
      {
         ValidationHelper.AssertValidId(leaderboardId, "leaderboardId");
         var payload = new Dictionary<string, object> { { "leaderboard", new { id = leaderboardId, scope = (int)scope, page = page, records = records } } };
         var communicator = new Communicator(this);
         communicator.SendPayload<LeaderboardScores>(Communicator.POST, "scores", payload, r =>
         {
            if (r.Success) { r.Data = JsonConvert.DeserializeObject<LeaderboardScores>(r.Raw); }
            callback(r);
         });
      }

      public void GetYesterdaysLeaders(string leaderboardId, Action<Response<LeaderboardScores>> callback)
      {
         ValidationHelper.AssertValidId(leaderboardId, "leaderboardId");
         var payload = new Dictionary<string, object> { { "leaderboard_id", leaderboardId } };
         var communicator = new Communicator(this);
         communicator.SendPayload<LeaderboardScores>(Communicator.POST, "scores/yesterdays_leaders", payload, r =>
         {
            if (r.Success) { r.Data = JsonConvert.DeserializeObject<LeaderboardScores>(r.Raw); }
            callback(r);
         });
      }

      public void GetLeaderboard(string leaderboardId, LeaderboardScope scope, int page, string userName, string uniqueIdentifier, Action<Response<LeaderboardScoresWithUser>> callback)
      {
         GetLeaderboard(leaderboardId, scope, page, 10, userName, uniqueIdentifier, callback);
      }

      public void GetLeaderboard(string leaderboardId, LeaderboardScope scope, int page, int records, string userName, string uniqueIdentifier, Action<Response<LeaderboardScoresWithUser>> callback)
      {
         ValidationHelper.AssertValidId(leaderboardId, "leaderboardId");
         var payload = new Dictionary<string, object>
                       {
                          { "leaderboard", new { id = leaderboardId, scope = (int)scope, page = page, records = records  } }, 
                          { "username", userName},
                          { "unique", uniqueIdentifier},
                       };
         var communicator = new Communicator(this);
         communicator.SendPayload<LeaderboardScoresWithUser>(Communicator.POST, "scores", payload, r =>
         {
            if (r.Success) { r.Data = JsonConvert.DeserializeObject<LeaderboardScoresWithUser>(r.Raw); }
            callback(r);
         });
      }

      public void GetYesterdaysTopRank(string leaderboardId, string userName, string uniqueIdentifier, Action<Response<int>> callback)
      {
         ValidationHelper.AssertValidId(leaderboardId, "leaderboardId");
         var payload = new Dictionary<string, object>
                       {
                          { "leaderboard_id", leaderboardId},
                          { "username", userName},
                          { "unique", uniqueIdentifier},
                       };
         var communicator = new Communicator(this);
         communicator.SendPayload<int>(Communicator.POST, "scores/yesterdays_rank", payload, r =>
         {
            if (r.Success)
            {
               var container = ((JContainer) JsonConvert.DeserializeObject(r.Raw))["rank"];
               r.Data = container == null ? 0 : container.Value<int>();
            }
            callback(r);
         });
      }

      public void GrantAchievement(string achievementId, string userName, string uniqueIdentifier, Action<Response<Achievement>> callback)
      {
         ValidationHelper.AssertValidId(achievementId, "achievementId");
         ValidationHelper.AssertNotNullOrEmpty(userName, 20, "username");
         ValidationHelper.AssertNotNullOrEmpty(uniqueIdentifier, 50, "unique identifier");
         var payload = new Dictionary<string, object> { { "achievement_id", achievementId}, {"username", userName }, {"unique", uniqueIdentifier} };
         var communicator = new Communicator(this);
         communicator.SendPayload<Achievement>(Communicator.PUT, "achievements", payload, r =>
         {
            if (r.Success)
            {
               r.Data = new Achievement();
               var pointsContainer = ((JContainer)JsonConvert.DeserializeObject(r.Raw))["points"];
               var idContainer = ((JContainer)JsonConvert.DeserializeObject(r.Raw))["id"];
               r.Data.Points = pointsContainer == null ? 0 : pointsContainer.Value<int>();
               r.Data.Id = idContainer == null ? null : idContainer.Value<string>();
            }            
            callback(r);
         });         
      }


      public void GrantAchievement(Achievement achievement, string userName, string uniqueIdentifier, Action<Response<Achievement>> callback)
      {
         ValidationHelper.AssertNotNull(achievement, "achievement");
         GrantAchievement(achievement.Id, userName, uniqueIdentifier, callback);
      }

      public void LogError(string subject, string details)
      {
         var payload = new Dictionary<string, object> { { "subject", subject }, { "details", details }};
         var communicator = new Communicator(this);
         communicator.SendPayload<object>(Communicator.PUT, "logging/error", payload, null);       
      }

      public void LogApplicationStart(string uniqueIdentifier)
      {
         var payload = new Dictionary<string, object> { { "unique", uniqueIdentifier }};
         var communicator = new Communicator(this);
         communicator.SendPayload<object>(Communicator.PUT, "analytics/start", payload, null);    
      }
   }
}