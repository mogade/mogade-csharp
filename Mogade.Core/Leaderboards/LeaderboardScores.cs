using System.Collections.Generic;

namespace Mogade.Leaderboards
{
   public class LeaderboardScores
   {
      public IList<Score> Scores { get; set; }
      public int Page { get; set; }
   }
}