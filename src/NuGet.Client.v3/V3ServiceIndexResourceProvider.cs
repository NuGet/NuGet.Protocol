﻿using Newtonsoft.Json.Linq;
using NuGet.Data;
using NuGet.Versioning;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace NuGet.Client
{
    /// <summary>
    /// Retrieves and caches service index.json files
    /// V3ServiceIndexResource stores the json, all work is done in the provider
    /// </summary>

    [NuGetResourceProviderMetadata(typeof(V3ServiceIndexResource), "V3ServiceIndexResourceProvider", NuGetResourceProviderPositions.Last)]
    public class V3ServiceIndexResourceProvider : INuGetResourceProvider
    {
        private readonly ConcurrentDictionary<string, V3ServiceIndexResource> _cache;

        public V3ServiceIndexResourceProvider()
        {
            _cache = new ConcurrentDictionary<string, V3ServiceIndexResource>();
        }

        // TODO: refresh the file when it gets old
        public async Task<INuGetResource> Create(SourceRepository source)
        {
            V3ServiceIndexResource index = null;

            string url = source.PackageSource.Source;

            // the file type can easily rule out if we need to request the url
            if (url.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            {
                // check the cache before downloading the file
                if (!_cache.TryGetValue(url, out index))
                {
                    DataClient client = new DataClient((await source.GetResource<HttpHandlerResource>()).MessageHandler);

                    JObject json = await client.GetJObjectAsync(new Uri(url));

                    if (json != null)
                    {
                        // Use SemVer instead of NuGetVersion, the service index should always be
                        // in strict SemVer format
                        SemanticVersion version = null;
                        var status = json.Value<string>("version");
                        if (status != null && SemanticVersion.TryParse(status, out version))
                        {
                            if (version.Major == 3)
                            {
                                index = new V3ServiceIndexResource(json, DateTime.UtcNow);
                            }
                        }
                    }
                }

                // cache the value even if it is null to avoid checking it again later
                _cache.TryAdd(url, index);
            }

            return index;
        }
    }
}
