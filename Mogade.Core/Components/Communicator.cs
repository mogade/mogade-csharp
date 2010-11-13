using System;
using System.Collections;
using System.Collections.Generic;
#if !WINDOWS_PHONE
using System.ComponentModel;
#endif
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

      public void SendPayload(string method, string endPoint, IDictionary<string, object> partialPayload, Action<Response> callback)
      {
         var request = (HttpWebRequest)WebRequest.Create(DriverConfiguration.Data.Url + endPoint);
         request.Method = method;
         request.ContentType = "application/json";
         request.UserAgent = "mogade-csharp";
#if !WINDOWS_PHONE
         request.Timeout = 10000;
         request.ReadWriteTimeout = 10000;
         request.KeepAlive = false;
#endif         
         request.BeginGetRequestStream(GetRequestStream, new RequestState{Request = request, Payload = FinalizePayload(partialPayload), Callback = callback});         
      }

      private static void GetRequestStream(IAsyncResult result)
      {
         var state = (RequestState)result.AsyncState;
         using (var requestStream = state.Request.EndGetRequestStream(result))
         {
            requestStream.Write(state.Payload, 0, state.Payload.Length);
            requestStream.Flush();
            requestStream.Close();
         }
         state.Request.BeginGetResponse(GetResponseStream, state);
      }
      
      private static void GetResponseStream(IAsyncResult result)
      {
         var state = (ResponseState)result.AsyncState;
         try
         {
            var response = (HttpWebResponse) state.Request.EndGetResponse(result);
            state.Callback(Response.CreateSuccess(GetResponseBody(response)));
         }
         catch (Exception ex)
         {
            state.Callback(Response.CreateError(HandleException(ex)));
         }
      }


      private byte[] FinalizePayload(IDictionary<string, object> payload)
      {
         payload.Add("key", _context.Key);
         payload.Add("v", _context.ApiVersion);
#if WINDOWS_PHONE
         const string signatureKey = "sig2";
#else
         const string signatureKey = "sig";
#endif
         payload.Add(signatureKey, GetSignature(payload, _context.Secret));
         return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload, Formatting.None, _jsonSettings));
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
         using (var hasher = CreateHasher())
         {
            var bytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
            var data = new StringBuilder(bytes.Length * 2);
            for (var i = 0; i < bytes.Length; ++i)
            {
               data.Append(bytes[i].ToString("x2"));
            }
            return data.ToString();
         }
      }

      private static HashAlgorithm CreateHasher()
      {         
#if WINDOWS_PHONE
         return new SHA1Managed();
#else
         return new MD5CryptoServiceProvider();
#endif
      }

      private static string GetResponseBody(WebResponse response)
      {
         using (var stream = response.GetResponseStream())
         {
            var buffer = new byte[response.ContentLength == -1 ? 1024 : response.ContentLength];
            stream.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer, 0, buffer.Length).TrimEnd('\0');
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
               var properties = valueType.GetProperties();
               var hash = new Dictionary<string, object>(properties.Length);
               foreach (var property in properties)
               {
                  hash.Add(property.Name, property.GetValue(kvp.Value, null));                  
               }
               BuildPayloadParameters(hash, parameters);
            }
         }
      }

      private static ErrorMessage HandleException(Exception exception)
      {
         if (exception is WebException)
         {
            var body = GetResponseBody(((WebException)exception).Response);
            try
            {
               var message = JsonConvert.DeserializeObject<ErrorMessage>(body, _jsonSettings);
               message.InnerException = exception;
               return message;
            }
            catch (Exception)
            {
               return new ErrorMessage {Error = body, InnerException = exception};
            }
         }
         return new ErrorMessage {Error = "Unknown Error", InnerException = exception};
      }


      private class ResponseState
      {
         public HttpWebRequest Request { get; set; }
         public Action<Response> Callback { get; set; }
      }
      private class RequestState : ResponseState
      {         
         public byte[] Payload { get; set; }
      }
   }
}