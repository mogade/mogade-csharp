using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Mogade
{
   public class Communicator
   {
      public const string PUT = "PUT";
      public const string POST = "POST";
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
         var request = (HttpWebRequest)WebRequest.Create(MogadeConfiguration.Data.Url + endPoint);
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

         try
         {
            var response = (HttpWebResponse)request.GetResponse();
            return GetResponseBody(response);
         }
         catch (Exception ex)
         {
            throw HandleException(ex);
         }
      }


      private byte[] FinalizePayload(IDictionary<string, object> payload)
      {
         payload.Add("key", _context.Key);
         payload.Add("v", _context.ApiVersion);
         payload.Add("sig", GetSignature(payload, _context.Secret));
         return Encoding.Default.GetBytes(JsonConvert.SerializeObject(payload, Formatting.None, _jsonSettings));
      }

      public static string GetSignature(IEnumerable<KeyValuePair<string, object>> parameters, string secret)
      {
         var sortAndFlat = new SortedDictionary<string, string>();
         BuildPayloadParameters(parameters, sortAndFlat);
         var sb = new StringBuilder();
         foreach (var parameter in sortAndFlat)
         {
            sb.AppendFormat("{0}={1}&", parameter.Key, parameter.Value);
         }
         sb.Append(secret);
         using (var hasher = new MD5CryptoServiceProvider())
         {
            var bytes = hasher.ComputeHash(Encoding.Default.GetBytes(sb.ToString()));
            var data = new StringBuilder(bytes.Length * 2);
            for (var i = 0; i < bytes.Length; ++i)
            {
               data.Append(bytes[i].ToString("x2"));
            }
            return data.ToString();
         }
      }

      private static string GetResponseBody(WebResponse response)
      {
         using (var stream = response.GetResponseStream())
         {
            var buffer = new byte[response.ContentLength == -1 ? 1024 : response.ContentLength];
            stream.Read(buffer, 0, buffer.Length);            
            return Encoding.Default.GetString(buffer).TrimEnd('\0');
         }
      }
      private static void BuildPayloadParameters(IEnumerable<KeyValuePair<string, object>> payload, IDictionary<string, string> parameters)
      {
         foreach (var kvp in payload)
         {
            if (kvp.Value == null) { continue; }
            var valueType = kvp.Value.GetType();
            if (typeof(string).IsAssignableFrom(valueType))
            {
               parameters.Add(kvp.Key, (string)kvp.Value);
            }
            else if (typeof(int).IsAssignableFrom(valueType) || typeof(long).IsAssignableFrom(valueType))
            {
               parameters.Add(kvp.Key, kvp.Value.ToString());
            }
            else if (typeof(bool).IsAssignableFrom(valueType))
            {
               parameters.Add(kvp.Key, (bool)kvp.Value ? "true" : "false");
            }            
            else if (typeof(IEnumerable).IsAssignableFrom(valueType))
            {
               var sb = new StringBuilder();
               foreach (var v in (IEnumerable)kvp.Value) { sb.AppendFormat("{0}-", v); }
               if (sb.Length > 0) { sb.Remove(sb.Length - 1, 1); }
               parameters.Add(kvp.Key, sb.ToString());
            }
            else
            {
               var properties = TypeDescriptor.GetProperties(kvp.Value);
               var hash = new Dictionary<string, object>(properties.Count);
               foreach (PropertyDescriptor descriptor in properties)
               {
                  hash.Add(descriptor.Name, descriptor.GetValue(kvp.Value));
               }
               BuildPayloadParameters(hash, parameters);               
            }            
         }
      }
      private static Exception HandleException(Exception exception)
      {
         if (exception is WebException)
         {            
            var body = GetResponseBody(((WebException)exception).Response);
            try
            {
               var message = JsonConvert.DeserializeObject<ErrorMessage>(body, _jsonSettings);
               return new MogadeException(message.Error ?? message.Maintenance, message.Info, exception);
            }
            catch (Exception)
            {
               return new MogadeException(body, exception);
            }                        
         }
         return new MogadeException("Unknown Error", exception);
      }

      private class ErrorMessage
      {
         public string Error { get; set; }
         public string Info { get; set; }
         public string Maintenance { get; set; }
      }
   }
}