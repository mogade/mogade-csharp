namespace Mogade.Tests
{
   public class FakeContext : IRequestContext
   {
      public readonly static IRequestContext Defaults = new FakeContext();

      private string version = "gamma";
      private string secret = "secret";
      private string key = "api key";
      
      public string ApiVersion
      {
         get { return version; }
         set { version = value; }
      }
      public string Secret
      {
         get { return secret; }
         set { secret = value; }
      }
      public string Key
      {
         get { return key; }
         set { key = value; }
      }
   }
}