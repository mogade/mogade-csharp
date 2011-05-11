using Mogade.Leaderboards;
using NUnit.Framework;

namespace Mogade.Tests.LeaderboardsTest
{
   public class SaveScoreTests : BaseFixture
   {
      [Test]
      public void SendsScoreWithoutDataToTheServer()
      {
         Server.Stub(new ApiExpectation { Method = "POST", Url = "/scores", Request = @"lid=mybaloney&username=Scytale&userkey=gom%20jabbar&points=10039&key=thekey&v=2&sig=6aed03350d0d02d027478a137ae51beccb82c93b", Response = "{}" });
         var score = new Score { Points = 10039, UserName = "Scytale"};
         new Driver("thekey", "sssshh").SaveScore("mybaloney", score, "gom jabbar", SetIfSuccess);
         WaitOne();
      }

      [Test]
      public void SendsScoreWithDataToTheServer()
      {
         Server.Stub(new ApiExpectation { Method = "POST", Url = "/scores", Request = "lid=mybaloney&username=Scytale&userkey=gom%20jabbar&points=10039&data=mydata&key=thekey&v=2&sig=ff01c97ae8b21a32c9833a011ffa6a2464bb8362", Response = "{}" });
         var score = new Score { Points = 10039, UserName = "Scytale", Data = "mydata" };
         new Driver("thekey", "sssshh").SaveScore("mybaloney", score, "gom jabbar", SetIfSuccess);
         WaitOne();
      }

      [Test]
      public void RetrievesAllTheRanksFromTheResponse()
      {
         Server.Stub(new ApiExpectation { Response = @"{1: 20, 2: 25, 3: 45, 4:22}" });
         var score = new Score { Points = 10039, UserName = "Scytale" };
         new Driver("thekey", "sssshh").SaveScore("mybaloney", score, "gom jabbar", ranks =>
         {
            Assert.AreEqual(true, ranks.Success);
            Assert.AreEqual(20, ranks.Data.Daily);
            Assert.AreEqual(25, ranks.Data.Weekly);
            Assert.AreEqual(45, ranks.Data.Overall);
            Assert.AreEqual(22, ranks.Data.Yesterday);
            Set();
         });
         WaitOne();
      }

      [Test]
      public void RetrievesAnEmptyRankSet() //SaveScore isn't guaranteed to return all, or even any rank
      {
         Server.Stub(new ApiExpectation { Response = @"{}" });
         var score = new Score { Points = 10039, UserName = "Scytale", };
         new Driver("thekey", "sssshh").SaveScore("mybaloney", score, "gom jabbar", ranks =>
         {
            Assert.AreEqual(0, ranks.Data.Daily);
            Assert.AreEqual(0, ranks.Data.Weekly);
            Assert.AreEqual(0, ranks.Data.Overall);
            Assert.AreEqual(0, ranks.Data.Yesterday);
            Set();
         });
         WaitOne();
      }

      [Test]
      public void RetrievesAnPartialRankSet() //SaveScore isn't guaranteed to return all, or even any rank
      {
         Server.Stub(new ApiExpectation { Response = @"{2: 49494}" });
         var score = new Score { Points = 10039, UserName = "Scytale", };
         new Driver("thekey", "sssshh").SaveScore("mybaloney", score, "gom jabbar", ranks =>
         {
            Assert.AreEqual(0, ranks.Data.Daily);
            Assert.AreEqual(49494, ranks.Data.Weekly);
            Assert.AreEqual(0, ranks.Data.Overall);
            Assert.AreEqual(0, ranks.Data.Yesterday);
            Set();
         });
         WaitOne();
      }
   }
}