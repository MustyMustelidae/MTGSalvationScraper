//===============================================================================
// TinyIoC
//
// An easy to use, hassle free, Inversion of Control Container for small projects
// and beginners alike.
//
// https://github.com/grumpydev/TinyIoC
//===============================================================================
// Copyright © Steven Robbins.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#region Preprocessor Directives
// Uncomment this line if you want the container to automatically
// register the TinyMessenger messenger/event aggregator
//#define TINYMESSENGER

// Preprocessor directives for enabling/disabling functionality
// depending on platform features. If the platform has an appropriate
// #DEFINE then these should be set automatically below.
#define EXPRESSIONS                         // Platform supports System.Linq.Expressions
#define APPDOMAIN_GETASSEMBLIES             // Platform supports getting all assemblies from the AppDomain object
#define UNBOUND_GENERICS_GETCONSTRUCTORS    // Platform supports GetConstructors on unbound generic types
#define GETPARAMETERS_OPEN_GENERICS         // Platform supports GetParameters on open generics
#define RESOLVE_OPEN_GENERICS               // Platform supports resolving open generics

//// NETFX_CORE
//#if NETFX_CORE
//#endif

// CompactFramework / Windows Phone 7
// By default does not support System.Linq.Expressions.
// AppDomain object does not support enumerating all assemblies in the app domain.
#if PocketPC || WINDOWS_PHONE
#undef EXPRESSIONS
#undef APPDOMAIN_GETASSEMBLIES
#undef UNBOUND_GENERICS_GETCONSTRUCTORS
#endif

// PocketPC has a bizarre limitation on enumerating parameters on unbound generic methods.
// We need to use a slower workaround in that case.
#if PocketPC
#undef GETPARAMETERS_OPEN_GENERICS
#undef RESOLVE_OPEN_GENERICS
#endif

#if SILVERLIGHT
#undef APPDOMAIN_GETASSEMBLIES
#endif

#if NETFX_CORE
#undef APPDOMAIN_GETASSEMBLIES
#undef RESOLVE_OPEN_GENERICS
#endif

#endregion
namespace TinyIoC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
#if EXPRESSIONS

#endif

#if NETFX_CORE
	using System.Threading.Tasks;
	using Windows.Storage.Search;
    using Windows.Storage;
	using Windows.UI.Xaml.Shapes;
#endif

    #region SafeDictionary
    public class SafeDictionary<TKey, TValue> : IDisposable
    {
        private readonly object _Padlock = new object();
        private readonly Dictionary<TKey, TValue> _Dictionary = new Dictionary<TKey, TValue>();

        public TValue this[TKey key]
        {
            set
            {
                lock (_Padlock)
                {
                    TValue current;
                    if (_Dictionary.TryGetValue(key, out current))
                    {
                        var disposable = current as IDisposable;

                        if (disposable != null)
                            disposable.Dispose();
                    }

                    _Dictionary[key] = value;
                }
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            lock (_Padlock)
            {
                return _Dictionary.TryGetValue(key, out value);
            }
        }

        public bool Remove(TKey key)
        {
            lock (_Padlock)
            {
                return _Dictionary.Remove(key);
            }
        }

        public void Clear()
        {
            lock (_Padlock)
            {
                _Dictionary.Clear();
            }
        }

        public IEnumerable<TKey> Keys
        {
            get
            {
                return _Dictionary.Keys;
            }
        }
        #region IDisposable Members

        public void Dispose()
        {
            lock (_Padlock)
            {
                var disposableItems = from item in _Dictionary.Values
                                      where item is IDisposable
                                      select item as IDisposable;

                foreach (var item in disposableItems)
                {
                    item.Dispose();
                }
            }

            GC.SuppressFinalize(this);
        }

        #endregion
    }
    #endregion

    #region Extensions

    // @mbrit - 2012-05-22 - shim for ForEach call on List<T>...
#if NETFX_CORE
	internal static class ListExtender
	{
		internal static void ForEach<T>(this List<T> list, Action<T> callback)
		{
			foreach (T obj in list)
				callback(obj);
		}
	}
#endif

    #endregion

    #region TinyIoC Exception Types

    #endregion

    #region Public Setup / Settings Classes

    #endregion
}

// reverse shim for WinRT SR changes...
#if !NETFX_CORE
#endif
