
using NUnit.Framework;

namespace Mogade.Tests
{
   public abstract class BaseFixture
   {
      protected FakeServer Server;
      [SetUp]
      public void SetUp()
      {           
         Server = new FakeServer();
         Communicator.IHateMyself("http://localhost:" + FakeServer.Port + "/");
         BeforeEachTest();
      }
      [TearDown]
      public void TearDown()
      {
         Server.Dispose();
         AfterEachTest();
      }
      public virtual void AfterEachTest() { }
      public virtual void BeforeEachTest() { }
   }
}