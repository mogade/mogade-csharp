using Mogade.Leaderboards;
using NUnit.Framework;

namespace Mogade.Tests.LeaderboardsTest
{
   public class GetLeaderboardTests : BaseFixture
   {
      [Test]
      public void SendsRequestForLeaderboardToTheServer()
      {
         Server.Stub(new ApiExpectation { Method = "POST", Url = "/scores", Request = @"{""leaderboard"":{""id"":""theid"",""scope"":2,""page"":3},""key"":""akey"",""v"":1,""sig"":""93a73d45c0cd3a0a648b3898d8992889""}" });
         new Mogade("akey", "sssshh2").GetLeaderboard("theid", LeaderboardScope.Weekly, 3);
      }
      [Test]
      public void RetrievesAnEmptyLeaderboard()
      {
         Server.Stub(new ApiExpectation { Response = @"{'scores':[]}"});
         var leaderboard = new Mogade("akey", "sssshh2").GetLeaderboard("theid", LeaderboardScope.Weekly, 3);
         Assert.AreEqual(0, leaderboard.Scores.Count);
      }
      [Test]
      public void RetrievesALeaderboard()
      {
         Server.Stub(new ApiExpectation { Response = @"{'scores':[{'username':'teg', 'points': 9001, 'data': 'something'}, {'username':'paul', 'points': 8999}]}" });
         var leaderboard = new Mogade("akey", "sssshh2").GetLeaderboard("theid", LeaderboardScope.Weekly, 3);
         Assert.AreEqual(2, leaderboard.Scores.Count);
         Assert.AreEqual("teg", leaderboard.Scores[0].UserName);
         Assert.AreEqual(9001, leaderboard.Scores[0].Points);
         Assert.AreEqual("something", leaderboard.Scores[0].Data);
         Assert.AreEqual("paul", leaderboard.Scores[1].UserName);
         Assert.AreEqual(8999, leaderboard.Scores[1].Points);
         Assert.AreEqual(null, leaderboard.Scores[1].Data);         
      }
      [Test]
      public void NullOrEmptyLeaderboardIdCausesAnExceptionToBeThrown()
      {
         AssertMogadeException("leaderboardId is required and cannot be null or empty", () => new Mogade("key", "secret").GetLeaderboard(null, LeaderboardScope.Daily, 3));
         AssertMogadeException("leaderboardId is required and cannot be null or empty", () => new Mogade("key", "secret").GetLeaderboard(string.Empty, LeaderboardScope.Overall, 4));
      }
   }
}