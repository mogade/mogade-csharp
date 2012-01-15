using NUnit.Framework;

namespace Mogade.Tests.Users
{
   public class RenameTests : BaseFixture
   {
      [Test]
      public void SendsRequestToTheServer()
      {
         Server.Stub(new ApiExpectation { Method = "POST", Url = "/gamma/users/rename", Request = "username=old&userkey=did&newname=new&key=akey&sig=0b29a989f610c9a4eb166846d9a63ff8a87c5c78", Response = "true" });
         new Driver("akey", "sssshh2").Rename("did", "old", "new", SetIfSuccess);
         WaitOne();
      }
   }
}