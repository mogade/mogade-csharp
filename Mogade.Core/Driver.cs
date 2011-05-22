using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mogade
{
   public class Driver : IDriver, IRequestContext
   {      
      public const int VERSION = 2;
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

      public void SaveScore(string leaderboardId, Score score, string uniqueIdentifier, Action<Response<Ranks>> callback)
      {
         var payload = new Dictionary<string, object> {{"lid", leaderboardId}, {"username", score.UserName}, {"userkey", uniqueIdentifier}, {"points", score.Points}, {"data", score.Data}};
         var communicator = new Communicator(this);
         communicator.SendPayload<Ranks>(Communicator.Post, "scores", payload, r =>
         {
            if (r.Success) { r.Data = JsonConvert.DeserializeObject<Ranks>(r.Raw); }
            if (callback != null) { callback(r); }
         });
      }

      public void GetLeaderboard(string leaderboardId, LeaderboardScope scope, int page, int records, Action<Response<LeaderboardScores>> callback)
      {
         var payload = new Dictionary<string, object> {{"lid", leaderboardId}, {"page", page}, {"records", records}, {"scope", (int) scope}};
         GetLeaderboard(payload, callback);
      }

      public void GetLeaderboard(string leaderboardId, LeaderboardScope scope, string userName, string uniqueIdentifier, int records, Action<Response<LeaderboardScores>> callback)
      {
         var payload = new Dictionary<string, object> { { "lid", leaderboardId }, { "username", userName }, {"userKey", uniqueIdentifier}, { "records", records }, { "scope", (int)scope } };
         GetLeaderboard(payload, callback);
      }

      private void GetLeaderboard(IDictionary<string, object> payload, Action<Response<LeaderboardScores>> callback)
      {
         var communicator = new Communicator(this);
         communicator.SendPayload<LeaderboardScores>(Communicator.Get, "scores", payload, r =>
         {
            if (r.Success) { r.Data = JsonConvert.DeserializeObject<LeaderboardScores>(r.Raw); }
            callback(r);
         });
      }

      public void GetRank(string leaderboardId, string userName, string uniqueIdentifier, LeaderboardScope scope, Action<Response<int>> callback)
      {
         var payload = new Dictionary<string, object> { { "lid", leaderboardId }, { "username", userName }, { "userkey", uniqueIdentifier },  {"scopes", (int)scope} };
         var communicator = new Communicator(this);
         communicator.SendPayload<int>(Communicator.Get, "ranks", payload, r =>
         {
            if (r.Success)
            {
               var ranks = JsonConvert.DeserializeObject<Ranks>(r.Raw);
               r.Data = ranks.GetByScope(scope);
            }
            callback(r);
         });
      }

      public void GetRanks(string leaderboardId, string userName, string uniqueIdentifier, Action<Response<Ranks>> callback)
      {
         var allScopes = new[] { LeaderboardScope.Daily, LeaderboardScope.Weekly, LeaderboardScope.Overall, LeaderboardScope.Yesterday};
         GetRanks(leaderboardId, userName, uniqueIdentifier, allScopes, callback);
      }

      public void GetRanks(string leaderboardId, string userName, string uniqueIdentifier, LeaderboardScope[] scopes, Action<Response<Ranks>> callback)
      {
         var realScopes = new int[scopes.Length];
         for (var i = 0; i < scopes.Length; ++i)
         {
            realScopes[i] = (int) scopes[i];
         }
         var payload = new Dictionary<string, object> { { "lid", leaderboardId }, { "username", userName }, { "userkey", uniqueIdentifier }, { "scopes", realScopes } };
         var communicator = new Communicator(this);
         communicator.SendPayload<Ranks>(Communicator.Get, "ranks", payload, r =>
         {
            if (r.Success) { r.Data = JsonConvert.DeserializeObject<Ranks>(r.Raw); }
            callback(r);
         });
      }

      public void GetEarnedAchievements(string userName, string uniqueIdentifier, Action<Response<ICollection<string>>> callback)
      {
         //unlike most GET operation, this actually requies the game's key
         //though it still doesn't require signing
         var payload = new Dictionary<string, object> {  { "username", userName }, { "userKey", uniqueIdentifier }, {"key", Key}};
         var communicator = new Communicator(this);
         communicator.SendPayload<ICollection<string>>(Communicator.Get, "achievements", payload, r =>
         {
            if (r.Success) { r.Data = JsonConvert.DeserializeObject<ICollection<string>>(r.Raw); }
            callback(r);
         });
      }

      public void AchievementEarned(string achievementId, string userName, string uniqueIdentifier, Action<Response<Achievement>> callback)
      {
         var payload = new Dictionary<string, object> { {"aid", achievementId}, { "username", userName }, { "userKey", uniqueIdentifier }};
         var communicator = new Communicator(this);
         communicator.SendPayload<Achievement>(Communicator.Post, "achievements", payload, r =>
         {
            if (r.Success) { r.Data = JsonConvert.DeserializeObject<Achievement>(r.Raw); }
            if (callback != null) { callback(r); }
         });
      }

      public void LogApplicationStart(string uniqueIdentifier, Action<Response> callback)
      {
         var payload = new Dictionary<string, object> { { "unique", uniqueIdentifier } };
         var communicator = new Communicator(this);
         communicator.SendPayload<object>(Communicator.Post, "stats", payload, r =>
         {
            if (callback != null) { callback(r); }
         });
      }
   }
}