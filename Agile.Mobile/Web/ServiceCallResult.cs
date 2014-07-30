using System;
using System.IO;
using System.Net;
using System.Text;
using Agile.Diagnostics.Logging;
using Agile.Framework;
using Agile.Framework.Web;
using Newtonsoft.Json;

namespace Agile.Mobile.Web
{
    /// <summary>
    /// Wrapper for all service call results.
    /// </summary>
    /// <typeparam name="T">Expected type to deserialize json into</typeparam>
    public class ServiceCallResult<T>
    {

        /// <summary>
        /// ctor if you already have deserialized data
        /// </summary>
        public ServiceCallResult(T data)
        {
            Logger.Debug("ServiceCallResult {0}", typeof(T).Name);
            Value = data;
        }

        /// <summary>
        /// Constuct an 'ERROR' response
        /// </summary>
        public ServiceCallResult(Exception ex)
        {
            Logger.Debug("ServiceCallResult EX:");
            if (ex == null)
            {
                Logger.Warning("Tried to create ServiceCallResult with Exception but it was NULL!");
                return;
            }

            Logger.Warning("[{0}] {1}", ex.GetType().Name, ex.Message);
            Exception = ex;

            if (WebException == null)
            {
                Logger.Warning("WebException was null");
                return;
            }

            SetWebResponseDetails(WebException.Response);
        }

        public ServiceCallResult(WebResponse response)
        {
            if (response == null)
                throw new Exception("Response cannot be null");
            SetWebResponseDetails(response);
        }

        /// <summary>
        /// Only call this in create methods while the Response has definitely not been disposed
        /// </summary>
        private void DeserializeResponse(WebResponse response)
        {
            if(response == null)
                return;

            Logger.Debug("Deserializing {0}", ContentType);
            if (IsTextResponse)
            {
                try
                {
                    TextResponse = response.GetResponseStream().StreamToString();
                    // log a warning because a text response usually means something has gone wrong
                    Logger.Warning(TextResponse);
                }
                catch
                {
                    Logger.Warning("Failed to convert stream to string");
                    // do nothing is fine, ex is already logged in the extension method
                }
            }
            else if (IsXmlResponse) // for now just handle like plain text
            {
                try
                {
                    TextResponse = response.GetResponseStream().StreamToString();
                    // log a warning because a text response usually means something has gone wrong
                    Logger.Warning(TextResponse);
                }
                catch
                {
                    Logger.Warning("Failed to convert stream to string");
                    // do nothing is fine, ex is already logged in the extension method
                }
            }
            else if (IsJsonResponse)
            {
                try
                {
                    var reader = new StreamReader(response.GetResponseStream());
                    var jsonReader = new JsonTextReader(reader);

                    Value = jsonSerializer.Deserialize<T>(jsonReader);
                    Logger.Debug("Value:{0}", Value);
                }
                catch (Exception ex)
                {
                    // do NOT Logger.Error! if an ex occurs deserializing, that could trigger another message sent to the server with potentially the same recurring deserialization issue...
                    Logger.Warning("ServiceCallResult[T:{0}] {1}"
                        , typeof(T).Name, ex.Message);
                }
            }
            else
            {
                Logger.Warning("No matching ContentType for Response Deserialization [{0}]", ContentType);
            }
        }
        
        /// <summary>
        /// extract the info we need because the response gets
        /// disposed almost immediately.
        /// </summary>
        private void SetWebResponseDetails(WebResponse response)
        {
            ContentType = "";
            StatusCode = HttpStatusCode.HttpVersionNotSupported;

            if (response == null)
            {
                Logger.Warning("Response is NULL");
                return;
            }
            ContentType = response.ContentType;

            var httpResponse = response as HttpWebResponse;
            if (httpResponse == null)
                return;

            StatusCode = httpResponse.StatusCode;
            Logger.Info("Response: {0} | {1}", StatusCode.ToString(), ContentType);

            DeserializeResponse(response);
        }

        /// <summary>
        /// Gets the response ContentType
        /// </summary>
        public string ContentType { get; private set; }

        /// <summary>
        /// Returns true if ContentType is json
        /// </summary>
        public bool IsJsonResponse {
            get { return ContentType.StartsWith(ContentTypes.JSON, StringComparison.CurrentCultureIgnoreCase); }
        }

        /// <summary>
        /// Returns true if ContentType is plain text
        /// </summary>
        public bool IsTextResponse
        {
            get { return ContentType.StartsWith(ContentTypes.TEXT, StringComparison.CurrentCultureIgnoreCase); }
        }

        /// <summary>
        /// Returns true if ContentType isXML
        /// </summary>
        public bool IsXmlResponse
        {
            get { return ContentType.StartsWith(ContentTypes.XML, StringComparison.CurrentCultureIgnoreCase); }
        }

        private static JsonSerializer jsonSerializer = new JsonSerializer();

        /// <summary>
        /// Gets the Response HttpStatusCode
        /// </summary>
        /// <returns>HttpVersionNotSupported if not an HttpWebResponse</returns>
        public HttpStatusCode StatusCode { get; private set; }

        /// <summary>
        /// Returns true if the call was successful (no exception occurred)
        /// </summary>
        public bool WasSuccessful
        {
            get { return Exception == null; }
        }

        /// <summary>
        /// Gets the deserialized value returned by the call.
        /// Note that if T is a string, value will still not be set, you need to access TextResponse
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// Gets the text response that was returned instead of the expected Json.
        /// If T is string then this gets populated instead of Value
        /// </summary>
        public string TextResponse { get; private set; }

        /// <summary>
        /// Gets the exception that occured when the call was made
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Gets the Exception cast to a WebEx (most of the time will be a WebEx)
        /// </summary>
        public WebException WebException {
            get { return Exception as WebException; }
        }
        

    }
}