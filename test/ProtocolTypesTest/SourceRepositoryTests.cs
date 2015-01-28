﻿using NuGet.Client;
using NuGet.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ProtocolTypesTest
{
    public class SourceRepositoryTests
    {
        [Fact]
        public void SourceRepository_Counts()
        {
            PackageSource source = new PackageSource("http://source");

            var A = new TestProvider(null);
            var B = new TestProvider(null);
            var C = new TestProvider(null);

            var A2 = new TestProvider(null);
            var B2 = new TestProvider(null);
            var C2 = new TestProvider(null);

            List<KeyValuePair<INuGetResourceProviderMetadata, INuGetResourceProvider>> providers = new List<KeyValuePair<INuGetResourceProviderMetadata, INuGetResourceProvider>>()
            {
                new KeyValuePair<INuGetResourceProviderMetadata, INuGetResourceProvider>(new NuGetResourceProviderMetadata(typeof(TestResource), "A", NuGetResourceProviderPositions.First), A),
                new KeyValuePair<INuGetResourceProviderMetadata, INuGetResourceProvider>(new NuGetResourceProviderMetadata(typeof(TestResource), "B"), B),
                new KeyValuePair<INuGetResourceProviderMetadata, INuGetResourceProvider>(new NuGetResourceProviderMetadata(typeof(TestResource), "C", NuGetResourceProviderPositions.Last), C),

                new KeyValuePair<INuGetResourceProviderMetadata, INuGetResourceProvider>(new NuGetResourceProviderMetadata(typeof(TestResource), "A2", "A"), A2),
                new KeyValuePair<INuGetResourceProviderMetadata, INuGetResourceProvider>(new NuGetResourceProviderMetadata(typeof(TestResource), "B2", "B"), B2),
                new KeyValuePair<INuGetResourceProviderMetadata, INuGetResourceProvider>(new NuGetResourceProviderMetadata(typeof(TestResource), "C2", "C"), C2),
            };

            SourceRepository repo = new SourceRepository(source, providers);

            // verify order - work backwards
            Assert.Null(repo.GetResource<TestResource>().Result);

            C.Data = "C";
            Assert.Equal("C", repo.GetResource<TestResource>().Result.Data);

            C2.Data = "C2";
            Assert.Equal("C2", repo.GetResource<TestResource>().Result.Data);

            B.Data = "B";
            Assert.Equal("B", repo.GetResource<TestResource>().Result.Data);

            B.Data = "B2";
            Assert.Equal("B2", repo.GetResource<TestResource>().Result.Data);

            A.Data = "A";
            Assert.Equal("A", repo.GetResource<TestResource>().Result.Data);

            A2.Data = "A2";
            Assert.Equal("A2", repo.GetResource<TestResource>().Result.Data);
        }


        [Fact]
        public void SourceRepository_EmptyType()
        {
            PackageSource source = new PackageSource("http://source");

            var A = new TestProvider(null);
            var B = new TestProvider(null);
            var C = new TestProvider(null);

            List<KeyValuePair<INuGetResourceProviderMetadata, INuGetResourceProvider>> providers = new List<KeyValuePair<INuGetResourceProviderMetadata, INuGetResourceProvider>>()
            {
                new KeyValuePair<INuGetResourceProviderMetadata, INuGetResourceProvider>(new NuGetResourceProviderMetadata(typeof(TestResource), "A", NuGetResourceProviderPositions.First), A),
                new KeyValuePair<INuGetResourceProviderMetadata, INuGetResourceProvider>(new NuGetResourceProviderMetadata(typeof(TestResource), "B"), B),
                new KeyValuePair<INuGetResourceProviderMetadata, INuGetResourceProvider>(new NuGetResourceProviderMetadata(typeof(TestResource), "C", NuGetResourceProviderPositions.Last), C),
            };

            SourceRepository repo = new SourceRepository(source, providers);

            // verify order - work backwards
            Assert.Null(repo.GetResource<TestResource2>().Result);
        }

        [Fact]
        public void SourceRepository_Empty()
        {
            PackageSource source = new PackageSource("http://source");

            List<KeyValuePair<INuGetResourceProviderMetadata, INuGetResourceProvider>> providers = new List<KeyValuePair<INuGetResourceProviderMetadata, INuGetResourceProvider>>()
            {
            };

            SourceRepository repo = new SourceRepository(source, providers);

            // verify order - work backwards
            Assert.Null(repo.GetResource<TestResource>().Result);
        }

        [Fact]
        public void SourceRepository_SortTest()
        {
            PackageSource source = new PackageSource("http://source");

            var A = new TestProvider(null);
            var B = new TestProvider(null);
            var C = new TestProvider(null);

            List<KeyValuePair<INuGetResourceProviderMetadata, INuGetResourceProvider>> providers = new List<KeyValuePair<INuGetResourceProviderMetadata, INuGetResourceProvider>>()
            {
                new KeyValuePair<INuGetResourceProviderMetadata, INuGetResourceProvider>(new NuGetResourceProviderMetadata(typeof(TestResource), "A", NuGetResourceProviderPositions.First), A),
                new KeyValuePair<INuGetResourceProviderMetadata, INuGetResourceProvider>(new NuGetResourceProviderMetadata(typeof(TestResource), "B"), B),
                new KeyValuePair<INuGetResourceProviderMetadata, INuGetResourceProvider>(new NuGetResourceProviderMetadata(typeof(TestResource), "C", NuGetResourceProviderPositions.Last), C),
            };

            SourceRepository repo = new SourceRepository(source, providers);

            // verify order - work backwards
            Assert.Null(repo.GetResource<TestResource>().Result);

            C.Data = "C";
            Assert.Equal("C", repo.GetResource<TestResource>().Result.Data);

            B.Data = "B";
            Assert.Equal("B", repo.GetResource<TestResource>().Result.Data);

            A.Data = "A";
            Assert.Equal("A", repo.GetResource<TestResource>().Result.Data);
        }

        [Fact]
        public void SourceRepository_SortTest2()
        {
            PackageSource source = new PackageSource("http://source");

            var empty = new TestProvider(null);
            var A = new TestProvider(null);
            var B = new TestProvider(null);
            var C = new TestProvider(null);
            var D = new TestProvider(null);
            var E = new TestProvider(null);

            List<KeyValuePair<INuGetResourceProviderMetadata, INuGetResourceProvider>> providers = new List<KeyValuePair<INuGetResourceProviderMetadata, INuGetResourceProvider>>()
            {
                new KeyValuePair<INuGetResourceProviderMetadata, INuGetResourceProvider>(new NuGetResourceProviderMetadata(typeof(TestResource)), empty),
                new KeyValuePair<INuGetResourceProviderMetadata, INuGetResourceProvider>(new NuGetResourceProviderMetadata(typeof(TestResource), "B", NuGetResourceProviderPositions.First), B),
                new KeyValuePair<INuGetResourceProviderMetadata, INuGetResourceProvider>(new NuGetResourceProviderMetadata(typeof(TestResource), "C"), C),
                new KeyValuePair<INuGetResourceProviderMetadata, INuGetResourceProvider>(new NuGetResourceProviderMetadata(typeof(TestResource), "A", "B"), A),
                new KeyValuePair<INuGetResourceProviderMetadata, INuGetResourceProvider>(new NuGetResourceProviderMetadata(typeof(TestResource), "E", NuGetResourceProviderPositions.Last), E),
                new KeyValuePair<INuGetResourceProviderMetadata, INuGetResourceProvider>(new NuGetResourceProviderMetadata(typeof(TestResource), "D", "E"), D),
            };

            SourceRepository repo = new SourceRepository(source, providers);

            // verify order - work backwards
            Assert.Null(repo.GetResource<TestResource>().Result);

            empty.Data = "EMPTY";
            Assert.Equal("EMPTY", repo.GetResource<TestResource>().Result.Data);

            E.Data = "E";
            Assert.Equal("E", repo.GetResource<TestResource>().Result.Data);

            D.Data = "D";
            Assert.Equal("D", repo.GetResource<TestResource>().Result.Data);

            C.Data = "C";
            Assert.Equal("C", repo.GetResource<TestResource>().Result.Data);

            B.Data = "B";
            Assert.Equal("B", repo.GetResource<TestResource>().Result.Data);

            A.Data = "A";
            Assert.Equal("A", repo.GetResource<TestResource>().Result.Data);
        }

        [Fact]
        public void SourceRepository_SortTest3()
        {
            PackageSource source = new PackageSource("http://source");

            var A = new TestProvider(null);
            var B = new TestProvider(null);
            var C = new TestProvider(null);

            List<KeyValuePair<INuGetResourceProviderMetadata, INuGetResourceProvider>> providers = new List<KeyValuePair<INuGetResourceProviderMetadata, INuGetResourceProvider>>()
            {
                new KeyValuePair<INuGetResourceProviderMetadata, INuGetResourceProvider>(new NuGetResourceProviderMetadata(typeof(TestResource), "A", new string[] { NuGetResourceProviderPositions.Last, "C", "B" }), A),
                new KeyValuePair<INuGetResourceProviderMetadata, INuGetResourceProvider>(new NuGetResourceProviderMetadata(typeof(TestResource), "C", NuGetResourceProviderPositions.Last), C),
                new KeyValuePair<INuGetResourceProviderMetadata, INuGetResourceProvider>(new NuGetResourceProviderMetadata(typeof(TestResource), "B", new string[] { NuGetResourceProviderPositions.Last, "C" }), B),
            };

            SourceRepository repo = new SourceRepository(source, providers);

            // verify order - work backwards
            Assert.Null(repo.GetResource<TestResource>().Result);

            C.Data = "C";
            Assert.Equal("C", repo.GetResource<TestResource>().Result.Data);

            B.Data = "B";
            Assert.Equal("B", repo.GetResource<TestResource>().Result.Data);

            A.Data = "A";
            Assert.Equal("A", repo.GetResource<TestResource>().Result.Data);
        }

        // helper classes
        private class TestProvider : INuGetResourceProvider
        {
            public string Data { get; set; }

            public TestProvider(string data)
            {
                Data = data;
            }

            public async Task<INuGetResource> Create(SourceRepository source)
            {
                INuGetResource resource = null;
                if (Data != null)
                {
                    resource = new TestResource(Data);
                }

                return await Task.FromResult(resource);
            }
        }

        private class TestResource : INuGetResource
        {
            public string Data { get; set; }

            public TestResource(string data)
            {
                Data = data;
            }
        }

        private class TestResource2 : INuGetResource
        {
            public string Data { get; set; }

            public TestResource2(string data)
            {
                Data = data;
            }
        }


    }
}
