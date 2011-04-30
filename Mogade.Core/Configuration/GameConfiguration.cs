using System.Collections.Generic;
using Mogade.Achievements;
using Mogade.Leaderboards;

namespace Mogade.Configuration
{
   public class GameConfiguration
   {
      private IList<Achievement> _achievements;
      private IList<Leaderboard> _leaderboards;

      public int Version { get; set; }
      public IList<Achievement> Achievements
      {
         get { return _achievements ?? (_achievements = new List<Achievement>(5)); }
      }

      public IList<Leaderboard> Leaderboards
      {
         get { return _leaderboards ?? (_leaderboards = new List<Leaderboard>(5)); }
      }
   }
}