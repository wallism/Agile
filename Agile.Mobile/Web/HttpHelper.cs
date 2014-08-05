using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Diagnostics.Logging;
using Agile.Shared.PubSub;
using Newtonsoft.Json;

namespace Agile.Mobile.Web
{
    /// <summary>
    /// http helper stuff
    /// </summary>
    public static class HttpHelper
    {
        /// <summary>
        /// Call this early in bootstrapping (depending on how you're managing security...)
        /// Subs for: 
        ///     - Login Completed (needed for http calls)
        /// </summary>
        public static void RegisterHubSubscriptions()
        {

            Hub.Subscribe(HubEvents.LoginCompletedSuccessfully
                , delegate(string userName, string password)
                {
                    UserName = userName;
                    Password = password;
                }, "HttpHelper");
        }


        public const string POST = "POST";
        public const string PUT = "PUT";
        public const string GET = "GET";
        public const string HEAD = "HEAD";

        public static string Platform { get; set; }
        public static string UserName { get; set; }
        public static string Password { get; set; }
        /// <summary>
        /// Make sure you're serializing the right type (you usually want to serialize a dto
        /// , which means mapping it before calling this method)
        /// </summary>
        public static byte[] SerializeToJsonThenByteArray<T>(T value)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));
        }

        /// <summary>
        /// Result of this should be Json
        /// </summary>
        private static string DeSerialize(byte[] data)
        {
            try
            {
                return Encoding.UTF8.GetString(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "DeSerialize to string");
                return string.Empty;
            }
        }

        /// <summary>
        /// Deserialize to the given type
        /// </summary>
        public static T DeSerialize<T>(byte[] data)
            where T : class
        {
            var json = DeSerialize(data);
            if (string.IsNullOrEmpty(json))
                return null;
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "DeSerialize to {0}", typeof(T).Name);
                return null;
            }
        }
    }
}
