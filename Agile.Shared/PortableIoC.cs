using System;
using System.Collections;
using System.Collections.Generic;
using Agile.Diagnostics.Logging;

namespace Agile.Shared.IoC
{
    /// <summary>
    /// Very simplistic IoC container that will work in a PCL library targeting WP8, IoS and Android (Xamarin).
    /// Uses factories (or delegates) to get the instantiated type instead of using reflection.
    /// </summary>
    public static class Container
    {
        private static readonly List<IocItem> Items = new List<IocItem>();

        /// <summary>
        /// Reset the container, i.e. clear all registered items 
        /// </summary>
        public static void Reset()
        {
            // can't call dispose because we don't know type T of each item (should really only be called from tests anyway)
            Logger.Info("Reset IoC Container, removing {0} items", Items.Count);
            Items.Clear();
        }

        /// <summary>
        /// Returns a count of all registered factories/instances
        /// </summary>
        public static int Count 
        {
            get { return Items.Count; }
        }

        /// <summary>
        /// Register a new factory method for the interface
        /// </summary>
        public static void Register<T>(Func<T> factory) where T : class
        {
            var interfaceName = typeof(T).Name;
            // create the new item
            var newitem = new IocItem { Factory = factory, InterfaceName = interfaceName };

            RegisterInstance<T>(interfaceName, newitem);
        }

        /// <summary>
        /// Register an instance
        /// </summary>
        /// <typeparam name="T">DO provide the interface (the compiler will allow you to NOT provide it)
        /// Leaving it out will register the instance with the concrete type name instead of the interface name.</typeparam>
        /// <param name="instance"></param>
        public static void RegisterInstance<T>(T instance) where T : class
        {
            var interfaceName = typeof(T).Name;
            // create the new item
            var newitem = new IocItem { InterfaceName = interfaceName };
            newitem.SetInstance(instance);

            RegisterInstance<T>(interfaceName, newitem);
        }

        private static void RegisterInstance<T>(string interfaceName, IocItem newitem) where T : class
        {
            for (int index = 0; index < Items.Count; index++)
            {
                var existing = Items[index];
                // if it already exists and we get a new register, remove the existing, log removal, and add new register
                if (!existing.InterfaceName.Equals(interfaceName, StringComparison.OrdinalIgnoreCase))
                    continue;

                var existingInstance = existing.GetInstance<T>();
                var existingHadInstance = existingInstance != null;
                Logger.Info("IoC: Replacing existing {0}.{1}"
                            , existing.InterfaceName
                            , existingHadInstance ? " (did have an existing instance)" : "");
                Items.Remove(existing);

                // Dispose if disposable
                if (existingHadInstance)
                {
                    var disposable = existingInstance as IDisposable;
                    if (disposable != null)
                    {
                        Logger.Info("IoC: Disposing {0}", existing.InterfaceName);
                        disposable.Dispose();
                    }
                }
                break;
            }

            // then just add it
            Logger.Debug("IoC: Registered {0}", interfaceName);
            Items.Add(newitem);
        }


        public static T Resolve<T>() where T : class
        {
            var interfaceName = typeof(T).Name;

            // find the registration
            for (int index = 0; index < Items.Count; index++)
            {
                var existing = Items[index];
                // try to find existing instance
                if (!existing.InterfaceName.Equals(interfaceName, StringComparison.OrdinalIgnoreCase))
                    continue;

                // matches so if there is an existing instance just return it
                var instance = existing.GetInstance<T>();
                if (instance != null)
                    return instance;

                // if not need to run the factory
                existing.RunFactory<T>();
                return existing.GetInstance<T>();
            }

            // failed to find anything
            var message = string.Format("IoC: Nothing registered for {0}", interfaceName);
            Logger.Warning(message);
            throw new Exception(message);
        }

    }

    internal class IocItem
    {
        /// <summary>
        /// Func T
        /// </summary>
        internal object Factory { get; set; } 
        /// <summary>
        /// Instance of T
        /// </summary>
        private object Instance { get; set; }
        internal string InterfaceName { get; set; }

        internal T GetInstance<T>() where T : class
        {
            return Instance as T;
        }

        internal void SetInstance<T>(T instance)
        {
            Instance = instance;
        }

        internal bool HasInstanceOfType<T>() where T : class
        {
            return GetInstance<T>() != null;
        }

        public void RunFactory<T>()
        {
            var factory = Factory as Func<T>;
            if (factory == null)
                throw new Exception(string.Format("failed to cast Factory T {0} : {1}", typeof(T).Name, InterfaceName));
            Instance = factory.Invoke();
            Logger.Debug("Created instance of {0}", InterfaceName);
        }
    }

}

