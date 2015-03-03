﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NuGet.Protocol.Data
{
    public static class DataTraceSources
    {
#if !ASPNETCORE50
        public static readonly TraceSource DataClient = new TraceSource(typeof(DataClient).FullName);

        public static IEnumerable<TraceSource> GetAllSources()
        {
            return typeof(DataTraceSources).GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(f => typeof(TraceSource).IsAssignableFrom(f.FieldType))
                .Select(f => (TraceSource)f.GetValue(null));
        }
#endif

        internal static void Verbose(string format, params string[] message)
        {
#if !ASPNETCORE50
            DataClient.TraceEvent(TraceEventType.Verbose, 0, String.Format(CultureInfo.InvariantCulture, format, message));
#endif
        }

        internal static void Verbose(string message)
        {
#if !ASPNETCORE50
            DataClient.TraceEvent(TraceEventType.Verbose, 0, message);
#endif
        }
    }
}