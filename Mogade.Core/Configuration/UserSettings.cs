using System.Collections.Generic;
using Mogade.JsonConverters;
using Newtonsoft.Json;

namespace Mogade.Configuration
{
   public class UserSettings
   {
      private IList<string> _achievements;
      private IDictionary<string, int> _leaderboardHighScores;

      public IList<string> Achievements
      {
         get { return _achievements ?? (_achievements = new List<string>(2)); }
      }

      [JsonProperty("leaderboards"), JsonConverter(typeof(LeaderboardHighScoresConverter))]
      public IDictionary<string, int> LeaderboardHighScores
      {
         get { return _leaderboardHighScores ?? (_leaderboardHighScores = new Dictionary<string, int>(2)); }
      }
   }
}