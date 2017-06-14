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

            var cache = new MemoryCacheAsync<CelestialBody, string>(new TimeSpan(0, 0, 0, 0, msecs));

            var venus = CelestialBody.Venus;

            (await cache.CacheAsync(venus.Name, venus)).ShouldBeTrue();

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
            var cache = new MemoryCacheAsync<CelestialBody, string>(TimeSpan.MaxValue);

            var result = await cache.GetAsync(CelestialBody.Mars.Name);

            result.HasValue.ShouldBeFalse();

            result.ValueOrDefault.ShouldBeNull();
        }

        [TestCase]
        public async Task CanCacheItemWithMemoryCacheAsync()
        {
            var cache = new MemoryCacheAsync<CelestialBody, string>(new TimeSpan(30, 0, 0));

            var earth = CelestialBody.Earth;

            var fired = false;

            cache.ItemCached += (sender, args) => fired = true;

            (await cache.CacheAsync(earth.Name, earth)).ShouldBeTrue();

            fired.ShouldBeTrue();

            (await cache.ContainsKeyAsync(earth.Name)).ShouldBeTrue();

            var result = await cache.GetAsync(earth.Name);

            result.HasValue.ShouldBeTrue();

            result.Value.ShouldEqual(earth);
        }

        [TestCase]
        public async Task CanRemovePreviouslyCachedItem()
        {
            var cache = new MemoryCacheAsync<CelestialBody, string>(new TimeSpan(30, 0, 0));

            var moon = CelestialBody.Moon;

            (await cache.CacheAsync(moon.Name, moon)).ShouldBeTrue();

            (await cache.ContainsKeyAsync(moon.Name)).ShouldBeTrue();

            (await cache.RemoveAsync(moon.Name)).ShouldBeTrue();

            (await cache.ContainsKeyAsync(moon.Name)).ShouldBeFalse();

            var result = await cache.GetAsync(moon.Name);

            result.HasValue.ShouldBeFalse();
        }
    }
}