using NUnit.Framework;

namespace Mogade.Tests.LoggingTests
{
   public class ErrorTests : BaseFixture
   {
     [Test]
      public void SendsTheErrorToTheserver()
      {
         Server.Stub(new ApiExpectation { Method = "PUT", Url = "/logging/error", Request = @"{""subject"":""the error subject"",""details"":""the error details"",""key"":""akey"",""v"":1,""sig"":""03d599be2804554ac6e056fa6c1e6297""}" });
         Server.OnInvoke(Set);
         new Driver("akey", "sssshh2").LogError("the error subject", "the error details");
         WaitOne();
      }
   }
}