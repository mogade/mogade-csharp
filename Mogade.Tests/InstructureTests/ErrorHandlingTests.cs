using NUnit.Framework;

namespace Mogade.Tests
{
   public class ErrorHandlingTests : BaseFixture
   {
      [Test]
      public void WrapsANormalServerErrorInMogadeException()
      {
         Server.Stub(new ApiExpectation { Status = 400, Response = @"{'error': 'its over 9000!!'}" });
         var ex = Assert.Throws<MogadeException>(() => new Communicator(FakeContext.Defaults).SendPayload("any", "any", EmptyPayload()));
         Assert.AreEqual("its over 9000!!", ex.Message);
      }

      [Test]
      public void IncludesAnErrorInfoInTheExceptionIfPresent()
      {
         Server.Stub(new ApiExpectation { Status = 400, Response = @"{'error': 'its over 9000!!', 'info': 'some extra goodness'}" });
         var ex = Assert.Throws<MogadeException>(() => new Communicator(FakeContext.Defaults).SendPayload("any", "any", EmptyPayload()));
         Assert.AreEqual("some extra goodness", ex.AdditionalInformation);
      }

      [Test]
      public void WrapsAnUnexpectedServerErrorInMogadeException()
      {
         Server.Stub(new ApiExpectation { Status = 500, Response = @"Server CRASH!" });
         var ex = Assert.Throws<MogadeException>(() => new Communicator(FakeContext.Defaults).SendPayload("any", "any", EmptyPayload()));
         Assert.AreEqual("Server CRASH!", ex.Message);
      }

      [Test]
      public void WrapsAMaintenanceErrorInAMogadeException() //for now
      {
         Server.Stub(new ApiExpectation { Status = 503, Response = @"{'maintenance': 'the server is down for a bit'}" });
         var ex = Assert.Throws<MogadeException>(() => new Communicator(FakeContext.Defaults).SendPayload("any", "any", EmptyPayload()));
         Assert.AreEqual("the server is down for a bit", ex.Message);
      }

   }
}