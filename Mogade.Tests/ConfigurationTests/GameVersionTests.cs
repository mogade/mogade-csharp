using NUnit.Framework;

namespace Mogade.Tests.ConfigurationTests
{
   public class GameVersionTests : BaseFixture
   {
      [Test]
      public void SendsRequestForVersionTheServer()
      {
         Server.Stub(new ApiExpectation { Method = "POST", Url = "/conf/version", Request = @"{""key"":""akey"",""v"":1,""sig"":""b2cf06b6f6f1d7c3b51092670b005010""}", Response = "{version:0}" });
         new Mogade("akey", "sssshh2").GameVersion();
      }

      [Test]
      public void GetsTheVersionFromTheServer()
      {
         Server.Stub(new ApiExpectation { Response = "{version: 47}"});
         Assert.AreEqual(47, new Mogade("akey", "sssshh2").GameVersion());
      }      
   }
}