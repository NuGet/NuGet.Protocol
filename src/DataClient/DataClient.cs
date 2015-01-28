﻿using Newtonsoft.Json.Linq;
using NuGet.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Cache;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Data
{
    /// <summary>
    /// A NuGet http client with support for authentication, proxies, and caching.
    /// </summary>
    public sealed class DataClient : HttpClient
    {
        private bool _disposed;
        private readonly INuGetMessageHandlerProvider[] _modifiers;

        /// <summary>
        /// Raw constructor that allows full overriding of all caching and default DataClient behavior.
        /// </summary>
        public DataClient(HttpMessageHandler handler)
            : base(handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }
        }

        /// <summary>
        /// DataClient with the default options and caching support
        /// </summary>
        public DataClient()
            : this(CachingHandler)
        {

        }

        /// <summary>
        /// Internal constructor for building the final handler
        /// </summary>
        internal DataClient(HttpMessageHandler handler, IEnumerable<INuGetMessageHandlerProvider> modifiers)
            : this(AssembleHandlers(handler, modifiers))
        {
            _modifiers = modifiers.ToArray();
        }

        /// <summary>
        /// Default caching handler used by the data client
        /// </summary>
        public static HttpMessageHandler DefaultHandler
        {
            get
            {
                return AssembleHandlers(CachingHandler, Enumerable.Empty<INuGetMessageHandlerProvider>());
            }
        }

        /// <summary>
        /// Chain the handlers together
        /// </summary>
        private static HttpMessageHandler AssembleHandlers(HttpMessageHandler handler, IEnumerable<INuGetMessageHandlerProvider> modifiers)
        {
            // final retry logic
            // RetryHandler retryHandler = new RetryHandler(handler, 5);

            // auth & proxy
            //RequestModifierHandler modHandler = new RequestModifierHandler(handler, modifiers);

            //return modHandler;

            return handler;
        }
        
        /// <summary>
        /// Retrieve a json file
        /// </summary>
        public async Task<JObject> GetJObjectAsync(Uri address)
        {
            return await GetJObjectAsync(address, CancellationToken.None);
        }

        /// <summary>
        /// Retrieve a json file with caching
        /// </summary>
        public async Task<JObject> GetJObjectAsync(Uri address, CancellationToken token)
        {
            var response = await GetAsync(address, token);

            string json = await response.Content.ReadAsStringAsync();

            return await Task.Run(() =>
                {
                    try
                    {
                        return JObject.Parse(json);
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format(CultureInfo.InvariantCulture, "GetJObjectAsync({0})", address), e);
                    }
                });
        }

        private static HttpMessageHandler CachingHandler
        {
            get
            {
                return new WebRequestHandler()
                {
                    CachePolicy = new RequestCachePolicy(RequestCacheLevel.Default)
                };
            }
        }

        private static HttpMessageHandler NoCacheHandler
        {
            get
            {
                return new WebRequestHandler()
                {
                    CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache)
                };
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
