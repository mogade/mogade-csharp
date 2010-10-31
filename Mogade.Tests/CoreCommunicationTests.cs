using System.Collections.Generic;
using NUnit.Framework;

namespace Mogade.Tests
{
   public class CoreCommunicationTests : BaseFixture
   {
      [Test]
      public void PayloadIncludesTheVersion()
      {
         Server.Stub(new ApiExpectation());
         var response = new Communicator(new FakeContext()).SendPayload("PUT", "anything", new Dictionary<string, object>(0));
         Assert.True(response.Contains("\"v\":1"), "payload should contain the api version");
      }
   }
}