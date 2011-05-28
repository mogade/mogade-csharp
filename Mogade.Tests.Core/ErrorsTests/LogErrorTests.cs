using NUnit.Framework;

namespace Mogade.Tests.ErrorsTests
{
   public class LogErrorTests : BaseFixture
   {
      [Test]
      public void SendsRequestToTheServer()
      {
         Server.Stub(new ApiExpectation { Method = "POST", Url = "/gamma/errors", Request = "subject=ts&details=thedetails&key=akey&sig=98c6c01d87fecb1504d70d82c05b74cde732f2bf", Response = null });
         new Driver("akey", "sssshh2").LogError("ts", "thedetails", SetIfSuccess);
         WaitOne();
      }
   }
}