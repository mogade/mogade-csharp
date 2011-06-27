using System.Collections.Generic;

namespace Mogade
{
   public class LeaderboardScores
   {
      public IList<Score> Scores { get; set; }
      public int Page { get; set; }
   }
}