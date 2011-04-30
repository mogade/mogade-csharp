using NUnit.Framework;

namespace Mogade.Tests
{
   public class ErrorHandlingTests : BaseFixture
   {
      [Test]
      public void WrapsANormalServerErrorInMogadeException()
      {
         Server.Stub(new ApiExpectation { Status = 400, Response = @"{'error': 'its over 9000!!'}" });
         new Communicator(FakeContext.Defaults).SendPayload<object>("any", "any", EmptyPayload(), r =>
         {
            Assert.IsFalse(r.Success);
            Assert.AreEqual("its over 9000!!", r.Error.Message);
            Set();
         });
         WaitOne();         
      }

      [Test]
      public void IncludesAnErrorInfoInTheExceptionIfPresent()
      {
         Server.Stub(new ApiExpectation { Status = 400, Response = @"{'error': 'its over 9000!!', 'info': 'some extra goodness'}" });
         new Communicator(FakeContext.Defaults).SendPayload<object>("any", "any", EmptyPayload(), r =>
         {
            Assert.IsFalse(r.Success);
            Assert.AreEqual("some extra goodness",r.Error.Info);
            Set();
         });
         
         WaitOne();     
      }

      [Test]
      public void WrapsAnUnexpectedServerErrorInMogadeException()
      {
         Server.Stub(new ApiExpectation { Status = 500, Response = @"Server CRASH!" });
         new Communicator(FakeContext.Defaults).SendPayload<object>("any", "any", EmptyPayload(), r =>
         {
            Assert.IsFalse(r.Success);
            Assert.AreEqual("Server CRASH!", r.Error.Message);
            Set();
         });
         WaitOne();
      }

      [Test]
      public void WrapsAMaintenanceErrorInAMogadeException() //for now
      {
         Server.Stub(new ApiExpectation { Status = 503, Response = @"{'maintenance': 'the server is down for a bit'}" });
         new Communicator(FakeContext.Defaults).SendPayload<object>("any", "any", EmptyPayload(), r =>
         {
            Assert.IsFalse(r.Success);
            Assert.AreEqual("the server is down for a bit", r.Error.Maintenance);
            Set();
         });
         WaitOne();
      }

   }
}