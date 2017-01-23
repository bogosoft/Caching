using Bogosoft.Maybe;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Caching
{
    /// <summary>
    /// Extended functionality for the <see cref="ICache{TItem, TKey}"/> contract.
    /// </summary>
    public static class CacheExtensions
    {
        /// <summary>
        /// Cache an object of the item type. Calling this method is equivalent to calling
        /// <see cref="ICache{TItem, TKey}.CacheAsync(TItem, TimeSpan, CancellationToken)"/>
        /// with a value of <see cref="CancellationToken.None"/>.
        /// </summary>
        /// <typeparam name="TItem">The type of the item that can be cached.</typeparam>
        /// <typeparam name="TKey">The type of thekey used to retrieve a cached object of the item type.</typeparam>
        /// <param name="cache">The current <see cref="ICache{TItem, TKey}"/> implementation.</param>
        /// <param name="item">An object of the cached item type.</param>
        /// <param name="lifetime">
        /// A value corresponding to the lifetime of the cached item after which the item is considered stale.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the caching operation succeeded.
        /// </returns>
        public static async Task<bool> CacheAsync<TItem, TKey>(
            this ICache<TItem, TKey> cache,
            TItem item,
            TimeSpan lifetime
            )
        {
            return await cache.CacheAsync(item, lifetime, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve a previously cached item from the current cache by a given key. Calling this method is
        /// equivalent to calling <see cref="ICache{TItem, TKey}.CacheAsync(TItem, TimeSpan, CancellationToken)"/>
        /// with a value of <see cref="CancellationToken.None"/>.
        /// </summary>
        /// <typeparam name="TItem">The type of the item that can be cached.</typeparam>
        /// <typeparam name="TKey">The type of thekey used to retrieve a cached object of the item type.</typeparam>
        /// <param name="cache">The current <see cref="ICache{TItem, TKey}"/> implementation.</param>
        /// <param name="key">An object of the key type.</param>
        /// <returns>
        /// A value that might contain an object of the cached item type.
        /// </returns>
        public static async Task<IMayBe<TItem>> GetAsync<TItem, TKey>(
            this ICache<TItem, TKey> cache,
            TKey key
            )
        {
            return await cache.GetAsync(key, CancellationToken.None).ConfigureAwait(false);
        }
    }
}