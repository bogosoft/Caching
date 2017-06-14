using Bogosoft.Maybe;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Bogosoft.Caching
{
    /// <summary>
    /// A synchronous, in-memory implementation of the <see cref="ICache{TItem, TKey}"/> contract.
    /// </summary>
    /// <typeparam name="TItem">The type of the item that can be cached.</typeparam>
    /// <typeparam name="TKey">The type of the key used to retrieve a cached object of the item type.</typeparam>
    public sealed class MemoryCache<TItem, TKey> : ICache<TItem, TKey>
    {
        Func<DateTimeOffset> dates;

        Dictionary<TKey, CachedItem<TItem>> items = new Dictionary<TKey, CachedItem<TItem>>();

        ReaderWriterLockSlim @lock = new ReaderWriterLockSlim();

        TimeSpan ttl;

        /// <summary>
        /// Occurs when an item is successfully cached.
        /// </summary>
        public event ItemCachedEventHandler<TItem, TKey> ItemCached;

        /// <summary>
        /// Create a new instance of the <see cref="MemoryCache{TItem, TKey}"/> class
        /// with a given key extraction strategy.
        /// </summary>
        /// <param name="ttl">
        /// A value corresponding to the time to live for an object of the cached item type
        /// before it is considered stale.
        /// </param>
        public MemoryCache(TimeSpan ttl)
            : this(ttl, () => DateTimeOffset.Now)
        {
        }

        /// <summary>
        /// Create a new instance of the <see cref="MemoryCache{TItem, TKey}"/> class with a given key extraction
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
        public MemoryCache(TimeSpan ttl, Func<DateTimeOffset> dates)
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
        /// <returns>
        /// A value indicating whether or not the caching operation succeeded.
        /// </returns>
        public bool Cache(TKey key, TItem item)
        {
            @lock.EnterWriteLock();

            try
            {
                items[key] = new CachedItem<TItem>
                {
                    Expiry = dates.Invoke().Add(ttl),
                    Item = item
                };

                if (items.ContainsKey(key))
                {
                    if (ItemCached != null)
                    {
                        ItemCached.Invoke(this, new ItemCachedEventArgs<TItem, TKey>(key, item));
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            finally
            {
                @lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Clear the current cache of all cached items.
        /// </summary>
        /// <returns>
        /// A value corresponding to the number of cached items cleared.
        /// </returns>
        public int Clear()
        {
            @lock.EnterWriteLock();

            try
            {
                var count = items.Count;

                items.Clear();

                return count;
            }
            finally
            {
                @lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Determine if the current cache currently contains a given key.
        /// </summary>
        /// <param name="key">
        /// An object of the key type.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the current cache contains the given key.
        /// </returns>
        public bool Contains(TKey key)
        {
            @lock.EnterReadLock();

            try
            {
                return items.ContainsKey(key);
            }
            finally
            {
                @lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Retrieve a previously cached item from the current cache by a given key.
        /// </summary>
        /// <param name="key">An object of the key type.</param>
        /// <returns>
        /// A value that might contain an object of the cached item type.
        /// </returns>
        public IMayBe<TItem> Get(TKey key)
        {
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

                return result;
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
        /// <returns>
        /// A value indicating whether or not the removal operation was successful.
        /// </returns>
        public bool Remove(TKey key)
        {
            @lock.EnterUpgradeableReadLock();

            try
            {
                if (items.ContainsKey(key))
                {
                    @lock.EnterWriteLock();

                    try
                    {
                        return items.Remove(key);
                    }
                    finally
                    {
                        @lock.ExitWriteLock();
                    }
                }
                else
                {
                    return false;
                }
            }
            finally
            {
                @lock.ExitUpgradeableReadLock();
            }
        }
    }
}