﻿using Bogosoft.Maybe;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Caching
{
    /// <summary>
    /// Indicates that an implementation is capable of caching items of a specified type.
    /// </summary>
    /// <typeparam name="TItem">The type of the item that can be cached.</typeparam>
    /// <typeparam name="TKey">The type of thekey used to retrieve a cached object of the item type.</typeparam>
    public interface ICache<TItem, TKey>
    {
        /// <summary>
        /// Cache an object of the item type.
        /// </summary>
        /// <param name="item">An object of the cached item type.</param>
        /// <param name="lifetime">
        /// A value corresponding to the lifetime of the cached item after which the item is considered stale.
        /// </param>
        /// <param name="token">a <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// A value indicating whether or not the caching operation succeeded.
        /// </returns>
        Task<bool> CacheAsync(TItem item, TimeSpan lifetime, CancellationToken token);

        /// <summary>
        /// Retrieve a previously cached item from the current cache by a given key.
        /// </summary>
        /// <param name="key">An object of the key type.</param>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// A value that might contain an object of the cached item type.
        /// </returns>
        Task<IMayBe<TItem>> GetAsync(TKey key, CancellationToken token);
    }
}