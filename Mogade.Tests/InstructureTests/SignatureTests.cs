using System.Collections.Generic;
using NUnit.Framework;

namespace Mogade.Tests
{
   public class SignatureTests : BaseFixture
   {
      protected override bool NeedAServer
      {
         get { return false; }
      }

      [Test]
      public void ProperlySignsThePayLoadBlackBox1()
      {
         var payload = new Dictionary<string, object> {{"itsover", 9000}};         
         Assert.AreEqual("36aed57e06468fd6f059be8d67277da3", Communicator.GetSignature(payload, "vegeta!"));
      }

      [Test]
      public void ProperlySignsThePayLoadBlackBox2()
      {
         var payload = new Dictionary<string, object>
                       {
                          { "itsover", 9000 },
                          { "first", new Dictionary<string, object>()
                                     {
                                        { "arealFirst", true}
                                     }
                          }
                       };
         Assert.AreEqual("e04df86b1b3b46bd195470097d039fa3", Communicator.GetSignature(payload, "duncanidaho"));
      }

      [Test]
      public void ProperlySignsThePayLoadBlackBox3()
      {
         var payload = new Dictionary<string, object>
                       {
                          { "complex", new Dictionary<string, object>()
                                     {
                                        { "arealFirst", new object[]{1, 1.5, "2"}}
                                     }
                          }
                       };
         Assert.AreEqual("46048805712f72eef059610f745883bb", Communicator.GetSignature(payload, "einstein"));
      }
   }   
}