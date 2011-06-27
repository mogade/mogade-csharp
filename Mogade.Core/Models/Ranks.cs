using Newtonsoft.Json;

namespace Mogade
{
   public class Ranks
   {
      [JsonProperty("1")]
      public int Daily { get; set; }
      [JsonProperty("2")]
      public int Weekly { get; set; }
      [JsonProperty("3")]
      public int Overall { get; set; }
      [JsonProperty("4")]
      public int Yesterday { get; set; }


      public int GetByScope(LeaderboardScope scope)
      {
         switch (scope)
         {
            case LeaderboardScope.Daily:
               return Daily;
            case LeaderboardScope.Overall:
               return Overall;
            case LeaderboardScope.Weekly:
               return Weekly;
            case LeaderboardScope.Yesterday:
               return Yesterday;
         }
         return 0;
      }
   }
}