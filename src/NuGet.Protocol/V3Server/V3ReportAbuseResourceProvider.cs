﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Protocol
{
    public class V3ReportAbuseResourceProvider : ResourceProvider
    {
        public V3ReportAbuseResourceProvider()
            : base(typeof(V3ReportAbuseResource), "V3ReportAbuseResource", NuGetResourceProviderPositions.Last)
        {

        }

        public override async Task<Tuple<bool, INuGetResource>> TryCreate(SourceRepository source, CancellationToken token)
        {
            V3ReportAbuseResource resource = null;
            var serviceIndex = await source.GetResourceAsync<V3ServiceIndexResource>(token);
            if (serviceIndex != null)
            {
                IEnumerable<Uri> templateUrls = serviceIndex[ServiceTypes.ReportAbuse];
                if (templateUrls != null && templateUrls.Any())
                {
                    resource = new V3ReportAbuseResource(templateUrls);
                }
            }

            return new Tuple<bool, INuGetResource>(resource != null, resource);
        }
    }
}