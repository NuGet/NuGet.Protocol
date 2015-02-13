﻿using NuGet.PackagingCore;
using NuGet.Versioning;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Client
{
    /// <summary>
    /// Basic metadata
    /// </summary>
    public abstract class MetadataResource : INuGetResource
    {
        /// <summary>
        /// Get all versions of a package
        /// </summary>
        public async Task<IEnumerable<NuGetVersion>> GetVersions(string packageId, CancellationToken token)
        {
            return await GetVersions(packageId, true, token);
        }

        /// <summary>
        /// Get all versions of a package
        /// </summary>
        public abstract Task<IEnumerable<NuGetVersion>> GetVersions(string packageId, bool includePrerelease, CancellationToken token);

        public abstract Task<IEnumerable<KeyValuePair<string, NuGetVersion>>> GetLatestVersions(IEnumerable<string> packageIds, bool includePrerelease, CancellationToken token);

        public async Task<NuGetVersion> GetLatestVersion(string packageId, bool includePrerelease, bool includeUnlisted, CancellationToken token)
        {
            var results = await GetLatestVersions(new string[] { packageId }, includePrerelease, token);
            var result = results.SingleOrDefault();

            if (!result.Equals(default(KeyValuePair<string, bool>)))
            {
                return result.Value;
            }

            return null;
        }
    }
}
