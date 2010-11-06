using NUnit.Framework;

namespace Mogade.Tests.ConfigurationTests
{
   public class GameVersionTests : BaseFixture
   {
      [Test]
      public void SendsRequestForVersionTheServer()
      {
         Server.Stub(new ApiExpectation { Method = "POST", Url = "/conf/version", Request = @"{""key"":""akey"",""v"":1,""sig"":""b0721964c554325c53250fa17b2cae29""}", Response = "{version:0}" });
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