using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Mogade
{
   public class Communicator
   {
      public const string APIURL = "http://api.mogade.com/api/";
      public const string PUT = "PUT";
      private readonly IRequestContext _context;
      private static readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
                                                                     {
                                                                        DefaultValueHandling = DefaultValueHandling.Ignore,                                                                        
                                                                     };

      public Communicator(IRequestContext context)
      {
         _context = context;
      }

      public string SendPayload(string method, string endPoint, IDictionary<string, object> partialPayload)
      {
         var request = (HttpWebRequest)WebRequest.Create(_testUrlCuzImACheapLoser ?? APIURL + endPoint);
         request.Method = method;
         request.ContentType = "application/json";
         request.Timeout = 10000;
         request.ReadWriteTimeout = 10000;
         request.KeepAlive = false;

         var payload = FinalizePayload(partialPayload);
         var requestStream = request.GetRequestStream();

         requestStream.Write(payload, 0, payload.Length);
         requestStream.Flush();         
         requestStream.Close();

         var response = (HttpWebResponse)request.GetResponse();

         using (var stream = response.GetResponseStream())
         {
            var buffer = new byte[response.ContentLength];
            stream.Read(buffer, 0, (int) response.ContentLength);
            return Encoding.Default.GetString(buffer);
         }
      }

      private byte[] FinalizePayload(IDictionary<string, object> payload)
      {
         payload.Add("key", _context.Key);
         payload.Add("v", _context.ApiVersion);
         payload.Add("sig", GetSignature(payload, _context.Secret));
         return Encoding.Default.GetBytes(JsonConvert.SerializeObject(payload));
      }

      private static string GetSignature(IEnumerable<KeyValuePair<string, object>> payload, string secret)
      {
         var sb = new StringBuilder(100);
         BuildPayloadString(payload, sb);
         sb.Append(secret);
         using (var hasher = new MD5CryptoServiceProvider())
         {
            var bytes = hasher.ComputeHash(Encoding.Default.GetBytes(sb.ToString()));
            var data = new StringBuilder(bytes.Length*2);
            for (var i = 0; i < bytes.Length; ++i)
            {
               data.Append(bytes[i].ToString("x2"));
            }
            return data.ToString();
         }
      }

      private static void BuildPayloadString(IEnumerable<KeyValuePair<string, object>> payload, StringBuilder sb)
      {
         foreach (var kvp in payload)
         {
            var valueType = kvp.Value.GetType();
            if (typeof(IEnumerable<KeyValuePair<string, object>>).IsAssignableFrom(valueType))
            {
               BuildPayloadString((IEnumerable<KeyValuePair<string, object>>) kvp.Value, sb);               
            }
            if (typeof(IEnumerable).IsAssignableFrom(valueType))
            {
               sb.AppendFormat("{0}=", kvp.Key);
               foreach (var v in (IEnumerable)kvp.Value)
               {
                  sb.AppendFormat("{0}-", v);
               }
               sb.Remove(sb.Length - 1, 1);
            }
            else
            {
               sb.AppendFormat("{0}={1}", kvp.Key, kvp.Value);
            }            
         }
      }


      private static string _testUrlCuzImACheapLoser;
      internal static void IHateMyself(string dinosaursDiedBecauseITouchMyselfAtNight)
      {
         _testUrlCuzImACheapLoser = dinosaursDiedBecauseITouchMyselfAtNight;
      }
   }
}