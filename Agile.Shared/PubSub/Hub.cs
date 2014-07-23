using System;
using System.Collections.Generic;
using System.Linq;
using Agile.Diagnostics.Logging;

namespace Agile.Shared.PubSub
{
    public static class Hub
    {
        private static readonly List<Handler> subscribers = new List<Handler>();
        public static List<Handler> Subscribers
        {
            get { return subscribers; }
        }

        public static void Subscribe(string eventName, System.Action action, string owner)
        {
            var handler = new Handler { EventName = eventName, Do = action, Owner = owner };
            subscribers.Add(handler);
        }

        public static void Subscribe<T>(string eventName, Action<T> action, string owner)
        {
            var handler = new Handler { EventName = eventName, Do = action, Owner = owner };
            subscribers.Add(handler);
        }

        public static void Subscribe<T, T1>(string eventName, Action<T, T1> action, string owner)
        {
            var handler = new Handler { EventName = eventName, Do = action, Owner = owner };
            subscribers.Add(handler);
        }

        public static void Subscribe<T, T1, T2>(string eventName, Action<T, T1, T2> action, string owner)
        {
            var handler = new Handler { EventName = eventName, Do = action, Owner = owner };
            subscribers.Add(handler);
        }

        /// <summary>
        /// Unsubscribe the owner from one specific event only
        /// </summary>
        public static void UnSubscribe(string owner, string eventName)
        {
            subscribers.RemoveAll(h =>
                                  h.Owner.Equals(owner, StringComparison.OrdinalIgnoreCase) &&
                                  (string.IsNullOrEmpty(eventName) || h.EventName.Equals(eventName, StringComparison.OrdinalIgnoreCase)));
        }

        /// <summary>
        /// Unsubscribe the owner from ALL events
        /// </summary>
        public static void UnSubscribe(string owner)
        {
            UnSubscribe(owner, null);
        }

        /// <summary>
        /// Publish an event without any parameters
        /// </summary>
        public static void Publish(string eventName)
        {
            Logger.Debug("publish:{0}", eventName);
            var subs = subscribers.Where(h => h.EventName.Equals(eventName, StringComparison.OrdinalIgnoreCase) && h.Do is System.Action).ToList();
            if (subs.Count == 0)
                return;
            
            foreach(Handler handler in subs)
            {
                try
                {
                    if (handler == null)
                        return;
                    ((System.Action) handler.Do)();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, string.Format("PUBLISHing {0}", eventName));
                }
            }
        }

        public static void Publish<T>(string eventName, T data)
        {
            Logger.Debug("publish:{0}", eventName);
            var subs = subscribers.Where(h => h.EventName.Equals(eventName, StringComparison.OrdinalIgnoreCase) && h.Do is Action<T>).ToList();
            if (subs.Count == 0)
                return;

            foreach (Handler handler in subs)
            {
                try
                {
                    if (handler == null)
                        return;
                    ((Action<T>) handler.Do)(data);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, string.Format("PUBLISHing {0}", eventName));
                }
            }
        }

        public static void Publish<T, T1>(string eventName, T data, T1 data1)
        {
            Logger.Debug("publish:{0}", eventName);
            var subs = subscribers.Where(h => h.EventName.Equals(eventName, StringComparison.OrdinalIgnoreCase) && h.Do is Action<T, T1>).ToList();
            if (subs.Count == 0)
                return;

            foreach (Handler handler in subs)
            {
                try
                {
                    if (handler == null)
                        return;
                    ((Action<T, T1>) handler.Do)(data, data1);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, string.Format("PUBLISHing {0}", eventName));
                }
            }
        }

        public static void Publish<T, T1, T2>(string eventName, T data, T1 data1, T2 data2)
        {
            Logger.Debug("publish:{0}", eventName);
            var subs = subscribers.Where(h => h.EventName.Equals(eventName, StringComparison.OrdinalIgnoreCase) && h.Do is Action<T, T1, T2>).ToList();
            if (subs.Count == 0)
                return;

            foreach (Handler handler in subs)
            {
                try
                {
                    if (handler == null)
                        return;
                    ((Action<T, T1, T2>) handler.Do)(data, data1, data2);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, string.Format("PUBLISHing {0}", eventName));
                }
            }
        }
    }

    public class Handler
    {
        public string Owner { get; set; }
        public string EventName { get; set; }
        public object Do { get; set; }
    }
}