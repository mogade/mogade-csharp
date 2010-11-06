using System.Collections.Generic;
using System.Drawing;
using Mogade.Achievements;
using Mogade.Configuration;
using Mogade.Leaderboards;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mogade
{
   public class Mogade : IMogade, IRequestContext
   {      
      public const int VERSION = 1;

      /// <summary>
      /// The Mogade logo
      /// </summary>
      public static Image Logo 
      { 
         get
         {
            using (var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Mogade.Assets.logo.png"))
            {
               return new Bitmap(stream);
            }
         }   
      }
      
      public Mogade(string gameKey, string secret)
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
      
      public int GameVersion()
      {
         var payload = new Dictionary<string, object>(0);
         var communicator = new Communicator(this);
         var container = (JContainer)JsonConvert.DeserializeObject(communicator.SendPayload(Communicator.POST, "conf/version", payload));
         return container["version"].Value<int>();
      }

      public UserSettings GetUserSettings(string userName, string uniqueIdentifier)
      {
         ValidationHelper.AssertNotNullOrEmpty(userName, 20, "username");
         ValidationHelper.AssertNotNullOrEmpty(uniqueIdentifier, 50, "unique identifier");
         var payload = new Dictionary<string, object> { { "username", userName }, { "unique", uniqueIdentifier } };
         var communicator = new Communicator(this);
         return JsonConvert.DeserializeObject<UserSettings>(communicator.SendPayload(Communicator.POST, "conf/my", payload));
      }

      public Ranks SaveScore(string leaderboardId, Score score)
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
         return JsonConvert.DeserializeObject<Ranks>(communicator.SendPayload(Communicator.PUT, "scores", payload));         
      }

      public Leaderboard GetLeaderboard(string leaderboardId, LeaderboardScope scope, int page)
      {
         ValidationHelper.AssertValidId(leaderboardId, "leaderboardId");
         var payload = new Dictionary<string, object> {{"leaderboard", new {id = leaderboardId, scope = (int) scope, page = page}}};
         var communicator = new Communicator(this);
         return JsonConvert.DeserializeObject<Leaderboard>(communicator.SendPayload(Communicator.POST, "scores", payload));       
      }

      public int GrantAchievement(string achievementId, string userName, string uniqueIdentifier)
      {
         ValidationHelper.AssertValidId(achievementId, "achievementId");
         ValidationHelper.AssertNotNullOrEmpty(userName, 20, "username");
         ValidationHelper.AssertNotNullOrEmpty(uniqueIdentifier, 50, "unique identifier");
         var payload = new Dictionary<string, object> { { "achievement_id", achievementId}, {"username", userName }, {"unique", uniqueIdentifier} };
         var communicator = new Communicator(this);
         var container = (JContainer)JsonConvert.DeserializeObject(communicator.SendPayload(Communicator.PUT, "achievements", payload));
         return container["points"].Value<int>();
      }


      public int GrantAchievement(Achievement achievement, string userName, string uniqueIdentifier)
      {
         ValidationHelper.AssertNotNull(achievement, "achievement");
         return GrantAchievement(achievement.Id, userName, uniqueIdentifier);
      }
   }
}