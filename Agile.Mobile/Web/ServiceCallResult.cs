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
        private ServiceCallResult()
        {
            ContentType = "notSet";
        }

        /// <summary>
        /// ctor if you already have deserialized data
        /// </summary>
        public ServiceCallResult(T data) : this()
        {
            Logger.Debug("ServiceCallResult {0}", typeof(T).Name);
            Value = data;
        }

        /// <summary>
        /// Constuct an 'ERROR' response
        /// </summary>
        public ServiceCallResult(Exception ex)
            : this()
        {
            Logger.Debug("ServiceCallResult EX:");
            if (ex == null)
            {
                Logger.Warning("Tried to create ServiceCallResult with Exception but it was NULL!");
                return;
            }

            Logger.Info("[{0}] {1}", ex.GetType().Name, ex.Message);
            Exception = ex;

            if (WebException == null)
            {
                Logger.Info("WebException was null");
                return;
            }

            SetWebResponseDetails(WebException.Response);
        }

        public ServiceCallResult(WebResponse response)
            : this()
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

            Logger.Debug("Deserializing {0}", ContentType ?? "null");
            if (IsJpegResponse)
            {
                throw new NotImplementedException("need to handle jpeg response types");
            }
            if (ContentType == null && StatusCode == HttpStatusCode.Created)
            {
                TextResponse = response.ResponseUri.AbsoluteUri;
                return;
            }

            if (IsTextResponse)
            {
                try
                {
                    TextResponse = response.GetResponseStream().StreamToString();
                    // log because a text response usually means something has gone wrong (only Info level because error might be connecting to server and if log at a higher level it could continually try again, depending on setting in ServerErrorLogger)
                    Logger.Info(TextResponse);
                }
                catch
                {
                    Logger.Info("Failed to convert stream to string");
                    // do nothing is fine, ex is already logged in the extension method
                }
            }
            if (IsHtmlResponse)
            {
                // just show the status code for html for now
                TextResponse = StatusCode.ToString();
                Logger.Info(TextResponse);
                
            }
            else if (IsXmlResponse) // for now just handle like plain text
            {
                try
                {
                    TextResponse = response.GetResponseStream().StreamToString();
                    // log a warning because a text response usually means something has gone wrong
                    Logger.Info(TextResponse);
                }
                catch
                {
                    Logger.Info("Failed to convert stream to string");
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
                    Logger.Info("ServiceCallResult[T:{0}] {1}"
                        , typeof(T).Name, ex.Message);
                }
            }
            else
            {
                Logger.Info("No matching ContentType for Response Deserialization [{0}]", ContentType ?? "null");
            }
        }
        
        /// <summary>
        /// extract the info we need because the response gets
        /// disposed almost immediately.
        /// </summary>
        private void SetWebResponseDetails(WebResponse response)
        {
            StatusCode = HttpStatusCode.HttpVersionNotSupported;

            if (response == null)
            {
                Logger.Info("Response is NULL");
                return;
            }
            ContentType = response.ContentType;

            var httpResponse = response as HttpWebResponse;
            if (httpResponse == null)
                return;

            StatusCode = httpResponse.StatusCode;
            Logger.Info("Response: {0} | {1}", StatusCode.ToString(), ContentType ?? "null");

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
            get { return ContentType != null && ContentType.StartsWith(ContentTypes.JSON, StringComparison.CurrentCultureIgnoreCase); }
        }

        /// <summary>
        /// Returns true if ContentType is plain text
        /// </summary>
        public bool IsTextResponse
        {
            get
            {
                return ContentType != null && ContentType.StartsWith(ContentTypes.TEXT, StringComparison.CurrentCultureIgnoreCase);
            }
        }

        /// <summary>
        /// Returns true if ContentType is html text
        /// </summary>
        public bool IsHtmlResponse
        {
            get
            {
                return ContentType != null && ContentType.StartsWith(ContentTypes.HTML, StringComparison.CurrentCultureIgnoreCase);
            }
        }

        /// <summary>
        /// Returns true if ContentType isXML
        /// </summary>
        public bool IsXmlResponse
        {
            get { return ContentType != null && ContentType.StartsWith(ContentTypes.XML, StringComparison.CurrentCultureIgnoreCase); }
        }

        /// <summary>
        /// Returns true if ContentType is Jpeg
        /// </summary>
        public bool IsJpegResponse
        {
            get { return ContentType != null && ContentType.StartsWith(ContentTypes.JPEG, StringComparison.CurrentCultureIgnoreCase); }
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

        /// <summary>
        /// Returns web ex message or ex message if there was an ex.
        /// </summary>
        public string ExceptionMessage {
            get
            {
                if (WebException != null)
                {
                    return string.Format("{0}{1}"
                        , WebException.Message
                        , WebException.InnerException != null ? WebException.InnerException.Message : "");
                }
                if (Exception != null)
                {
                    return string.Format("{0}{1}"
                        , Exception.Message
                        , Exception.InnerException != null ? Exception.InnerException.Message : "");
                }
                return "";
            }
        }


    }
}