using Bogosoft.Maybe;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Caching
{
    /// <summary>
    /// An in-memory, concurrent implementation of the <see cref="ICache{TItem, TKey}"/> contract.
    /// </summary>
    /// <typeparam name="TItem">
    /// The type of the object that can be cached.
    /// </typeparam>
    /// <typeparam name="TKey">
    /// The type of the object that serves as a lookup key for cached objects.
    /// </typeparam>
    public sealed class AsyncCache<TItem, TKey> : ICache<TItem, TKey>
    {
        Func<DateTimeOffset> dates;

        Dictionary<TKey, CachedItem<TItem>> items = new Dictionary<TKey, CachedItem<TItem>>();

        Func<TItem, TKey> keySelector;

        ReaderWriterLockSlim @lock = new ReaderWriterLockSlim();

        TimeSpan ttl;

        /// <summary>
        /// Create a new instance of the <see cref="AsyncCache{TItem, TKey}"/> class
        /// with a given key extraction strategy.
        /// </summary>
        /// <param name="keySelector">
        /// A strategy for extracting a key from a given object of the cached item type.
        /// </param>
        /// <param name="ttl">
        /// A value corresponding to the time to live for an object of the cached item type
        /// before it is considered stale.
        /// </param>
        public AsyncCache(Func<TItem, TKey> keySelector, TimeSpan ttl)
            : this(keySelector, ttl, () => DateTimeOffset.Now)
        {
        }

        /// <summary>
        /// Create a new instance of the <see cref="AsyncCache{TItem, TKey}"/> class with a given key extraction
        /// strategy and a given <see cref="DateTimeOffset"/> provider.
        /// </summary>
        /// <param name="keySelector"></param>
        /// <param name="ttl">
        /// A value corresponding to the time to live for an object of the cached item type
        /// before it is considered stale.
        /// </param>
        /// <param name="dates"></param>
        public AsyncCache(Func<TItem, TKey> keySelector, TimeSpan ttl, Func<DateTimeOffset> dates)
        {
            this.dates = dates;
            this.keySelector = keySelector;
            this.ttl = ttl;
        }

        /// <summary>
        /// Cache an object of the item type.
        /// </summary>
        /// <param name="item">An object of the cached item type.</param>
        /// <param name="token">a <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// A value indicating whether or not the caching operation succeeded.
        /// </returns>
        public Task<bool> CacheAsync(TItem item, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            @lock.EnterWriteLock();

            try
            {
                var key = keySelector.Invoke(item);

                items[key] = new CachedItem<TItem>
                {
                    Expiry = dates.Invoke().Add(ttl),
                    Item = item
                };

                return Task.FromResult(items.ContainsKey(key));
            }
            finally
            {
                @lock.ExitWriteLock();
            }
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

            @lock.EnterReadLock();

            try
            {
                IMayBe<TItem> result;

                if (items.ContainsKey(key) && items[key].Expiry > dates.Invoke())
                {
                    result = new MayBe<TItem>(items[key].Item);
                }
                else
                {
                    result = MayBe<TItem>.Empty;
                }

                return Task.FromResult(result);
            }
            finally
            {
                @lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Determine if the current cache currently contains an item referenced by a given key.
        /// </summary>
        /// <param name="key">
        /// A value corresponding to the key of a potentially cached item.
        /// </param>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// A value indicating whether or not the current cache contains an object
        /// associated with the given key value.
        /// </returns>
        public Task<bool> IsCachedAsync(TKey key, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            @lock.EnterReadLock();

            try
            {
                return Task.FromResult(items.ContainsKey(key));
            }
            finally
            {
                @lock.ExitReadLock();
            }
        }
    }
}