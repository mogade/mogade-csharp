using NUnit.Framework;

namespace Mogade.Tests.LeaderboardsTest
{
   public class GetLeaderboardTests : BaseFixture
   {
      [Test]
      public void SendsRequestForLeaderboardToTheServer()
      {
         Server.Stub(new ApiExpectation { Method = "GET", Url = "/gamma/scores", Request = "lid=theid&page=3&records=10&scope=2", Response = "{}" });
         new Driver("akey", "sssshh2").GetLeaderboard("theid", LeaderboardScope.Weekly, 3, 10, SetIfSuccess);
         WaitOne();
      }

      [Test]
      public void SendsRequestForLeaderboardWitUserAndRecordCount()
      {
         Server.Stub(new ApiExpectation { Method = "GET", Url = "/gamma/scores", Request = "lid=theid&username=itsme&userKey=imunique&records=25&scope=2", Response = "{}" });
         new Driver("akey", "sssshh2").GetLeaderboard("theid", LeaderboardScope.Weekly, "itsme", "imunique", 25, SetIfSuccess);
         WaitOne();
      }

      [Test]
      public void RetrievesAnEmptyLeaderboard()
      {
         Server.Stub(new ApiExpectation { Response = @"{'scores':[]}"});
         new Driver("akey", "sssshh2").GetLeaderboard("theid", LeaderboardScope.Weekly, 3, 10, leaderboard =>
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
         Server.Stub(new ApiExpectation { Response = @"{'page': 23, 'scores':[{'username':'teg', 'points': 9001, 'data': 'something'}, {'username':'paul', 'points': 8999}]}" });
         new Driver("akey", "sssshh2").GetLeaderboard("theid", LeaderboardScope.Weekly, 3, 10, leaderboard =>
         {
            Assert.AreEqual(true, leaderboard.Success);
            Assert.AreEqual(23, leaderboard.Data.Page);
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
   }
}