namespace Mogade
{
   public interface IDriverData
   {
      string Url{get;}
   }

   public class DriverData : IDriverData
   {
      public const string APIURL = "http://api.mogade.com/api/";
      public const string TESTURL = "http://testing.mogade.com/api/";
      private string _url;
      public string Url
      {
         get { return _url ?? APIURL; }
         set { _url = value; }
      }

      public void Reset()
      {
         _url = null;
      }
   }
}