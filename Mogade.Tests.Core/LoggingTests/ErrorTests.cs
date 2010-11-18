using NUnit.Framework;

namespace Mogade.Tests.LoggingTests
{
   public class ErrorTests : BaseFixture
   {
     [Test]
      public void SendsRequestForLeaderboardToTheServer()
      {
         Server.Stub(new ApiExpectation { Method = "PUT", Url = "/logging/error", Request = @"{""subject"":""the error subject"",""details"":""the error details"",""key"":""akey"",""v"":1,""sig"":""93a73d45c0cd3a0a648b3898d8992889""}" });
         new Driver("akey", "sssshh2").LogError("the error subject", "the error details");
      }
   }
}