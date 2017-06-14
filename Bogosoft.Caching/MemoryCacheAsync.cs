using Bogosoft.Maybe;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Caching
{
    /// <summary>
    /// An in-memory, concurrent implementation of the <see cref="ICacheAsync{TItem, TKey}"/> contract.
    /// </summary>
    /// <typeparam name="TItem">
    /// The type of the object that can be cached.
    /// </typeparam>
    /// <typeparam name="TKey">
    /// The type of the object that serves as a lookup key for cached objects.
    /// </typeparam>
    public sealed class MemoryCacheAsync<TItem, TKey> : ICacheAsync<TItem, TKey>
    {
        MemoryCache<TItem, TKey> cache;

        /// <summary>
        /// Occurs when an item is successfully cached.
        /// </summary>
        public event ItemCachedEventHandler<TItem, TKey> ItemCached;

        /// <summary>
        /// Create a new instance of the <see cref="MemoryCacheAsync{TItem, TKey}"/> class.
        /// </summary>
        public MemoryCacheAsync()
            : this(TimeSpan.MaxValue, () => DateTimeOffset.Now)
        {
        }

        /// <summary>
        /// Create a new instance of the <see cref="MemoryCacheAsync{TItem, TKey}"/> class.
        /// </summary>
        /// <param name="ttl">
        /// A value corresponding to the time to live for an object of the cached item type
        /// before it is considered stale.
        /// </param>
        public MemoryCacheAsync(TimeSpan ttl)
            : this(ttl, () => DateTimeOffset.Now)
        {
        }

        /// <summary>
        /// Create a new instance of the <see cref="MemoryCacheAsync{TItem, TKey}"/> class.
        /// </summary>
        /// <param name="ttl">
        /// A value corresponding to the time to live for an object of the cached item type
        /// before it is considered stale.
        /// </param>
        /// <param name="dates">
        /// A provider for generating <see cref="DateTimeOffset"/> objects when a reference date is needed,
        /// i.e. when a staleness check is performed or when an expiration date needs to be calculated.
        /// </param>
        public MemoryCacheAsync(TimeSpan ttl, Func<DateTimeOffset> dates)
        {
            cache = new MemoryCache<TItem, TKey>(ttl, dates);
        }

        /// <summary>
        /// Cache an object of the item type.
        /// </summary>
        /// <param name="key">
        /// An object of the key type which will serve as the key by which an item is cached.
        /// </param>
        /// <param name="item">An object of the cached item type.</param>
        /// <param name="token">a <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// A value indicating whether or not the caching operation succeeded.
        /// </returns>
        public Task<bool> CacheAsync(TKey key, TItem item, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var success = cache.Cache(key, item);

            if (success && ItemCached != null)
            {
                ItemCached.Invoke(this, new ItemCachedEventArgs<TItem, TKey>(key, item));
            }

            return Task.FromResult(success);
        }

        /// <summary>
        /// Clear the current cache of all cached items.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// A value corresponding to the number of cached items cleared.
        /// </returns>
        public Task<int> ClearAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            return Task.FromResult(cache.Clear());
        }

        /// <summary>
        /// Determine if the current cache currently contains a given key.
        /// </summary>
        /// <param name="key">
        /// An object of the key type.
        /// </param>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// A value indicating whether or not the current cache contains the given key.
        /// </returns>
        public Task<bool> ContainsKeyAsync(TKey key, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            return Task.FromResult(cache.Contains(key));
        }

        /// <summary>
        /// Retrieve a previously cached item from the current cache by a given key.
        /// </summary>
        /// <param name="key">An object of the key type.</param>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// A value that might contain an object of the cached item type.
        /// </returns>
        public Task<IMayBe<TItem>> GetAsync(TKey key, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            return Task.FromResult(cache.Get(key));
        }

        /// <summary>
        /// Attempt to remove a previously cached item from the current cache.
        /// </summary>
        /// <param name="key">
        /// A value corresponding to the key that the item was originally cached against.
        /// </param>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// A value indicating whether or not the removal operation was successful.
        /// </returns>
        public Task<bool> RemoveAsync(TKey key, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            return Task.FromResult(cache.Remove(key));
        }
    }
}