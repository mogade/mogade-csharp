using System;
using System.Collections.Generic;

namespace Mogade
{
   public interface IDriver
   {
      /// <summary>
      /// Returns the version of the API this library understands
      /// </summary>      
      int ApiVersion { get; }

      /// <summary>
      /// Saves a score
      /// </summary>
      /// <param name="leaderboardId">The id of the leaderboard to save the score to</param>
      /// <param name="score">The required score to save</param>
      /// <param name="uniqueIdentifier">A unique identifier for the user. Mobile devices should use the deviceId.</param>
      /// <returns>A rank object containing the daily, weekly, and overall rank of the user (or 0 if the user doesn't have a rank for the specific scope)</returns>
      /// <remarks>
      /// The username and points properties of the Score are required.
      /// Usernames should be 20 characters max
      /// 
      /// The data field is optional and limited to 50 characters. You can stuff meta information inside, such as "4|12:30", which
      /// might mean the user got to level 4 and played for 12 minutes and 30 seconds. You are responsible for encoding/decoding
      /// this information...we just take it in, store it, and pass it back out
      /// </remarks>
      void SaveScore(string leaderboardId, Score score, string uniqueIdentifier, Action<Response<Ranks>> callback);


      /// <summary>
      /// Gets a leaderboard page with a specific number of records
      /// </summary>
      /// <param name="leaderboardId">The id of the leaderboard to get the scores from</param>
      /// <param name="scope">The scope to get the scores from (daily, weekly or overall)</param>
      /// <param name="page">The page to get (starting with 1)</param>
      /// <param name="records">The number of records (up to 50)</param>
      /// <returns>A leaderboard object containing an array of scores</returns
      void GetLeaderboard(string leaderboardId, LeaderboardScope scope, int page, int records, Action<Response<LeaderboardScores>> callback);

      /// <summary>
      /// Gets a leaderboard located around the user's page
      /// </summary>
      /// <param name="leaderboardId">The id of the leaderboard to get the scores from</param>
      /// <param name="scope">The scope to get the scores from (daily, weekly or overall)</param>
      /// <param name="userName">the name of the user</param>
      /// <param name="uniqueIdentifier">A unique identifier for the user. Mobile devices should use the deviceId</param>
      /// <param name="records">The number of records (up to 50)</param>
      /// <returns>A leaderboard object containing an array of scores</returns>
      /// <remarks>
      /// Will return up to 10 records. The user's score object will only be returned when page = 1 and, of course, if the user has a score
      /// </remarks>
      void GetLeaderboard(string leaderboardId, LeaderboardScope scope, string userName, string uniqueIdentifier, int records, Action<Response<LeaderboardScores>> callback);

      /// <summary>
      /// Gets a a user's rank across all scopes
      /// </summary>
      /// <param name="leaderboardId">The id of the leaderboard to get the scores from</param>
      /// <param name="userName">the name of the user</param>
      /// <param name="uniqueIdentifier">A unique identifier for the user. Mobile devices should use the deviceId</param>
      /// <returns>Returns a rank object (0 means the user doesn't have a rank for the specified scope)</returns>
      void GetRanks(string leaderboardId, string userName, string uniqueIdentifier, Action<Response<Ranks>> callback);

      /// <summary>
      /// Gets a a user's rank across an individual scope
      /// </summary>
      /// <param name="leaderboardId">The id of the leaderboard to get the scores from</param>
      /// <param name="userName">the name of the user</param>
      /// <param name="uniqueIdentifier">A unique identifier for the user. Mobile devices should use the deviceId</param>
      /// <param name="scope">The scope to get the rank for</param>
      /// <returns>Returns the user's rank (0 means the user doesn't have a rank for the specified scope)</returns>
      void GetRank(string leaderboardId, string userName, string uniqueIdentifier, LeaderboardScope scope, Action<Response<int>> callback);

      /// <summary>
      /// Gets a a user's rank across an individual scope
      /// </summary>
      /// <param name="leaderboardId">The id of the leaderboard to get the scores from</param>
      /// <param name="userName">the name of the user</param>
      /// <param name="uniqueIdentifier">A unique identifier for the user. Mobile devices should use the deviceId</param>
      /// <param name="scopes">The scopes to get the rank for</param>
      /// <returns>Returns the user's rank (0 means the user doesn't have a rank for the specified scope, or that the scope wasn't requested)</returns>
      void GetRanks(string leaderboardId, string userName, string uniqueIdentifier, LeaderboardScope[] scopes, Action<Response<Ranks>> callback);

      /// <summary>
      /// Gets the achievement ids that the player has earned
      /// </summary>
      /// <param name="userName">the name of the user</param>
      /// <param name="uniqueIdentifier">A unique identifier for the user. Mobile devices should use the deviceId</param>
      /// <returns>An array containing the achievements earned by the user (or an empty array if the user hasn't earned anything)</returns>
      void GetEarnedAchievements(string userName, string uniqueIdentifier, Action<Response<ICollection<string>>> callback);

      /// <summary>
      /// Grants the user the specified achievement
      /// </summary>
      /// <param name="achievementId">The id of the achievementId earned</param>
      /// <param name="userName">the name of the user</param>
      /// <param name="uniqueIdentifier">A unique identifier for the user. Mobile devices should use the deviceId</param>
      /// <returns>An array containing the achievements earned by the user (or an achievement with a null id if the user has already earned it)</returns>
      void AchievementEarned(string achievementId, string userName, string uniqueIdentifier, Action<Response<Achievement>> callback);
   }
}