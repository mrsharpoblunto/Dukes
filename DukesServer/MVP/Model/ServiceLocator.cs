using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DukesServer.MVP.Model
{
    internal class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        private ServiceLocator()
        {
        }

        public static void Register<T>(T t)
        {
            if (_services.ContainsKey(typeof(T)))
            {
                _services.Remove(typeof(T));
            }
            _services.Add(typeof(T), t);
        }

        public static T Get<T>()
        {
            if (_services.ContainsKey(typeof(T)))
            {
                return (T)_services[typeof(T)];
            }
            else
            {
                return default(T);
            }
        }
    }
}