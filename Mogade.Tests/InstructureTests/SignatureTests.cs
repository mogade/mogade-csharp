using System.Collections.Generic;
using NUnit.Framework;

namespace Mogade.Tests
{
   public class SignatureTests : BaseFixture
   {
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
         Assert.AreEqual("e78a795a3ede106b6f935b6d3b466aad", Communicator.GetSignature(payload, "duncanidaho"));
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
         Assert.AreEqual("708fce95cca6a26f245d2719eeb9b22d", Communicator.GetSignature(payload, "einstein"));
      }
   }   
}