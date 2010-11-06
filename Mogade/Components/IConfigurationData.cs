namespace Mogade
{
   public interface IConfigurationData
   {
      string Url{get;}
   }

   public class ConfigurationData : IConfigurationData
   {
      public const string APIURL = "http://api.mogade.com/api/";
      public const string TESTURL = "http://api.mogade.com/api/";
      private string _url;
      public string Url
      {
         get { return _url ?? APIURL; }
         set { _url = value; }
      }
   }
}