using NUnit.Framework;

namespace Mogade.Tests.LeaderboardsTest
{
   public class YesterdaysLeadersTests : BaseFixture
   {
      [Test]
      public void SendsRequestToServer()
      {
         Server.Stub(new ApiExpectation { Method = "POST", Url = "/scores/yesterdays_leaders", Request = @"{""leaderboard_id"":""mybaloney"",""key"":""thekey"",""v"":1,""sig"":""91b1e2746724f0dd12d9d4882666655f""}" });
         new Driver("thekey", "sssshh").GetYesterdaysLeaders("mybaloney", SetIfSuccess);
         WaitOne();
      }

      [Test]
      public void RetrievesTheRankFromTheServer()
      {
         Server.Stub(new ApiExpectation { Response = @"{'scores':[{'username':'teg', 'points': 9001, 'data': 'something'}, {'username':'paul', 'points': 8999}]}" });
         new Driver("thekey", "sssshh").GetYesterdaysLeaders("mybaloney",  leaderboard =>
         {
            Assert.AreEqual(true, leaderboard.Success);
            Assert.AreEqual(0, leaderboard.Data.Page);
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