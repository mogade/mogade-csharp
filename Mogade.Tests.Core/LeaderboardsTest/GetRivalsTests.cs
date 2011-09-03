using System;
using NUnit.Framework;

namespace Mogade.Tests.LeaderboardsTest
{
   public class GetRivalsTests : BaseFixture
   {

      [Test]
      public void SendsRequestForToServer()
      {
         Server.Stub(new ApiExpectation { Method = "GET", Url = "/gamma/scores/rivals", Request = "lid=theid&username=itsme&userKey=imunique&scope=2", Response = "[]" });
         new Driver("akey", "sssshh2").GetRivals("theid", LeaderboardScope.Weekly, "itsme", "imunique", SetIfSuccess);
         WaitOne();
      }

      [Test]
      public void RetrievesAnEmptyScoreset()
      {
         Server.Stub(new ApiExpectation { Response = @"[]" });
         new Driver("akey", "sssshh2").GetRivals("theid", LeaderboardScope.Weekly, "itsme", "imunique", leaderboard =>
         {
            Assert.AreEqual(true, leaderboard.Success);
            Assert.AreEqual(0, leaderboard.Data.Count);
            Set();
         });
         WaitOne();
      }

      [Test]
      public void RetrievesAnBlankScoreset()
      {
         Server.Stub(new ApiExpectation { Response = @"" });
         new Driver("akey", "sssshh2").GetRivals("theid", LeaderboardScope.Weekly, "itsme", "imunique", leaderboard =>
         {
            Assert.AreEqual(true, leaderboard.Success);
            Assert.AreEqual(0, leaderboard.Data.Count);
            Set();
         });
         WaitOne();
      }
      [Test]
      public void RetrivesScores()
      {
         Server.Stub(new ApiExpectation { Response = @"[{'username':'teg', 'points': 9001, 'data': 'something', 'dated': '2009-02-16T04:06:06Z'}, {'username':'paul', 'points': 8999, 'dated': '2009-02-15T04:05:06Z'}]" });
         new Driver("akey", "sssshh2").GetRivals("theid", LeaderboardScope.Weekly, "itsme", "imunique", leaderboard =>
         {
            Assert.AreEqual(true, leaderboard.Success);
            Assert.AreEqual(2, leaderboard.Data.Count);
            Assert.AreEqual("teg", leaderboard.Data[0].UserName);
            Assert.AreEqual(9001, leaderboard.Data[0].Points);
            Assert.AreEqual("something", leaderboard.Data[0].Data);
            Assert.AreEqual(new DateTime(2009, 2, 16, 4, 6, 6), leaderboard.Data[0].Dated.ToUniversalTime());
            Assert.AreEqual("paul", leaderboard.Data[1].UserName);
            Assert.AreEqual(8999, leaderboard.Data[1].Points);
            Assert.AreEqual(null, leaderboard.Data[1].Data);
            Assert.AreEqual(new DateTime(2009, 2, 15, 4, 5, 6), leaderboard.Data[1].Dated.ToUniversalTime());
            Set();
         });
         WaitOne();
      }
   }
}