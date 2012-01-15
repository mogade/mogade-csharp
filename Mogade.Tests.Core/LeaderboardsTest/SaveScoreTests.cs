using NUnit.Framework;

namespace Mogade.Tests.LeaderboardsTest
{
   public class SaveScoreTests : BaseFixture
   {

      [Test]
      public void Darren()
      {
         Server.Stub(new ApiExpectation { Method = "POST", Url = "/gamma/scores", Request = "lid=mybaloney&username=Scytale&userkey=gom%20jabbar&points=10039&key=thekey&sig=ea56e70da9398d58eff2ec78d7d00605021dba12", Response = "{}" });
         var score = new Score { Points = 3, UserName = "Name", Data = "Name"};
         new Driver("4ee6add2563d8a7d3200001d", "Fw>HPS^OXw1Kx=_SATiE@32[FUZ9lW@uO").SaveScore("4ee6b064563d8a7d32000038", score, "android-emulator", SetIfSuccess);
         WaitOne();
      }
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
         Server.Stub(new ApiExpectation { Response = @"{ranks: {1: 20, 2: 25, 3: 45, 4:22}}" });
         var score = new Score { Points = 10039, UserName = "Scytale" };
         new Driver("thekey", "sssshh").SaveScore("mybaloney", score, "gom jabbar", r =>
         {
            Assert.AreEqual(true, r.Success);
            Assert.AreEqual(20, r.Data.Ranks.Daily);
            Assert.AreEqual(25, r.Data.Ranks.Weekly);
            Assert.AreEqual(45, r.Data.Ranks.Overall);
            Assert.AreEqual(22, r.Data.Ranks.Yesterday);
            Set();
         });
         WaitOne();
      }

      [Test]
      public void RetrievesAnEmptyRankSet() //SaveScore isn't guaranteed to return all, or even any rank
      {
         Server.Stub(new ApiExpectation { Response = @"{ranks:{}}" });
         var score = new Score { Points = 10039, UserName = "Scytale", };
         new Driver("thekey", "sssshh").SaveScore("mybaloney", score, "gom jabbar", r =>
         {
            Assert.AreEqual(0, r.Data.Ranks.Daily);
            Assert.AreEqual(0, r.Data.Ranks.Weekly);
            Assert.AreEqual(0, r.Data.Ranks.Overall);
            Assert.AreEqual(0, r.Data.Ranks.Yesterday);
            Set();
         });
         WaitOne();
      }

      [Test]
      public void RetrievesAnPartialRankSet() //SaveScore isn't guaranteed to return all, or even any rank
      {
         Server.Stub(new ApiExpectation { Response = @"{ranks:{2: 49494}}" });
         var score = new Score { Points = 10039, UserName = "Scytale", };
         new Driver("thekey", "sssshh").SaveScore("mybaloney", score, "gom jabbar", r =>
         {
            Assert.AreEqual(0, r.Data.Ranks.Daily);
            Assert.AreEqual(49494, r.Data.Ranks.Weekly);
            Assert.AreEqual(0, r.Data.Ranks.Overall);
            Assert.AreEqual(0, r.Data.Ranks.Yesterday);
            Set();
         });
         WaitOne();
      }

      [Test]
      public void RetrievesHighScores()
      {
         Server.Stub(new ApiExpectation { Response = @"{highs:{1: true, 2: false, 3:true}}" });
         var score = new Score { Points = 10039, UserName = "Scytale", };
         new Driver("thekey", "sssshh").SaveScore("mybaloney", score, "gom jabbar", r =>
         {
            Assert.AreEqual(true, r.Data.Highs.Daily);
            Assert.AreEqual(false, r.Data.Highs.Weekly);
            Assert.AreEqual(true, r.Data.Highs.Overall);
            Set();
         });
         WaitOne();
      }
   }
}