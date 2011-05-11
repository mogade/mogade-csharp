using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Mogade
{
   public class Communicator
   {
      public const string Get = "GET";
      public const string Put = "PUT";
      public const string Post = "POST";

      private readonly IRequestContext _context;
      private static readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore, };

      public Communicator(IRequestContext context)
      {
         _context = context;
      }

      public void SendPayload<T>(string method, string endPoint, IDictionary<string, object> partialPayload, Action<Response<T>> callback)
      {
         if (!DriverConfiguration.Data.NetworkCheck())
         {
            if (callback != null)
            {
               callback(Response<T>.CreateError(new ErrorMessage {Message = "Network is not available"}));
            }
            return;
         }
         var url = DriverConfiguration.Data.Url + endPoint;
         var payload = FinalizePayload(partialPayload);
         if (method == Get) { url += '?' + payload; }
         var request = (HttpWebRequest)WebRequest.Create(url);
         request.Method = method;
         request.ContentType = "application/json";
         request.UserAgent = "mogade-csharp";
#if !WINDOWS_PHONE
         request.Timeout = 10000;
         request.ReadWriteTimeout = 10000;
         request.KeepAlive = false;
#endif   
         if (method == Get)
         {
            request.BeginGetResponse(GetResponseStream<T>, new RequestState<T> {Request = request, Callback = callback});
         }
         else 
         {
            request.BeginGetRequestStream(GetRequestStream<T>, new RequestState<T> { Request = request, Payload = Encoding.UTF8.GetBytes(payload), Callback = callback });
         }
      }

      private static void GetRequestStream<T>(IAsyncResult result)
      {
         var state = (RequestState<T>)result.AsyncState;
         using (var requestStream = state.Request.EndGetRequestStream(result))
         {
            requestStream.Write(state.Payload, 0, state.Payload.Length);
            requestStream.Flush();
            requestStream.Close();
         }
         state.Request.BeginGetResponse(GetResponseStream<T>, state);
      }

      private static void GetResponseStream<T>(IAsyncResult result)
      {
         var state = (ResponseState<T>)result.AsyncState;
         try
         {
            using (var response = (HttpWebResponse)state.Request.EndGetResponse(result))
            {
               if (state.Callback != null) { state.Callback(Response<T>.CreateSuccess(GetResponseBody(response))); }
            }
         }
         catch (Exception ex)
         {
            if (state.Callback != null) { state.Callback(Response<T>.CreateError(HandleException(ex))); }
         }
      }


      private string FinalizePayload(IDictionary<string, object> payload)
      {
         payload.Add("key", _context.Key);
         payload.Add("v", _context.ApiVersion);
         payload.Add("sig", GetSignature(payload, _context.Secret));
         var sb = new StringBuilder();
         foreach (var kvp in payload)
         {
            if (kvp.Value == null) { continue; }
            sb.Append(kvp.Key);
            sb.Append("=");
            sb.Append(Uri.EscapeUriString(kvp.Value.ToString()));
            sb.Append("&");
         }
         return sb.Remove(sb.Length - 1, 1).ToString();
      }

      public static string GetSignature(IEnumerable<KeyValuePair<string, object>> parameters, string secret)
      {
         var sorted = SortParameterForSignature(parameters);
         var sb = new StringBuilder();
         foreach (var parameter in sorted)
         {
            sb.AppendFormat("{0}={1}&", parameter.Key, parameter.Value);
         }
         sb.Append(secret);
         using (var hasher = new SHA1Managed())
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

      private static IEnumerable<KeyValuePair<string, object>> SortParameterForSignature(IEnumerable<KeyValuePair<string, object>> payload)
      {
         var parameters = new SortedDictionary<string, object>();
         foreach (var kvp in payload)
         {
            if (kvp.Value == null) { continue; }
            var valueType = kvp.Value.GetType();
            if (typeof(string).IsAssignableFrom(valueType))
            {
               parameters.Add(kvp.Key, kvp.Value);
            }
            else if (typeof(int).IsAssignableFrom(valueType) || typeof(long).IsAssignableFrom(valueType))
            {
               parameters.Add(kvp.Key, kvp.Value.ToString());
            }
            else if (typeof(bool).IsAssignableFrom(valueType))
            {
               parameters.Add(kvp.Key, (bool)kvp.Value ? "true" : "false");
            }
         }
         return parameters;
      }

      private static string GetResponseBody(WebResponse response)
      {
         using (var stream = response.GetResponseStream())
         {
            var sb = new StringBuilder();
            int read;
            var bufferSize = response.ContentLength == -1 ? 2048 : (int)response.ContentLength;
            if (bufferSize == 0) { return null; }
            do
            {
               var buffer = new byte[2048];
               read = stream.Read(buffer, 0, buffer.Length);
               sb.Append(Encoding.UTF8.GetString(buffer, 0, read));
            } while (read > 0);
            return sb.ToString();
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
               return new ErrorMessage {Message = body, InnerException = exception};
            }
         }
         return new ErrorMessage {Message = "Unknown Error", InnerException = exception};
      }


      private class ResponseState<T>
      {
         public HttpWebRequest Request { get; set; }
         public Action<Response<T>> Callback { get; set; }
      }
      private class RequestState<T> : ResponseState<T> 
      {         
         public byte[] Payload { get; set; }
      }
   }
}