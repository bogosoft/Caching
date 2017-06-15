using Bogosoft.Maybe;
using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Caching
{
    /// <summary>
    /// Extended functionality for the <see cref="ICacheAsync{TItem, TKey}"/> contract.
    /// </summary>
    public static class AsyncCacheExtensions
    {
        /// <summary>
        /// Cache an object of the item type. Calling this method is equivalent to calling
        /// <see cref="ICacheAsync{TItem, TKey}.CacheAsync(TKey, TItem, CancellationToken)"/>
        /// with a value of <see cref="CancellationToken.None"/>.
        /// </summary>
        /// <typeparam name="TItem">The type of the item that can be cached.</typeparam>
        /// <typeparam name="TKey">The type of thekey used to retrieve a cached object of the item type.</typeparam>
        /// <param name="cache">The current <see cref="ICacheAsync{TItem, TKey}"/> implementation.</param>
        /// <param name="key">
        /// An object of the key type which will serve as the key by which an item is cached.
        /// </param>
        /// <param name="item">An object of the cached item type.</param>
        /// <returns>
        /// A value indicating whether or not the caching operation succeeded.
        /// </returns>
        public static Task<bool> CacheAsync<TItem, TKey>(
            this ICacheAsync<TItem, TKey> cache,
            TKey key,
            TItem item
            )
        {
            return cache.CacheAsync(key, item, CancellationToken.None);
        }

        /// <summary>
        /// Clear the current cache of all cached items.
        /// </summary>
        /// <typeparam name="TItem">The type of the item that can be cached.</typeparam>
        /// <typeparam name="TKey">The type of thekey used to retrieve a cached object of the item type.</typeparam>
        /// <param name="cache">The current <see cref="ICacheAsync{TItem, TKey}"/> implementation.</param>
        /// <returns>
        /// A value corresponding to the number of cached items cleared.
        /// </returns>
        public static Task<int> ClearAsync<TItem, TKey>(this ICacheAsync<TItem, TKey> cache)
        {
            return cache.ClearAsync(CancellationToken.None);
        }

        /// <summary>
        /// Determine if the current cache currently contains a given key. Calling this method
        /// is equivalent to calling <see cref="ICacheAsync{TItem, TKey}.ContainsKeyAsync(TKey, CancellationToken)"/>
        /// with a value of <see cref="CancellationToken.None"/>.
        /// </summary>
        /// <typeparam name="TItem">The type of the item that can be cached.</typeparam>
        /// <typeparam name="TKey">The type of the key used to retrieve a cached object of the item type.</typeparam>
        /// <param name="cache">The current <see cref="ICacheAsync{TItem, TKey}"/> implementation.</param>
        /// <param name="key">An object of the key type.</param>
        /// <returns>
        /// A value indicating whether or not the current cache contains the given key.
        /// </returns>
        public static Task<bool> ContainsKeyAsync<TItem, TKey>(
            this ICacheAsync<TItem, TKey> cache,
            TKey key
            )
        {
            return cache.ContainsKeyAsync(key, CancellationToken.None);
        }

        /// <summary>
        /// Retrieve a previously cached item from the current cache by a given key. Calling this method is
        /// equivalent to calling <see cref="ICacheAsync{TItem, TKey}.GetAsync(TKey, CancellationToken)"/>
        /// with a value of <see cref="CancellationToken.None"/>.
        /// </summary>
        /// <typeparam name="TItem">The type of the item that can be cached.</typeparam>
        /// <typeparam name="TKey">The type of thekey used to retrieve a cached object of the item type.</typeparam>
        /// <param name="cache">The current <see cref="ICacheAsync{TItem, TKey}"/> implementation.</param>
        /// <param name="key">An object of the key type.</param>
        /// <returns>
        /// A value that might contain an object of the cached item type.
        /// </returns>
        public static Task<MayBe<TItem>> GetAsync<TItem, TKey>(
            this ICacheAsync<TItem, TKey> cache,
            TKey key
            )
        {
            return cache.GetAsync(key, CancellationToken.None);
        }

        /// <summary>
        /// Attempt to remove a previously cached item from the current cache. Calling this method is equivalent
        /// to calling <see cref="ICacheAsync{TItem, TKey}.RemoveAsync(TKey, CancellationToken)"/> with a value of
        /// <see cref="CancellationToken.None"/>.
        /// </summary>
        /// <typeparam name="TItem">The type of the item that can be cached.</typeparam>
        /// <typeparam name="TKey">The type of the key used to retrieve a cached object of the item type.</typeparam>
        /// <param name="cache">The current <see cref="ICacheAsync{TItem, TKey}"/> implementation.</param>
        /// <param name="key">An object of the key type.</param>
        /// <returns>
        /// A value indicating whether or not the removal operation was successful.
        /// </returns>
        public static Task<bool> RemoveAsync<TItem, TKey>(
            this ICacheAsync<TItem, TKey> cache,
            TKey key
            )
        {
            return cache.RemoveAsync(key, CancellationToken.None);
        }
    }
}