using Bogosoft.Testing.Objects;
using NUnit.Framework;
using Should;
using System;
using System.Threading.Tasks;

namespace Bogosoft.Caching.Tests
{
    [TestFixture, Category("Unit")]
    public class UnitTests
    {
        [TestCase]
        public async Task CacheReportsMissWhenCachedItemsLifetimeHasExpiredAsync()
        {
            var cache = new MemoryCache<CelestialBody, string>(x => x.Name);

            var venus = CelestialBody.Venus;

            var msecs = 1000;

            var lifetime = new TimeSpan(0, 0, msecs / 1000);

            (await cache.CacheAsync(venus, lifetime)).ShouldBeTrue();

            var result = await cache.GetAsync(venus.Name);

            result.HasValue.ShouldBeTrue();

            result.Value.ShouldEqual(venus);

            await Task.Delay(msecs);

            result = await cache.GetAsync(venus.Name);

            result.HasValue.ShouldBeFalse();

            result.ValueOrDefault.ShouldBeNull();
        }

        [TestCase]
        public async Task CacheReportsMissWhenGettingNonCachedItemAsync()
        {
            var cache = new MemoryCache<CelestialBody, string>(x => x.Name);

            var result = await cache.GetAsync(CelestialBody.Mars.Name);

            result.HasValue.ShouldBeFalse();

            result.ValueOrDefault.ShouldBeNull();
        }

        [TestCase]
        public async Task CanCacheItemWithMemoryCacheAsync()
        {
            var cache = new MemoryCache<CelestialBody, string>(x => x.Name);

            var earth = CelestialBody.Earth;

            var lifetime = new TimeSpan(0, 0, 5);

            (await cache.CacheAsync(earth, lifetime)).ShouldBeTrue();

            var result = await cache.GetAsync(earth.Name);

            result.HasValue.ShouldBeTrue();

            result.Value.ShouldEqual(earth);
        }
    }
}