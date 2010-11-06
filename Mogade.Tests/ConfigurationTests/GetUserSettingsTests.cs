using NUnit.Framework;

namespace Mogade.Tests.ConfigurationTests
{
   public class GetUserSettingsTests : BaseFixture
   {
      [Test]
      public void SendsRequestToTheServer()
      {
         Server.Stub(new ApiExpectation { Method = "POST", Url = "/conf/my", Request = @"{""username"":""Edric"",""unique"":""2323"",""key"":""thekey"",""v"":1,""sig"":""8be923bdb5bf9caf0e037652628cbbdb""}"});
         new Mogade("thekey", "sssshh").GetUserSettings("Edric", "2323");         
      }
      [Test]
      public void HandlesEmptyConfigurationFromServer()
      {
         Server.Stub(new ApiExpectation { Response = "{}" });
         var settings = new Mogade("thekey", "sssshh").GetUserSettings("Edric", "2323");
         Assert.AreEqual(0, settings.Achievements.Count);
      }
      [Test]
      public void GetsConfigurationFromServer()
      {
         Server.Stub(new ApiExpectation { Response = "{achievements: ['abc', '123']}" });
         var settings = new Mogade("thekey", "sssshh").GetUserSettings("Edric", "2323");
         Assert.AreEqual(2, settings.Achievements.Count);
         Assert.AreEqual("abc", settings.Achievements[0]);
         Assert.AreEqual("123", settings.Achievements[1]);
      }
      [Test]
      public void NullOrEmptyUserNameCausesAnException()
      {
         AssertMogadeException("username is required and cannot be null or empty", () => new Mogade("key", "secret").GetUserSettings(null, "something"));
         AssertMogadeException("username is required and cannot be null or empty", () => new Mogade("key", "secret").GetUserSettings(string.Empty, "something"));
      }
      [Test]
      public void NullOrEmptyUniqueIdentifierCausesAnException()
      {
         AssertMogadeException("unique identifier is required and cannot be null or empty", () => new Mogade("key", "secret").GetUserSettings("something", null));
         AssertMogadeException("unique identifier is required and cannot be null or empty", () => new Mogade("key", "secret").GetUserSettings("something", string.Empty));
      }
      [Test]
      public void LongUserNameCausesAnException()
      {
         AssertMogadeException("username cannot be longer than 20 characters", () => new Mogade("key", "secret").GetUserSettings(new string('c', 25), "something"));
      }
   }
}