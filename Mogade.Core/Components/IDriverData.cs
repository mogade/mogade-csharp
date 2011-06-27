using System;

namespace Mogade
{
   public interface IDriverData
   {
      string Url{get;}
      Func<bool> NetworkCheck { get; }
   }

   public class DriverData : IDriverData
   {
      public const string APIURL = "http://api2.mogade.com/api/";
      public const string TESTURL = "http://testing2.mogade.com/api/";
      private string _url;
      private Func<bool> _networkCheck = () => true;

      public string Url
      {
         get { return _url ?? APIURL; }
         set { _url = value; }
      }
      public Func<bool> NetworkCheck
      {
         get { return _networkCheck; }
         set { _networkCheck = value; }
      }

      public void Reset()
      {
         _url = null;
         _networkCheck = () => true;
      }
   }
}