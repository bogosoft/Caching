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

        ReaderWriterLockSlim @lock = new ReaderWriterLockSlim();

        TimeSpan ttl;

        /// <summary>
        /// Create a new instance of the <see cref="AsyncCache{TItem, TKey}"/> class
        /// with a given key extraction strategy.
        /// </summary>
        /// <param name="ttl">
        /// A value corresponding to the time to live for an object of the cached item type
        /// before it is considered stale.
        /// </param>
        public AsyncCache(TimeSpan ttl)
            : this(ttl, () => DateTimeOffset.Now)
        {
        }

        /// <summary>
        /// Create a new instance of the <see cref="AsyncCache{TItem, TKey}"/> class with a given key extraction
        /// strategy and a given <see cref="DateTimeOffset"/> provider.
        /// </summary>
        /// <param name="ttl">
        /// A value corresponding to the time to live for an object of the cached item type
        /// before it is considered stale.
        /// </param>
        /// <param name="dates">
        /// A provider for generating <see cref="DateTimeOffset"/> objects when a reference date is needed,
        /// i.e. when a staleness check is performed or when an expiration date needs to be calculated.
        /// </param>
        public AsyncCache(TimeSpan ttl, Func<DateTimeOffset> dates)
        {
            this.dates = dates;
            this.ttl = ttl;
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

            @lock.EnterWriteLock();

            try
            {
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

            @lock.EnterUpgradeableReadLock();

            try
            {
                if (items.ContainsKey(key))
                {
                    @lock.EnterWriteLock();

                    try
                    {
                        return Task.FromResult(items.Remove(key));
                    }
                    finally
                    {
                        @lock.ExitWriteLock();
                    }
                }
                else
                {
                    return Task.FromResult(false);
                }
            }
            finally
            {
                @lock.ExitUpgradeableReadLock();
            }
        }
    }
}