using NUnit.Framework;

namespace Mogade.Tests.LeaderboardsTest
{
   public class SaveScoreTests : BaseFixture
   {
      [Test]
      public void SendsScoreWithoutDataToTheServer()
      {
         Server.Stub(new ApiExpectation { Method = "POST", Url = "/gamma/scores", Request = "lid=mybaloney&username=Scytale&userkey=gom%20jabbar&points=10039&key=thekey&sig=ea56e70da9398d58eff2ec78d7d00605021dba12", Response = "{}" });
         var score = new Score { Points = 10039, UserName = "Scytale"};
         new Driver("thekey", "sssshh").SaveScore("mybaloney", score, "gom jabbar", SetIfSuccess);
         WaitOne();
      }

      [Test]
      public void SendsScoreWithDataToTheServer()
      {
         Server.Stub(new ApiExpectation { Method = "POST", Url = "/gamma/scores", Request = "lid=mybaloney&username=Scytale&userkey=gom%20jabbar&points=10039&data=mydata&key=thekey&sig=750c0ae7304e3e4cdc4e97e09f197f05d5708fad", Response = "{}" });
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