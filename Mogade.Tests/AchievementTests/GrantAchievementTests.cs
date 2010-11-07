using Mogade.Achievements;
using NUnit.Framework;

namespace Mogade.Tests.AchievementTests
{
   public class GrantAchievementTest : BaseFixture
   {
      [Test]
      public void SendsRequestToTheServer()
      {
         Server.Stub(new ApiExpectation { Method = "PUT", Url = "/achievements", Request = @"{""achievement_id"":""hasafirstname"",""username"":""Scytale"",""unique"":""10039"",""key"":""thekey"",""v"":1,""sig"":""64c32fca72deb24aa93f24f756403506""}", Response = "{points:123}" });
         new Mogade("thekey", "sssshh").GrantAchievement("hasafirstname", "Scytale", "10039", r => { });
      }

      [Test]
      public void GetsThePointsEarned()
      {
         Server.Stub(new ApiExpectation { Response = "{points:293}" });
         new Mogade("thekey", "sssshh").GrantAchievement("hasafirstname", "Scytale", "10039", r =>
         {
            Assert.AreEqual(293, r);
            Set();
         });         
         WaitOne();
      }

      [Test]
      public void NullOrEmptyAchievementIdCausesAnExceptionToBeThrown()
      {
         AssertMogadeException("achievementId is required and cannot be null or empty", () => new Mogade("key", "secret").GrantAchievement((string)null, "something", "something", r => { }));
         AssertMogadeException("achievementId is required and cannot be null or empty", () => new Mogade("key", "secret").GrantAchievement(string.Empty, "something", "something", r => { }));
      }
      [Test]
      public void NullAchievemenCausesAnExceptionToBeThrown()
      {
         AssertMogadeException("achievement is required and cannot be null", () => new Mogade("key", "secret").GrantAchievement((Achievement)null, "something", null, r => { }));
      }

      [Test]
      public void NullOrEmptyUserNameCausesAnException()
      {
         AssertMogadeException("username is required and cannot be null or empty", () => new Mogade("key", "secret").GrantAchievement("1234324", null, "something", r => { }));
         AssertMogadeException("username is required and cannot be null or empty", () => new Mogade("key", "secret").GrantAchievement("1234324", string.Empty, "something", r => { }));
      }
      [Test]
      public void NullOrEmptyUniqueIdentifierCausesAnException()
      {
         AssertMogadeException("unique identifier is required and cannot be null or empty", () => new Mogade("key", "secret").GrantAchievement("1234324", "something", null, r => { }));
         AssertMogadeException("unique identifier is required and cannot be null or empty", () => new Mogade("key", "secret").GrantAchievement("1234324", "something", string.Empty, r => { }));
      }
      [Test]
      public void LongUserNameCausesAnException()
      {
         AssertMogadeException("username cannot be longer than 20 characters", () => new Mogade("key", "secret").GrantAchievement("1234324", new string('c', 25), "something", r => { }));
      }
   }
}