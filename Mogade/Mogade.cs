using System.Collections.Generic;
using Mogade.Leaderboard;
using Newtonsoft.Json;

namespace Mogade
{
   public class Mogade : IMogade, IRequestContext
   {
      public const int VERSION = 1;
      
      public Mogade(string gameKey, string secret)
      {
         Key = gameKey;
         Secret = secret;
      }

      public int ApiVersion
      {
         get { return VERSION; }
      }
      public string Key { get; private set; }
      public string Secret { get; private set; }

      public Ranks SaveScore(string leaderboardId, Score score)
      {
         var payload = new Dictionary<string, object> { {"leaderboard_id", leaderboardId}, {"score", score} };
         var communicator = new Communicator(this);
         return JsonConvert.DeserializeObject<Ranks>(communicator.SendPayload(Communicator.PUT, "scores", payload));         
      }
   }
}