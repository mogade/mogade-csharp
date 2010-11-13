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
      
      public void GetGameVersion(Action<int> callback)
      {
         var payload = new Dictionary<string, object>(0);
         var communicator = new Communicator(this);
         communicator.SendPayload(Communicator.POST, "conf/version", payload, r =>
         {
            if (!r.Success){ throw new MogadeException(r.Error);}
            var container = (JContainer) JsonConvert.DeserializeObject(r.Raw);
            callback(container["version"].Value<int>());
         });
      }

      public void GetUserSettings(string userName, string uniqueIdentifier, Action<UserSettings> callback)
      {
         ValidationHelper.AssertNotNullOrEmpty(userName, 20, "username");
         ValidationHelper.AssertNotNullOrEmpty(uniqueIdentifier, 50, "unique identifier");
         var payload = new Dictionary<string, object> { { "username", userName }, { "unique", uniqueIdentifier } };
         var communicator = new Communicator(this);
         communicator.SendPayload(Communicator.POST, "conf/my", payload, r =>
         {
            if (!r.Success) { throw new MogadeException(r.Error); }
            callback(JsonConvert.DeserializeObject<UserSettings>(r.Raw));
         });
         
      }

      public void GetGameConfiguration(Action<GameConfiguration> callback)
      {         
         var payload = new Dictionary<string, object>(0);
         var communicator = new Communicator(this);
         communicator.SendPayload(Communicator.POST, "conf", payload, r =>
         {
            if (!r.Success) { throw new MogadeException(r.Error); }
            callback(JsonConvert.DeserializeObject<GameConfiguration>(r.Raw));
         });
      }

      public void SaveScore(string leaderboardId, Score score, Action<Ranks> callback)
      {
         ValidationHelper.AssertValidId(leaderboardId, "leaderboardId");
         ValidationHelper.AssertNotNull(score, "score");
         ValidationHelper.AssertMaximumLength(score.Data, 25, "score data");
         ValidationHelper.AssertNotNullOrEmpty(score.UserName, 20, "score username");

         var payload = new Dictionary<string, object>
                       {
                          {"leaderboard_id", leaderboardId}, 
                          {"score", new {username = score.UserName, points = score.Points, data = score.Data}},
                       };
         var communicator = new Communicator(this);
         communicator.SendPayload(Communicator.PUT, "scores", payload, r =>
         {
            if (!r.Success) { throw new MogadeException(r.Error); }
            callback(JsonConvert.DeserializeObject<Ranks>(r.Raw));
         });
      }

      public void GetLeaderboard(string leaderboardId, LeaderboardScope scope, int page, Action<LeaderboardScores> callback)
      {
         ValidationHelper.AssertValidId(leaderboardId, "leaderboardId");
         var payload = new Dictionary<string, object> {{"leaderboard", new {id = leaderboardId, scope = (int) scope, page = page}}};
         var communicator = new Communicator(this);
         communicator.SendPayload(Communicator.POST, "scores", payload, r =>
         {
            if (!r.Success) { throw new MogadeException(r.Error); }
            callback(JsonConvert.DeserializeObject<LeaderboardScores>(r.Raw));
         });
      }

      public void GrantAchievement(string achievementId, string userName, string uniqueIdentifier, Action<int> callback)
      {
         ValidationHelper.AssertValidId(achievementId, "achievementId");
         ValidationHelper.AssertNotNullOrEmpty(userName, 20, "username");
         ValidationHelper.AssertNotNullOrEmpty(uniqueIdentifier, 50, "unique identifier");
         var payload = new Dictionary<string, object> { { "achievement_id", achievementId}, {"username", userName }, {"unique", uniqueIdentifier} };
         var communicator = new Communicator(this);
         communicator.SendPayload(Communicator.PUT, "achievements", payload, r =>
         {
            if (!r.Success) { throw new MogadeException(r.Error); }
            var container = (JContainer) JsonConvert.DeserializeObject(r.Raw);
            callback(container["points"].Value<int>());
         });         
      }


      public void GrantAchievement(Achievement achievement, string userName, string uniqueIdentifier, Action<int> callback)
      {
         ValidationHelper.AssertNotNull(achievement, "achievement");
         GrantAchievement(achievement.Id, userName, uniqueIdentifier, callback);
      }
   }
}