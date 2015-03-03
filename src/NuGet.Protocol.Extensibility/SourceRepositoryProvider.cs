﻿using NuGet.Configuration;
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NuGet.Client
{
    /// <summary>
    /// SourceRepositoryProvider is the high level source for repository objects representing package sources.
    /// </summary>
    [Export(typeof(ISourceRepositoryProvider))]
    public sealed class SourceRepositoryProvider : ISourceRepositoryProvider
    {
        private static PackageSource[] DefaultPrimarySources = new [] {
            new PackageSource(NuGetConstants.V3FeedUrl, NuGetConstants.V3FeedName, isEnabled:true, isOfficial: true)
            {
                //Description = Resource.v3sourceDescription
                Description = "API v2 (legacy)"
            }
        };

        private static PackageSource[] DefaultSecondarySources = new [] {
            new PackageSource(NuGetConstants.V2FeedUrl, NuGetConstants.V2FeedName, isEnabled:true, isOfficial: true)
            {
                //Description = Resource.v2sourceDescription
                Description = "API v3"
            }
        };

        // TODO: add support for reloading sources when changes occur
        private readonly IPackageSourceProvider _packageSourceProvider;
        private IEnumerable<Lazy<INuGetResourceProvider>> _resourceProviders;
        private List<SourceRepository> _repositories;

        /// <summary>
        /// Public parameter-less constructor for SourceRepositoryProvider
        /// </summary>
        public SourceRepositoryProvider()
        {

        }

        /// <summary>
        /// Public importing constructor for SourceRepositoryProvider
        /// </summary>
        [ImportingConstructor]
        public SourceRepositoryProvider([ImportMany]IEnumerable<Lazy<INuGetResourceProvider>> resourceProviders, [Import]ISettings settings)
            : this(new PackageSourceProvider(settings, DefaultPrimarySources, DefaultSecondarySources, migratePackageSources: null), resourceProviders)
        {

        }

        /// <summary>
        /// Non-MEF constructor
        /// </summary>
        public SourceRepositoryProvider(IPackageSourceProvider packageSourceProvider, IEnumerable<Lazy<INuGetResourceProvider>> resourceProviders)
        {
            _packageSourceProvider = packageSourceProvider;
            _resourceProviders = resourceProviders;
            _repositories = new List<SourceRepository>();

            // Refresh the package sources
            Init();

            // Hook up event to refresh package sources when the package sources changed
            packageSourceProvider.PackageSourcesSaved += (sender, e) =>
            {
                Init();
            };
        }

        /// <summary>
        /// Retrieve repositories
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SourceRepository> GetRepositories()
        {
            return _repositories;
        }

        /// <summary>
        /// Create a repository for one time use.
        /// </summary>
        public SourceRepository CreateRepository(PackageSource source)
        {
            return new SourceRepository(source, _resourceProviders);
        }

        public IPackageSourceProvider PackageSourceProvider
        {
            get { return _packageSourceProvider; }
        }

        private void Init()
        {
            _repositories.Clear();
            foreach (var source in _packageSourceProvider.LoadPackageSources())
            {
                if (source.IsEnabled)
                {
                    SourceRepository sourceRepo = new SourceRepository(source, _resourceProviders);
                    _repositories.Add(sourceRepo);
                }
            }
        }
    }
}