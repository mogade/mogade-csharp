using Mogade.Leaderboard;

namespace Mogade
{
   public interface IMogade
   {
      int ApiVersion { get; }
      Ranks SaveScore(string leaderboardId, Score score);      
   }
}