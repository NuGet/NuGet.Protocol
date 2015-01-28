﻿using NuGet.Client;
using NuGet.PackagingCore;
using NuGet.Versioning;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Client.V2Test
{
    public class MetadataResourceTests : TestBase
    {
        [Fact]
        public async Task MetadataResource_UnZipped()
        {
            NuGet.UnzippedPackageRepository legacyRepo = new NuGet.UnzippedPackageRepository(@"C:\Program Files (x86)\Microsoft ASP.NET\ASP.NET Web Pages\v2.0\Packages");

            var sourceRepo = GetSourceRepository(legacyRepo);

            var resource = await sourceRepo.GetResource<MetadataResource>();

            // Microsoft.jQuery.Unobtrusive.Validation.2.0.30506.0

            Assert.True(await resource.Exists(new PackageIdentity("Microsoft.jQuery.Unobtrusive.Validation", NuGetVersion.Parse("2.0.30506.0")), CancellationToken.None));

            Assert.True(await resource.Exists(new PackageIdentity("Microsoft.jQuery.Unobtrusive.Validation", NuGetVersion.Parse("2.0.30506")), CancellationToken.None));

            Assert.True(await resource.Exists(new PackageIdentity("Microsoft.jQUERY.Unobtrusive.validation", NuGetVersion.Parse("2.0.30506")), CancellationToken.None));

            Assert.False(await resource.Exists(new PackageIdentity("Microsoft.jQUERY.Unobtrusive.validation", NuGetVersion.Parse("2.0.30506.1")), CancellationToken.None));
        }

        [Fact]
        public async Task MetadataResource_Local()
        {
            NuGet.LocalPackageRepository legacyRepo = new NuGet.LocalPackageRepository(@"C:\Program Files (x86)\Microsoft ASP.NET\ASP.NET Web Pages\v2.0\Packages");

            var sourceRepo = GetSourceRepository(legacyRepo);

            var resource = await sourceRepo.GetResource<MetadataResource>();

            // Microsoft.jQuery.Unobtrusive.Validation.2.0.30506.0

            Assert.True(await resource.Exists(new PackageIdentity("Microsoft.jQuery.Unobtrusive.Validation", NuGetVersion.Parse("2.0.30506.0")), CancellationToken.None));

            Assert.True(await resource.Exists(new PackageIdentity("Microsoft.jQuery.Unobtrusive.Validation", NuGetVersion.Parse("2.0.30506")), CancellationToken.None));

            Assert.True(await resource.Exists(new PackageIdentity("Microsoft.jQUERY.Unobtrusive.validation", NuGetVersion.Parse("2.0.30506")), CancellationToken.None));

            Assert.False(await resource.Exists(new PackageIdentity("Microsoft.jQUERY.Unobtrusive.validation", NuGetVersion.Parse("2.0.30506.1")), CancellationToken.None));
        }

        [Fact]
        public async Task MetadataResource_Online()
        {
            var sourceRepo = SourceRepository;

            var resource = await sourceRepo.GetResource<MetadataResource>();

            // Microsoft.jQuery.Unobtrusive.Validation.2.0.30506.0

            Assert.True(await resource.Exists(new PackageIdentity("Microsoft.jQuery.Unobtrusive.Validation", NuGetVersion.Parse("2.0.30506.0")), CancellationToken.None));

            Assert.True(await resource.Exists(new PackageIdentity("Microsoft.jQuery.Unobtrusive.Validation", NuGetVersion.Parse("2.0.30506")), CancellationToken.None));

            Assert.True(await resource.Exists(new PackageIdentity("Microsoft.jQUERY.Unobtrusive.validation", NuGetVersion.Parse("2.0.30506")), CancellationToken.None));

            Assert.False(await resource.Exists(new PackageIdentity("Microsoft.jQUERY.Unobtrusive.validation", NuGetVersion.Parse("2.0.30506.1")), CancellationToken.None));
        }

        // xunit.core.2.0.0-rc1-build2826
    }
}
