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
         new Driver("akey", "sssshh2").GetLeaderboard("theid", LeaderboardScope.Weekly, 3, SetIfSuccess);
         WaitOne();
      }

      [Test]
      public void SendsRequestForLeaderboardWithUserToTheServer()
      {
         Server.Stub(new ApiExpectation { Method = "POST", Url = "/scores", Request = @"{""leaderboard"":{""id"":""theid"",""scope"":2,""page"":3},""username"":""itsme"",""unique"":""imunique"",""key"":""akey"",""v"":1,""sig"":""8c9fc0bd05aee415a197cc0264dc4488""}" });
         new Driver("akey", "sssshh2").GetLeaderboard("theid", LeaderboardScope.Weekly, 3, "itsme", "imunique", SetIfSuccess);
         WaitOne();
      }
      [Test]
      public void RetrievesAnEmptyLeaderboard()
      {
         Server.Stub(new ApiExpectation { Response = @"{'scores':[]}"});
         new Driver("akey", "sssshh2").GetLeaderboard("theid", LeaderboardScope.Weekly, 3, leaderboard =>
         {
            Assert.AreEqual(true, leaderboard.Success);
            Assert.AreEqual(0, leaderboard.Data.Scores.Count);
            Set();
         });
         WaitOne();
      }
      [Test]
      public void RetrievesALeaderboard()
      {
         Server.Stub(new ApiExpectation { Response = @"{'scores':[{'username':'teg', 'points': 9001, 'data': 'something'}, {'username':'paul', 'points': 8999}]}" });
         new Driver("akey", "sssshh2").GetLeaderboard("theid", LeaderboardScope.Weekly, 3, leaderboard =>
         {
            Assert.AreEqual(true, leaderboard.Success);
            Assert.AreEqual(2, leaderboard.Data.Scores.Count);
            Assert.AreEqual("teg", leaderboard.Data.Scores[0].UserName);
            Assert.AreEqual(9001, leaderboard.Data.Scores[0].Points);
            Assert.AreEqual("something", leaderboard.Data.Scores[0].Data);
            Assert.AreEqual("paul", leaderboard.Data.Scores[1].UserName);
            Assert.AreEqual(8999, leaderboard.Data.Scores[1].Points);
            Assert.AreEqual(null, leaderboard.Data.Scores[1].Data);
            Set();
         });
         WaitOne();
      }
      [Test]
      public void RetrievesALeaderboardWithANullUserScore()
      {
         Server.Stub(new ApiExpectation { Response = @"{'user': null, 'scores':[{'username':'teg', 'points': 9001, 'data': 'something'}, {'username':'paul', 'points': 8999}]}" });
         new Driver("akey", "sssshh2").GetLeaderboard("theid", LeaderboardScope.Weekly, 3, "username", "unique", leaderboard =>
         {
            Assert.AreEqual(true, leaderboard.Success);
            Assert.AreEqual(2, leaderboard.Data.Scores.Count);            
            Assert.AreEqual(null, leaderboard.Data.User);
            Set();
         });
         WaitOne();
      }
      [Test]
      public void RetrievesALeaderboardWithAUserScore()
      {
         Server.Stub(new ApiExpectation { Response = @"{'user': {'username':'thatsMe!', 'points': 2393, 'data': 'mydata', 'rank': 4}, 'scores':[{'username':'teg', 'points': 9001, 'data': 'something'}, {'username':'paul', 'points': 8999}]}" });
         new Driver("akey", "sssshh2").GetLeaderboard("theid", LeaderboardScope.Weekly, 3, "username", "unique", leaderboard =>
         {
            Assert.AreEqual(true, leaderboard.Success);
            Assert.AreEqual(2, leaderboard.Data.Scores.Count);            
            Assert.AreEqual("thatsMe!", leaderboard.Data.User.UserName);
            Assert.AreEqual(2393, leaderboard.Data.User.Points);
            Assert.AreEqual("mydata", leaderboard.Data.User.Data);
            Assert.AreEqual(4, leaderboard.Data.User.Rank);
            Set();
         });
         WaitOne();
      }
      [Test]
      public void NullOrEmptyLeaderboardIdCausesAnExceptionToBeThrown()
      {
         AssertMogadeException("leaderboardId is required and cannot be null or empty", () => new Driver("key", "secret").GetLeaderboard(null, LeaderboardScope.Daily, 3, r => { }));
         AssertMogadeException("leaderboardId is required and cannot be null or empty", () => new Driver("key", "secret").GetLeaderboard(string.Empty, LeaderboardScope.Overall, 4, r => { }));
      }
   }
}