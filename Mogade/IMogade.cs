using Mogade.Leaderboards;

namespace Mogade
{
   public interface IMogade
   {
      int ApiVersion { get; }
      Ranks SaveScore(string leaderboardId, Score score);
      Leaderboard GetLeaderboard(string leaderboardId, LeaderboardScope scope, int page);
   }
}