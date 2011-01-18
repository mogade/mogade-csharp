using Mogade.Leaderboards;
using NUnit.Framework;

namespace Mogade.Tests.LeaderboardsTest
{
   public class SaveScoreTests : BaseFixture
   {
      [Test]
      public void SendsScoreWithoutDataToTheServer()
      {
         Server.Stub(new ApiExpectation { Method = "PUT", Url = "/scores", Request = @"{""leaderboard_id"":""mybaloney"",""score"":{""username"":""Scytale"",""points"":10039,""unique"":""gom jabbar""},""key"":""thekey"",""v"":1,""sig"":""b3a25f82263d56640ba29572699b29d6""}", Response = "{}" });
         var score = new Score { Points = 10039, UserName = "Scytale"};
         new Driver("thekey", "sssshh").SaveScore("mybaloney", score, "gom jabbar", SetIfSuccess);
         WaitOne();
      }

      [Test]
      public void SendsScoreWithDataToTheServer()
      {
         Server.Stub(new ApiExpectation { Method = "PUT", Url = "/scores", Request = @"{""leaderboard_id"":""mybaloney"",""score"":{""username"":""Scytale"",""points"":10039,""data"":""mydata"",""unique"":""gom jabbar""},""key"":""thekey"",""v"":1,""sig"":""af421951f2b5d691e9ec42ba30a61c34""}" });
         var score = new Score { Points = 10039, UserName = "Scytale", Data = "mydata" };
         new Driver("thekey", "sssshh").SaveScore("mybaloney", score, "gom jabbar", SetIfSuccess);
         WaitOne();
      }

      [Test]
      public void RetrievesAllTheRanksFromTheResponse()
      {
         Server.Stub(new ApiExpectation { Response = @"{""daily"": 20, ""weekly"": 25, ""overall"": 45}" });
         var score = new Score { Points = 10039, UserName = "Scytale" };
         new Driver("thekey", "sssshh").SaveScore("mybaloney", score, "gom jabbar", ranks =>
         {
            Assert.AreEqual(true, ranks.Success);
            Assert.AreEqual(20, ranks.Data.Daily);
            Assert.AreEqual(25, ranks.Data.Weekly);
            Assert.AreEqual(45, ranks.Data.Overall);
            Set();
         });
         WaitOne();
      }

      [Test]
      public void RetrievesANewTopFlag()
      {
         Server.Stub(new ApiExpectation { Response = @"{top_score: true}" });
         var score = new Score { Points = 10039, UserName = "Scytale" };
         new Driver("thekey", "sssshh").SaveScore("mybaloney", score, "gom jabbar", ranks =>
         {
            Assert.AreEqual(true, ranks.Data.TopScore);
            Set();
         });
         WaitOne();
      }

      [Test]
      public void RetrievesANonTopScoreFlag()
      {
         Server.Stub(new ApiExpectation { Response = @"{top_score: false}" });
         var score = new Score { Points = 10039, UserName = "Scytale" };
         new Driver("thekey", "sssshh").SaveScore("mybaloney", score, "gom jabbar", ranks =>
         {
            Assert.AreEqual(false, ranks.Data.TopScore);
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
            Set();
         });
         WaitOne();
      }

      [Test]
      public void RetrievesAnPartialRankSet() //SaveScore isn't guaranteed to return all, or even any rank
      {
         Server.Stub(new ApiExpectation { Response = @"{""weekly"": 49494}" });
         var score = new Score { Points = 10039, UserName = "Scytale", };
         new Driver("thekey", "sssshh").SaveScore("mybaloney", score, "gom jabbar", ranks =>
         {
            Assert.AreEqual(0, ranks.Data.Daily);
            Assert.AreEqual(49494, ranks.Data.Weekly);
            Assert.AreEqual(0, ranks.Data.Overall);
            Set();
         });         
         WaitOne();
      }

      [Test]
      public void NullOrEmptyLeaderboardIdCausesAnExceptionToBeThrown()
      {
         AssertMogadeException("leaderboardId is required and cannot be null or empty", () => new Driver("key", "secret").SaveScore(null, new Score(), "gom jabbar", r => { }));
         AssertMogadeException("leaderboardId is required and cannot be null or empty", () => new Driver("key", "secret").SaveScore(string.Empty, new Score(), "gom jabbar", r => { }));
      }

      [Test]
      public void NullScoreCausesAnExceptionToBeThrown()
      {
         AssertMogadeException("score is required and cannot be null", () => new Driver("key", "secret").SaveScore("abc", null, "gom jabbar", r => { }));
      }

      [Test]
      public void LongDataCausesAnExceptionToBeThrown()
      {
         AssertMogadeException("score data cannot be longer than 25 characters", () => new Driver("key", "secret").SaveScore("abc", new Score { Data = new string('a', 26) }, "gom jabbar", r => { }));
      }
      [Test]
      public void NullOrEmptyUserNameCausesAnException()
      {
         AssertMogadeException("score username is required and cannot be null or empty", () => new Driver("key", "secret").SaveScore("abc", new Score(), "gom jabbar", r => { }));
         AssertMogadeException("score username is required and cannot be null or empty", () => new Driver("key", "secret").SaveScore("abc", new Score { UserName = string.Empty }, "gom jabbar", r => { }));
      }
      [Test]
      public void NullOrEmptUniqueIdentifierCausesAnException()
      {
         AssertMogadeException("unique identifier is required and cannot be null or empty", () => new Driver("key", "secret").SaveScore("abc", new Score {UserName = "Leto"}, null, r => { }));
         AssertMogadeException("unique identifier is required and cannot be null or empty", () => new Driver("key", "secret").SaveScore("abc", new Score { UserName = "Ghanima"}, string.Empty, r => { }));
      }
      [Test]
      public void LongUserNameCausesAnException()
      {
         AssertMogadeException("score username cannot be longer than 20 characters", () => new Driver("key", "secret").SaveScore("abc", new Score { UserName = new string('a', 21) }, "gom jabbar", r => { }));         
      }
   }
}