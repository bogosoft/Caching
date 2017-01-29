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
            var msecs = 1000;

            var cache = new AsyncCache<CelestialBody, string>(x => x.Name, new TimeSpan(0, 0, 0, 0, msecs));

            var venus = CelestialBody.Venus;

            (await cache.CacheAsync(venus)).ShouldBeTrue();

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
            var cache = new AsyncCache<CelestialBody, string>(x => x.Name, TimeSpan.MaxValue);

            var result = await cache.GetAsync(CelestialBody.Mars.Name);

            result.HasValue.ShouldBeFalse();

            result.ValueOrDefault.ShouldBeNull();
        }

        [TestCase]
        public async Task CanCacheItemWithMemoryCacheAsync()
        {
            var cache = new AsyncCache<CelestialBody, string>(x => x.Name, new TimeSpan(30, 0, 0));

            var earth = CelestialBody.Earth;

            (await cache.CacheAsync(earth)).ShouldBeTrue();

            var result = await cache.GetAsync(earth.Name);

            result.HasValue.ShouldBeTrue();

            result.Value.ShouldEqual(earth);
        }
    }
}