using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Mogade.Tests
{
   public abstract class BaseFixture
   {
      protected static readonly Func<IDictionary<string, object>> EmptyPayload = () => new Dictionary<string, object>();
      protected virtual bool NeedAServer 
      { 
         get { return true; } 
      }
      protected FakeServer Server;
      [SetUp]
      public void SetUp()
      {
         if (NeedAServer)
         {
            Server = new FakeServer();
            Communicator.IHateMyself("http://localhost:" + FakeServer.Port + "/");
         }
         BeforeEachTest();
      }
      [TearDown]
      public void TearDown()
      {
         if (Server != null) { Server.Dispose(); }
         AfterEachTest();
      }
      public virtual void AfterEachTest() { }
      public virtual void BeforeEachTest() { }
   }   
}